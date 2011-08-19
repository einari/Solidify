using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Solidify.Services.Data
{
    public static class DataHelper
    {
        public static XmlDocument GetXmlDocument(string path)
        {
            var target = new XmlDocument();
            target.Load(XmlReader.Create(GetXmlStream(path, Assembly.GetCallingAssembly())));
            return target;
        }

        public static XDocument GetXDocument(string path)
        {
            return XDocument.Load(XmlReader.Create(GetXmlStream(path, Assembly.GetEntryAssembly())));
        }

        private static Stream GetXmlStream(string path, Assembly assembly)
        {
            var xmlStream = assembly.GetManifestResourceStream(path);

            if (xmlStream == null)
                throw new FileNotFoundException(String.Format("The Xml file was not found: {0}", path ?? string.Empty));

            return xmlStream;
        }
    }
}