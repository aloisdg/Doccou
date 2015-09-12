using System;
using NUnit.Framework;

namespace Doccou.Pcl.Test
{
	[TestFixture]
	public class UnitTestDocx
	{
		//[Test]
		//public void TestEmptyPdf()
		//{

		//}

		[Test]
		public void TestOnePageDocx()
		{
			const string path = "../../Example/Docx/OnePage.docx";
			Assert.AreEqual(1, Helper.ReadCount(path));
		}


		[Test]
		public void TestOnePageErrorDocx()
		{
			const string path = "../../Example/Docx/OnePageError.docx";
			Assert.Throws<Exception>(() => Helper.ReadCount(path));
		}

		//[Test]
		//public void TestBigPdf()
		//{
		//}
	}
}
