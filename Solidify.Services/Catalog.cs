using System.Xml;
using Solidify.Services.Data;

namespace Solidify.Services
{
    public class Catalog
    {
        static readonly XmlDocument _categories = DataHelper.GetXmlDocument("Solidify.Services.Data.Categories.xml");
        static readonly XmlDocument _products = DataHelper.GetXmlDocument("Solidify.Services.Data.Products.xml");

        public static XmlDocument GetBrowseNodes(string environment)
        {
            return _categories;
        }

        public static XmlDocument GetProducts(string environment)
        {
            return _products;
        }
    }
}