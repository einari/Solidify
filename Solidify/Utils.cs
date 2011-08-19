using System.Xml;
using System.Xml.Linq;

namespace Solidify
{
    public class Utils
    {
        public static XmlDocument NewXmlDocument(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc;
        }

        public static void CopyNode(XmlDocument recipient, XmlNode nodeToCopy)
        {
            XmlNode copiedNode = recipient.ImportNode(nodeToCopy, true);
            recipient.DocumentElement.AppendChild(copiedNode);
        }

        public static XmlDocument ToXmlDocument(XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

        public static XDocument ToXDocument(XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }
    }
}