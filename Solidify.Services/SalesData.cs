using System;
using System.Xml;
using Solidify.Services.Data;

namespace Solidify.Services
{
    public class SalesData
    {
        static XmlDocument _salesData = DataHelper.GetXmlDocument("Solidify.Services.Data.SalesData.xml");

        public static XmlDocument GetSalesData(DateTime from, DateTime to, string environment)
        {
            return _salesData;
        }

        public static XmlDocument GetSalesData(DateTime from, string environment)
        {
            return _salesData;
        }

    }
}