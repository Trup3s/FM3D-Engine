﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.VCCodeModel;
using EnvDTE;
using System.Windows.Forms;

namespace VS_Extension
{
    public class CodeManipulator
    {
        VCFileCodeModel model;
        public CodeManipulator(string filename)
        {
            if (!FindModel(MainPackage.Instance.MainProject.ProjectItems, filename))
                throw new ArgumentException("Not a valid filename for a code file", "filename");
        }

        private bool FindModel(ProjectItems p, string filename)
        {

            foreach (ProjectItem pi in p)
            {
                if (pi.Name == filename)
                {
                    model = pi.FileCodeModel as VCFileCodeModel;
                    return true;
                }
                else
                {
                    if (FindModel(pi.ProjectItems, filename)) return true;
                }
            }
            return false;
        }

        public CodeClass AddClass(string name, object bases = null)
        {
            return model.AddClass(name, 0, bases);
        }

        public bool AddAttribute(string method, string atname, string atvalue) {

            MessageBox.Show(model.Namespaces.ToString());
           
            if (!model.Namespaces.ToString().Contains("EntitySpace")) {
                model.AddNamespace("EntitySpace");
            } else { MessageBox.Show("NUUUUUU"); }

            //if (model.ValidateMember("Method1", vsCMElement.vsCMElementFunction, type)) {
            //    classElement.AddFunction("Method1", vsCMFunction.vsCMFunctionFunction, type);
            //}
            return true;
        }
    }
}
