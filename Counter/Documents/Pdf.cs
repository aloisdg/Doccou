using System;
using System.IO;
using iTextSharp.text.pdf;

namespace Counter.Documents
{
	internal sealed class Pdf : ADocument
	{
		public override DocumentType	Type { get { return DocumentType.Pdf; } }
		public override uint		Count { get; protected set; }

		public Pdf(Stream stream)
		{
			Count = Convert.ToUInt32(new PdfReader(stream).NumberOfPages);
		}
	}
}
