using System;
using NUnit.Framework;

namespace Doccou.Pcl.Test
{
	[TestFixture]
	public class UnitTestOdt
	{
		//[Test]
		//public void TestEmptyPdf()
		//{

		//}

		[Test]
		public void TestOnePageOdt()
		{
			const string path = "../../Example/Odt/OnePage.odt";

			Assert.AreEqual(1, Helper.ReadCount(path));
		}


		[Test]
		public void TestOnePageErrorOdt()
		{
			const string path = "../../Example/Odt/OnePageError.odt";
			Assert.Throws<Exception>(() => Helper.ReadCount(path));
		}

		//[Test]
		//public void TestBigPdf()
		//{
		//}
	}
}
