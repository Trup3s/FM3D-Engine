﻿using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System.IO;
using System.Collections.ObjectModel;

namespace FM3D_Designer.src
{
    public static class VisualStudio
    {
        private static class PipeSystem
        {
            enum Message : UInt16
            {
                GET_COMPONENTS,
            }

            private static Mutex mutex = new Mutex();
            private static NamedPipeServerStream pipe;
            private static byte[] readBuffer = new byte[4096];

            private static bool _IsStarting = false;
            private static bool _IsStarted = false;

            private static Process process;

            public static bool IsStarting
            {
                get
                {
                    lock (mutex)
                    {
                        return _IsStarting;
                    }
                }
            }
            public static bool IsStarted
            {
                get
                {
                    lock (mutex)
                    {
                        return _IsStarted;
                    }
                }
            }

            private static bool IsActive
            {
                get
                {
                    return IsStarting || IsStarted;
                }
            }

            public static void StartVS(string pipename, string solution)
            {
                if (IsActive) return;
                lock (mutex)
                {
                    _IsStarting = true;
                }
                try
                {
                    process = Process.Start("devenv.exe", "\"" + solution + "\"");
                    process.Exited += OnVSCloses;
                    process.EnableRaisingEvents = true;
                }
                catch (Exception e)
                {
                    lock (mutex)
                    {
                        _IsStarting = false;
                    }
                    MainWindow.Instance.Invoke(new Action(() => { MainWindow.Instance.ShowMessageAsync("FM3D-Designer-Error", "Could not start Visual Studio!\n" + e.Message, MessageDialogStyle.Affirmative); }));
                    return;
                }
                pipe = new NamedPipeServerStream(pipename, PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
                pipe.BeginWaitForConnection(new AsyncCallback(OnVSStarted), pipe);

            }

            public static void CloseVS()
            {
                lock (mutex)
                {
                    if (process != null)
                    {
                        process.Exited -= OnVSCloses;

                        process.CloseMainWindow();
                        process.Close();
                    }
                }
                _CloseVS();
            }
            private static void _CloseVS()
            {
                lock (mutex)
                {
                    if (process != null)
                    {
                        process.Dispose();
                        process = null;
                    }

                    _IsStarting = false;
                    _IsStarted = false;

                    if (pipe != null)
                    {
                        if (pipe.IsConnected)
                            pipe.Disconnect();
                        pipe.Close();
                        pipe.Dispose();
                        pipe = null;
                    }

                }
            }

            private static void OnVSStarted(IAsyncResult res)
            {
                try
                {
                    ((NamedPipeServerStream)res.AsyncState).EndWaitForConnection(res);
                }
                catch (ObjectDisposedException)
                {
                    lock (mutex)
                    {
                        _IsStarting = false;
                        _IsStarted = false;
                    }
                    return;
                }
                lock (mutex)
                {
                    _IsStarting = false;
                    _IsStarted = true;
                }
                Read();
            }

            private static void OnVSCloses(object sender, EventArgs e)
            {
                _CloseVS();
                MainWindow.Instance.Invoke(new Action(() => { MainWindow.Instance.ShowMessageAsync("Visual Studio Info", "Lost Connection of Visual Studio. Maybe it was closed?\nYou can restart it from the toolbar.", MessageDialogStyle.Affirmative); }));
            }

            private static void Read()
            {
                try
                {
                    pipe.BeginRead(readBuffer, 0, readBuffer.Length, ReadCallback, null);
                }
                catch (InvalidOperationException) //disconnected/disposed
                {
                    return;
                }
            }

            private static void ReadCallback(IAsyncResult ar)
            {
                int bytesRead;

                try
                {
                    bytesRead = pipe.EndRead(ar);
                }
                catch (IOException) //closed
                {
                    return;
                }

                MessageBox.Show(bytesRead.ToString(), "FM3D-Extension-INFO");

                Read();
            }

            private static void Write(byte[] buffer)
            {
                try
                {
                    pipe.BeginWrite(buffer, 0, buffer.Length, WriteCallback, null);
                }
                catch (InvalidOperationException) //disconnected/disposed
                {
                    return;
                }
            }

            private static void WriteCallback(IAsyncResult ar)
            {
                try
                {
                    pipe.EndWrite(ar);
                }
                catch (IOException) //closed
                {
                    return;
                }
            }

            public static ObservableCollection<string> GetComponents()
            {
                byte[] buffer = BitConverter.GetBytes((ushort)Message.GET_COMPONENTS);
                Write(buffer);
                return null;
            }
        }
    }
}
