using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using Solidify.Services;
using Solidify.Services.Data;

namespace Solidify
{
	public class HotlistGenerator
	{
		public static void Main (string[] args)
		{
		    try
		    {
                Console.WriteLine("Running hotlist generator in " + ConfigurationManager.AppSettings["SYSNAM"]);
                
		        //Generate the hot lists
                GenerateHotLists(DateTime.Now);
		        
                Console.WriteLine("Finished running hotlist generator in " + ConfigurationManager.AppSettings["SYSNAM"]);
                Console.WriteLine();
                Console.WriteLine("Press a key...");
                Console.ReadKey();
		    }
		    catch (Exception ex)
		    {
                Console.WriteLine("********* ERROR *********");
                Console.WriteLine("Exception: " + ex);
		        Console.WriteLine("********* ERROR *********");
                Console.WriteLine();
                Console.WriteLine("Press a key...");
                Console.ReadKey();
		    }	
		}

	    public static void GenerateHotLists(DateTime dateToGenerateFor)
	    {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

	        XmlDocument browseNodesXmlDoc;
	        XmlDocument productsXmlDoc;
	        XmlDocument newSalesDataXmlDoc;

	        if(Assembly.GetCallingAssembly().FullName.StartsWith("Solidify.Tests"))
	        {
                Console.WriteLine("\t\t\tGetting the test data...");
                //Get the categories
                browseNodesXmlDoc = DataHelper.GetXmlDocument("Solidify.TestData.Categories.xml");
                //Get the products
                productsXmlDoc = DataHelper.GetXmlDocument("Solidify.TestData.Products.xml");
                //Get the sales data
	            newSalesDataXmlDoc = DataHelper.GetXmlDocument("Solidify.TestData.SalesData.xml");
                Console.WriteLine("\t\t\tFinished getting the test data...");

                //tests expect the date to be 16/08/2011
                dateToGenerateFor = new DateTime(2011,8,16);
	        }
	        else
	        {
                Console.WriteLine("\t\t\tGetting the data...");
                //Get the categories
                browseNodesXmlDoc = Catalog.GetBrowseNodes(ConfigurationManager.AppSettings["SYSNAM"]);
                //Get the products
                productsXmlDoc = Catalog.GetProducts(ConfigurationManager.AppSettings["SYSNAM"]);
                //Get the sales data
                DateTime from = new DateTime(2011, 1, 1);
                newSalesDataXmlDoc = SalesData.GetSalesData(from, ConfigurationManager.AppSettings["SYSNAM"]);
                Console.WriteLine("\t\t\tFinished getting the data...");
	        }
            
            Console.WriteLine("\t\t\t\tNumber of browsenodes: " + browseNodesXmlDoc.DocumentElement.ChildNodes.Count);
            Console.WriteLine("\t\t\t\tNNumber of products: " + productsXmlDoc.DocumentElement.ChildNodes.Count);
            Console.WriteLine("\t\t\t\tNNumber of sales entries: " + newSalesDataXmlDoc.DocumentElement.ChildNodes.Count);

            Console.WriteLine("\tGenerating hotlist for all stores");
	        foreach(Store store in Stores.All)
	        {
	            Console.WriteLine(string.Format("\t\tGenerating Hotlist for Store {0}", store.StoreId));

                //Get the browse nodes for this store
	            XmlDocument storeBrowseNodesXmlDoc = Utils.NewXmlDocument("<rows />");
                foreach(XmlNode nodes in browseNodesXmlDoc.DocumentElement.SelectNodes((@"row[@storeId=" + store.StoreId + "]")))
	            {
	                Utils.CopyNode(storeBrowseNodesXmlDoc,nodes);
	            }
                //Get the products nodes for this store
                XmlDocument storeProductsXmlDoc = Utils.NewXmlDocument("<rows />");
                foreach (XmlNode nodes in productsXmlDoc.DocumentElement.SelectNodes((@"row[@StoreId=" + store.StoreId + "]")))
                {
                    Utils.CopyNode(storeProductsXmlDoc, nodes);
                }
                //Get the sales nodes for this store
                XmlDocument storeSalesEntriesXmlDoc = Utils.NewXmlDocument("<rows />");
                foreach (XmlNode nodes in newSalesDataXmlDoc.DocumentElement.SelectNodes((@"row[@StoreId=" + store.StoreId + "]")))
                {
                    Utils.CopyNode(storeSalesEntriesXmlDoc, nodes);
                }
	            Console.WriteLine(string.Format("\t\t\tNumber of browsenodes for store {0}: {1}", store.StoreId,
	                                            storeBrowseNodesXmlDoc.DocumentElement.ChildNodes.Count));
	            Console.WriteLine(string.Format("\t\t\tNumber of products for store {0}: {1}", store.StoreId,
	                                            storeProductsXmlDoc.DocumentElement.ChildNodes.Count));
	            Console.WriteLine(string.Format("\t\t\tNumber of sales data entries for store {0}: {1}", store.StoreId,
	                                            storeSalesEntriesXmlDoc.DocumentElement.ChildNodes.Count));

	            CalculateTopThreeSellersPerBrowseNodePerMonthAndPerWeek(store, storeBrowseNodesXmlDoc, storeProductsXmlDoc, storeSalesEntriesXmlDoc, dateToGenerateFor);

	            Console.WriteLine(string.Format("\t\tFinished generating hotlist for Store {0}", store.StoreId));
	        }
            Console.WriteLine("\tFinished generating hotlist for all stores");
            Console.WriteLine();
	    }

	    static void CalculateTopThreeSellersPerBrowseNodePerMonthAndPerWeek(Store store, XmlDocument storeBrowseNodesXmlDoc, 
                                                                                XmlDocument storeProductsXmlDoc, XmlDocument storeSalesEntriesXmlDoc,
                                                                                    DateTime dateToGenerateFor)
	    {
            //convert the xml document to xdocument so that we can perform linq to xml queries
	        XDocument browseNodesDoc = Utils.ToXDocument(storeBrowseNodesXmlDoc);
            XDocument productsDoc = Utils.ToXDocument(storeProductsXmlDoc);
            XDocument salesDoc = Utils.ToXDocument(storeSalesEntriesXmlDoc);

            //get the current week and month for filtering sales data on
            int thisYear = dateToGenerateFor.Year;
            int thisMonth = dateToGenerateFor.Month;
            int today = dateToGenerateFor.Day;
	        int currentWeek = GetWeekInYear(thisYear, thisMonth, today);

            // determine out of stock products
	        string[] productsOutOfStock = (from product in productsDoc.Descendants("row")
	                                       where decimal.Parse(product.Attribute("QuantityForSale").Value) < 1
	                                       select product.Attribute("MATNR").Value).ToArray();

            // determine out of stock products which are not going to be in stock within a week
            string[] productsOutOfStockAndNotComingIn = (from product in productsDoc.Descendants("row")
                                                           where decimal.Parse(product.Attribute("QuantityForSale").Value) < 1
                                                                    && !(decimal.Parse(product.Attribute("QuantityForSale1").Value) > 0m
                                                                            //due in within 7 days
                                                                            && DateTime.Parse(product.Attribute("AvailableDate1").Value) <= DateTime.Now.AddDays(7)) 
                                                           select product.Attribute("MATNR").Value).ToArray();

            //remove cancelled sales lines
            salesDoc.Descendants("row").Where(r => r.Attribute("ItemStatus").Value == "CD").Remove();
            
            //remove products that are not to be counted
            string[] productsToRemove = (from product in productsDoc.Descendants("row")
                                   where (product.Attribute("SPART").Value == "90" || product.Attribute("SPART").Value == "91" || product.Attribute("SPART").Value == "92") // demo products
                                            || ((store.StoreId == 325) && decimal.Parse(product.Attribute("ZCPRICE").Value) < 1000m) // itegra products less than 1000kr
                                            || ((store.StoreId == 311 || store.StoreId == 318) && decimal.Parse(product.Attribute("ZCPRICE").Value) < 500m) // norek.no or norek.se less than 500
                                            || (product.Attribute("VMSTA").Value == "03") // End of life products
                                   select (string)product.Attribute("MATNR")).ToArray();

            salesDoc.Descendants("row").Where(r => productsToRemove.Contains((string)r.Attribute("SKU"))).Remove();

            //remove sales for customer groups that we do not want
            /*
            * Customer Groups
            * 
            * 10 - public sector
            * 20 - commerical
            * 30 - dealer
            * 40 - private
            * 50 - employee
            */
            salesDoc.Descendants("row").Where(r => r.Attribute("KDGRP").Value == "10" || //Public Sector
                                                ((store.StoreType == "B2C") 
                                                && (r.Attribute("KDGRP").Value == "20" || r.Attribute("KDGRP").Value == "30"))) // customer-facing store and business customer
                                                .Remove(); 

            //work on a copy of the sales data when generating sales to be included in monthly hotlist calculation
            TextReader tr = new StringReader(salesDoc.ToString());
            XDocument salesForMonthHotlist = XDocument.Load(tr);
            // remove the sales of out of stock and not coming in products and sales that are not within the current month
            salesForMonthHotlist.Descendants("row").Where(r => productsOutOfStockAndNotComingIn.Contains(r.Attribute("SKU").Value)
                || (int.Parse(r.Attribute("Year").Value) != thisYear || int.Parse(r.Attribute("Month").Value) != thisMonth)
                ).Remove();

            //work on a copy of the sales data when generating sales to be included in weekly hotlist calculation
            TextReader tr2 = new StringReader(salesDoc.ToString());
            XDocument salesForWeekHotlist = XDocument.Load(tr2);
            // remove the sales of out of stock products and sales that are not within the current week
            salesForWeekHotlist.Descendants("row").Where(r => productsOutOfStock.Contains(r.Attribute("SKU").Value)
                || GetWeekInYear(int.Parse(r.Attribute("Year").Value), int.Parse(r.Attribute("Month").Value), int.Parse(r.Attribute("Day").Value)) != currentWeek
                ).Remove();

	        GenerateHotList(salesForMonthHotlist, browseNodesDoc, productsDoc, "Month", store.StoreId);
            GenerateHotList(salesForWeekHotlist, browseNodesDoc, productsDoc, "Week", store.StoreId);
	    }

        static int GetWeekInYear(int year, int month, int day)
        {
            DateTime date = new DateTime(year,month,day);
            int week = Thread.CurrentThread.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return week;
        }

        static void GenerateHotList(XDocument sales, XDocument browseNodes, XDocument products, string mode, int storeId)
        {
            foreach(var category in browseNodes.Descendants("row"))
            {
                int browseNodeId = int.Parse(category.Attribute("BrowseNodeId").Value);

                var topSellingProductsByCategory = from sale in sales.Descendants("row")
                                                     join product in products.Descendants("row")
                                                            on sale.Attribute("SKU").Value equals product.Attribute("MATNR").Value
                                                     join cat in browseNodes.Descendants("row")
                                                            on product.Attribute("BrowseNodeId").Value equals cat.Attribute("BrowseNodeId").Value
                                                     where int.Parse(cat.Attribute("BrowseNodeId").Value) == browseNodeId
                                                     group sale by new { ProductId = sale.Attribute("SKU").Value, Product = product.Attribute("MAKTX").Value } into aggregatedData
                                                     select new
                                                                {
                                                                    ProductId = (string)aggregatedData.Key.ProductId,
                                                                    TotalSales = aggregatedData.Sum(s => int.Parse(s.Attribute("Count").Value)),
                                                                    Product = aggregatedData.Key.Product
                                                                };

                var hotlist = topSellingProductsByCategory.OrderByDescending(i => i.TotalSales).Take(3);

                XDocument hotListXml = new XDocument(
                                                        new XDeclaration("1.0", "utf-8", null),
                                                        new XElement("Hotlist",
                                                            new XAttribute("store", storeId),
                                                            new XAttribute("category", browseNodeId),
                                                            new XAttribute("type", mode),
                                                            hotlist.Select(hl => new XElement("item",
                                                                new XAttribute("ProductId", hl.ProductId),
                                                                new XAttribute("Product", hl.Product),
                                                                new XAttribute("TotalSales", hl.TotalSales)
                                                            ))
                                                          )
                                                        );

                string fileName = string.Format("{0}_Hotlist_{1}_Category_{2}.xml", storeId, mode, browseNodeId);
                Hotlist.Save(hotListXml, fileName);
            }
        }
	}
}

