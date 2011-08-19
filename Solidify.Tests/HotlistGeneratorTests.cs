using System;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using System.Xml;

namespace Solidify.Tests
{
	[TestFixture]
	public class HotlistGeneratorTests
	{
       static string hotlistLocationString = "{0}_Hotlist_{1}_Category_{2}.xml";

		[Test]
		public void ShouldGenerateHotlistsCorrectly()
		{
            HotlistGenerator.GenerateHotLists(new DateTime(2011, 8, 16));

            #region Should build the komplett no hotlists correctly
            XDocument week_category_1_komplett_no = Hotlist.Get(string.Format(hotlistLocationString, 310, "week", 1));
            XDocument week_category_2_komplett_no = Hotlist.Get(string.Format(hotlistLocationString, 310, "week", 2));
            XDocument month_category_1_komplett_no = Hotlist.Get(string.Format(hotlistLocationString, 310, "month", 1));
            XDocument month_category_2_komplett_no = Hotlist.Get(string.Format(hotlistLocationString, 310, "month", 2));

            Assert.AreEqual(TopSellingProduct(week_category_1_komplett_no), 5);
            Assert.AreEqual(TopSellingProduct(week_category_2_komplett_no), 10);
            Assert.AreEqual(TopSellingProduct(month_category_1_komplett_no), 5);
            Assert.AreEqual(TopSellingProduct(month_category_2_komplett_no), 81); 
            #endregion

            #region Should build the norek no hotlists correctly
            XDocument week_category_1_norek_no = Hotlist.Get(string.Format(hotlistLocationString, 311, "week", 1));
            XDocument week_category_2_norek_no = Hotlist.Get(string.Format(hotlistLocationString, 311, "week", 2));
            XDocument month_category_1_norek_no = Hotlist.Get(string.Format(hotlistLocationString, 311, "month", 1));
            XDocument month_category_2_norek_no = Hotlist.Get(string.Format(hotlistLocationString, 311, "month", 2));

            Assert.AreEqual(TopSellingProduct(week_category_1_norek_no), 15);
            Assert.AreEqual(TopSellingProduct(week_category_2_norek_no), 91);
            Assert.AreEqual(TopSellingProduct(month_category_1_norek_no), 15);
            Assert.AreEqual(TopSellingProduct(month_category_2_norek_no), 91); 
            #endregion

            #region Should build the komplett se hotlists correctly
            XDocument week_category_1_komplett_se = Hotlist.Get(string.Format(hotlistLocationString, 312, "week", 1));
            XDocument week_category_2_komplett_se = Hotlist.Get(string.Format(hotlistLocationString, 312, "week", 2));
            XDocument month_category_1_komplett_se = Hotlist.Get(string.Format(hotlistLocationString, 312, "month", 1));
            XDocument month_category_2_komplett_se = Hotlist.Get(string.Format(hotlistLocationString, 312, "month", 2));

            Assert.AreEqual(TopSellingProduct(week_category_1_komplett_se), 25);
            Assert.AreEqual(TopSellingProduct(week_category_2_komplett_se), 30);
            Assert.AreEqual(TopSellingProduct(month_category_1_komplett_se), 25);
            Assert.AreEqual(TopSellingProduct(month_category_2_komplett_se), 30); 
            #endregion

            #region Should build the norek se hotlists correctly
            XDocument week_category_1_norek_se = Hotlist.Get(string.Format(hotlistLocationString, 318, "week", 1));
            XDocument week_category_2_norek_se = Hotlist.Get(string.Format(hotlistLocationString, 318, "week", 2));
            XDocument month_category_1_norek_se = Hotlist.Get(string.Format(hotlistLocationString, 318, "month", 1));
            XDocument month_category_2_norek_se = Hotlist.Get(string.Format(hotlistLocationString, 318, "month", 2));

            Assert.AreEqual(TopSellingProduct(week_category_1_norek_se), 35);
            Assert.AreEqual(TopSellingProduct(week_category_2_norek_se), 40);
            Assert.AreEqual(TopSellingProduct(month_category_1_norek_se), 35);
            Assert.AreEqual(TopSellingProduct(month_category_2_norek_se), 40); 
            #endregion

            #region Should build the komplett dk hotlists correctly
            XDocument week_category_1_komplett_dk = Hotlist.Get(string.Format(hotlistLocationString, 321, "week", 1));
            XDocument week_category_2_komplett_dk = Hotlist.Get(string.Format(hotlistLocationString, 321, "week", 2));
            XDocument month_category_1_komplett_dk = Hotlist.Get(string.Format(hotlistLocationString, 321, "month", 1));
            XDocument month_category_2_komplett_dk = Hotlist.Get(string.Format(hotlistLocationString, 321, "month", 2));

            Assert.AreEqual(TopSellingProduct(week_category_1_komplett_dk), 45);
            Assert.AreEqual(TopSellingProduct(week_category_2_komplett_dk), 50);
            Assert.AreEqual(TopSellingProduct(month_category_1_komplett_dk), 45);
            Assert.AreEqual(TopSellingProduct(month_category_2_komplett_dk), 50); 
            #endregion
            
            #region Should build the inwarehouse hotlists correctly
            XDocument week_category_1_inwarehouse = Hotlist.Get(string.Format(hotlistLocationString, 323, "week", 1));
            XDocument week_category_2_inwarehouse = Hotlist.Get(string.Format(hotlistLocationString, 323, "week", 2));
            XDocument month_category_1_inwarehouse = Hotlist.Get(string.Format(hotlistLocationString, 323, "month", 1));
            XDocument month_category_2_inwarehouse = Hotlist.Get(string.Format(hotlistLocationString, 323, "month", 2));

            Assert.AreEqual(TopSellingProduct(week_category_1_inwarehouse), 55);
            Assert.AreEqual(TopSellingProduct(week_category_2_inwarehouse), 60);
            Assert.AreEqual(TopSellingProduct(month_category_1_inwarehouse), 55);
            Assert.AreEqual(TopSellingProduct(month_category_2_inwarehouse), 60); 
            #endregion
            
            #region Should build the mpx hotlists correctly
            XDocument week_category_1_mpx = Hotlist.Get(string.Format(hotlistLocationString, 324, "week", 1));
            XDocument week_category_2_mpx = Hotlist.Get(string.Format(hotlistLocationString, 324, "week", 2));
            XDocument month_category_1_mpx = Hotlist.Get(string.Format(hotlistLocationString, 324, "month", 1));
            XDocument month_category_2_mpx = Hotlist.Get(string.Format(hotlistLocationString, 324, "month", 2));

            Assert.AreEqual(TopSellingProduct(week_category_1_mpx), 65);
            Assert.AreEqual(TopSellingProduct(week_category_2_mpx), 70);
            Assert.AreEqual(TopSellingProduct(month_category_1_mpx), 65);
            Assert.AreEqual(TopSellingProduct(month_category_2_mpx), 70); 
            #endregion

            #region Should build the itegra hotlists correctly
            XDocument week_category_1_itegra = Hotlist.Get(string.Format(hotlistLocationString, 325, "week", 1));
            XDocument week_category_2_itegra = Hotlist.Get(string.Format(hotlistLocationString, 325, "week", 2));
            XDocument month_category_1_itegra = Hotlist.Get(string.Format(hotlistLocationString, 325, "month", 1));
            XDocument month_category_2_itegra = Hotlist.Get(string.Format(hotlistLocationString, 325, "month", 2));

            Assert.AreEqual(TopSellingProduct(week_category_1_itegra), 75);
            Assert.AreEqual(TopSellingProduct(week_category_2_itegra), 101);
            Assert.AreEqual(TopSellingProduct(month_category_1_itegra), 75);
            Assert.AreEqual(TopSellingProduct(month_category_2_itegra), 101); 
            #endregion
		}

        private static int TopSellingProduct(XDocument hotlist)
        {
            return int.Parse(hotlist.Descendants("item").First().Attribute("ProductId").Value);
        }
	}
}

