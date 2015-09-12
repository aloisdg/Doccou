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
		public void TestOnePagePdf()
		{
			const string path = "../../Example/Docx/OnePage.docx";
			Assert.AreEqual(1, Helper.ReadCount(path));
		}

		//[Test]
		//public void TestBigPdf()
		//{
		//}
	}
}
