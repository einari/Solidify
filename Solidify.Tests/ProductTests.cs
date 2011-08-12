using System;
using NUnit.Framework;
using System.Xml;

namespace Solidify.Tests
{
	[TestFixture]
	public class ProductTests
	{
		[Test]
		public void ShouldProcessProductsCorrectly()
		{
            XmlDocument productsXmlDocument = Products.Load();

            Processor.ProcessKomplettProducts(productsXmlDocument);
            Processor.ProcessNorekProducts(productsXmlDocument);
            Processor.ProcessItegraProducts(productsXmlDocument);
            Processor.ProcessMpxProducts(productsXmlDocument);

            XmlDocument komplettProducts = Products.Load(310);
            int count = komplettProducts.SelectNodes[].Count;
            Assert.AreEqual(count,42);

            XmlDocument norekProducts = productsXmlDocument.Load(311);
            count = norekProducts.SelectNodes[].Count;
            Assert.That(count,Is.EqualTo(10));

            XmlNodeList nodes = productsXmlDocument.SelectNodes("/Row/[RowType == 3]");
            Assert.AreEqual(nodes.Count, 4);
		}
	}
}

