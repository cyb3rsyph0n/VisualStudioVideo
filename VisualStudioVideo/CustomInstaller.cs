using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace VisualStudioVideo
{
    [RunInstaller(true)]
    public partial class CustomInstaller : Installer
    {
        string addinFolder = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Visual Studio 2008\Addins\");
        string addinPath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Visual Studio 2008\Addins\VisualStudioVideo.AddIn");

        public CustomInstaller()
        {
            InitializeComponent();
        }

        public override void Commit(System.Collections.IDictionary savedState)
        {
            base.Commit(savedState);
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);

            string installationPath = base.Context.Parameters["AssemblyPath"];
            string addinResourceFile = Assembly.GetExecutingAssembly().GetName().Name + ".VisualStudioVideo.AddIn";
            XDocument linqXmlDocument;

            using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(addinResourceFile))
            {
                linqXmlDocument = XDocument.Load(XmlReader.Create(resourceStream));
            }

            var query = from assemblyNode in linqXmlDocument.Descendants()
                        where assemblyNode.Name.LocalName.ToLower().Equals("assembly")
                        select assemblyNode;

            foreach (XElement assemblyNode in query)
            {
                assemblyNode.SetValue(installationPath);
            }

            string addinXml = linqXmlDocument.ToString();
            System.IO.Directory.CreateDirectory(addinFolder);
            File.WriteAllText(addinPath, addinXml);
        }

        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            base.Uninstall(savedState);
            try
            {
                File.Delete(addinPath);
            }
            catch
            {
            }
        }

        public override void Rollback(System.Collections.IDictionary savedState)
        {
            base.Rollback(savedState);
            try
            {
                File.Delete(addinPath);
            }
            catch
            {
            }
        }
    }
}
