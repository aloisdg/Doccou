using Counter.Documents;
using System;
using System.IO;

namespace Counter
{
	/// <summary>
	/// Document is the class wrapping every other class in this library.
	/// If you are an user, everything you need come from here.
	/// </summary>
	public class Document
	{
		public DocumentType ExtensionType { get; private set; }
		public string Extension { get; private set; }
		public string FullName { get; private set; }
		public string Name { get; private set; }
		public string NameWithoutExtension { get; private set; }
		public uint Count { get; private set; }

		public Document(string fullName, Stream stream)
		{
			FullName = fullName;
			Name = Path.GetFileName(Name);
			NameWithoutExtension = Path.GetFileNameWithoutExtension(Name);
			Extension = Path.GetExtension(fullName);
			var doc = BuildDocument(Extension, stream);
			ExtensionType = doc.Type;
			Count = doc.Count;
		}

		// replace with a static dictionary ?
		// http://askubuntu.com/questions/305633/how-can-i-determine-the-page-count-of-odt-doc-docx-and-other-office-documents
		private static IDocument BuildDocument(string extension, Stream stream)
		{
			if (extension.Equals(".doc") || extension.Equals(".docx"))
				return new Doc(stream);
			if (extension.Equals(".pdf"))
				return new Pdf(stream);

			// hard. We should build a NotSupported document. A garbage/waiting place.
			throw new NotImplementedException("This extension is not emplemented");
		}
	}
}