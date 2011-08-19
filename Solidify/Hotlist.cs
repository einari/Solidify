using System.Configuration;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace Solidify
{
    public class Hotlist
    {
        static string fullPath;
        static string theDirectory; 

        static Hotlist()
        {
            fullPath = Assembly.GetCallingAssembly().Location;
            
            theDirectory = Path.GetDirectoryName(fullPath);

            if (!Directory.Exists(GetEnvironmentDirectoryPath("U01")))
                Directory.CreateDirectory(GetEnvironmentDirectoryPath("U01"));

            if (!Directory.Exists(GetEnvironmentDirectoryPath("T01")))
                Directory.CreateDirectory(GetEnvironmentDirectoryPath("T01"));

            if (!Directory.Exists(GetEnvironmentDirectoryPath("P01")))
                Directory.CreateDirectory(GetEnvironmentDirectoryPath("P01"));
        }

        public static void Save(XDocument hotlist, string filename)
        {
            hotlist.Save(GetFullFileNameWithPath(filename));
        }

        public static XDocument Get(string filename)
        {
            return XDocument.Load(GetFullFileNameWithPath(filename), LoadOptions.PreserveWhitespace);
        }

        public static string GetFullFileNameWithPath(string filename)
        {
            string filenameAndPath = GetEnvironmentDirectoryPath(ConfigurationManager.AppSettings["SYSNAM"]) + filename;
            return filenameAndPath;
        }

        public static string GetEnvironmentDirectoryPath(string environment)
        {
            return theDirectory + @"\Output\" + environment + @"\";
        }
    }
}