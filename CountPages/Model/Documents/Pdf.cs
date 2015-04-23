using System;
using iTextSharp.text.pdf;

namespace CountPages.Model.Documents
{
	public class Pdf : ADocument
	{
		public override DocumentType Type { get { return DocumentType.Pdf; } }

		public Pdf(string fullName) : base(fullName) { }

		public override uint Count()
		{
			try
			{
				var pdfReader = new PdfReader(this.FullName);
				return Convert.ToUInt32(pdfReader.NumberOfPages);
			}
			catch (Exception ex)
			{
				// we could skip and return 0.
				throw new Exception(String.Format("There is a problem with {0}", this.FullName), ex);
			}
		}
	}
}
