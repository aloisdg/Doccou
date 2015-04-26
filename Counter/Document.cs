using Counter.Documents;
using System;
using System.Collections.Generic;
using System.IO;
using Counter.Documents.Archives;
using iTextSharp.xmp.impl;

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

		private readonly Dictionary<string, DocumentType> _extensionsSupported
			= new Dictionary<string, DocumentType>
		{
			{ ".pdf", DocumentType.Pdf },
			{ ".docx", DocumentType.Docx },
			{ ".odt", DocumentType.Odt }
		};

		public Document(string fullName, Stream stream)
		{
			FullName = fullName;
			Name = Path.GetFileName(Name);
			NameWithoutExtension = Path.GetFileNameWithoutExtension(Name);
			Extension = Path.GetExtension(fullName);
			ExtensionType = IsSupported(Extension)
				? _extensionsSupported[Extension]
				: DocumentType.Unknow;
			Count = !ExtensionType.Equals(DocumentType.Unknow)
				? BuildDocument(Extension, stream).Count
				: 0;
		}

		public bool IsSupported(string extension)
		{
			return _extensionsSupported.ContainsKey(extension);
		}

		// replace with a static dictionary ?
		// http://askubuntu.com/questions/305633/how-can-i-determine-the-page-count-of-odt-doc-docx-and-other-office-documents
		private static IDocument BuildDocument(string extension, Stream stream)
		{
			if (extension.Equals(".docx"))
				return new Docx(stream);
			if (extension.Equals(".pdf"))
				return new Pdf(stream);
			if (extension.Equals(".odt"))
				return new Odt(stream);

			// hard. We should build a NotSupported document. A garbage/waiting place.
			//  InvalidEnumArgumentException
			throw new NotImplementedException("This extension is not emplemented");
		}
	}
}