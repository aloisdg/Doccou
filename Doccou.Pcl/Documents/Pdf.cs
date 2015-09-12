using System;
using System.IO;
using iTextSharp.text.pdf;

namespace Doccou.Pcl.Documents
{
	internal sealed class Pdf : ADocument
	{
		public override DocumentType	Type { get { return DocumentType.Pdf; } }
		public override uint		Count { get; protected set; }

		// faster than regex.
		// so thread http://stackoverflow.com/q/320281/1248177
		public Pdf(Stream stream)
		{
			try
			{
				Count = Convert.ToUInt32(new PdfReader(stream).NumberOfPages);
			}
			catch (Exception ex)
			{
				throw  new Exception("We can't create this PDF.", ex);
			}
		}
	}
}