﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM3D_Designer.src.Dialogs
{

    public class DialogBase : BaseMetroDialog
    {
        public MetroWindow Window { get; private set; }
        protected DialogBase(MetroWindow win)
        {
            Window = win;
        }

        public void Close()
        {
            Window.HideMetroDialogAsync(this);
        }
    }

    public static class DialogManager
    {
        public static void ShowNewResourceDialog(this MetroWindow window)
        {
            window.ShowMetroDialogAsync(new NewResourceDialog(window));
        }

        public static void ShowEntityEditor(this MetroWindow window, bool isnewentity, string path)
        {
            window.ShowMetroDialogAsync(new EntityEditor(window, path, isnewentity ));
        }

        public static void ShowAddFileDialog(this MetroWindow window, String path)
        {
            window.ShowMetroDialogAsync(new AddFileDialog(window, path));
        }

        public static void ShowModelDialog(this MetroWindow window, String path)
        {
            window.ShowMetroDialogAsync(new ModelDialog(window, path));
        }
        
    }
}