using System.Collections.Generic;
using System.IO;
using Doccou.Pcl.Documents;
using Doccou.Pcl.Documents.Archives;

namespace Doccou.Pcl
{
	/// <summary>
	/// Document is the class wrapping every other class in this library.
	/// If you are an user, everything you need come from here.
	/// </summary>
	public class Document : IDocument
	{
		public DocumentType Type { get; private set; }
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
			{ ".pptx", DocumentType.Pptx },
			{ ".odt", DocumentType.Odt },
			{ ".bmp", DocumentType.Bmp },
			{ ".jpg", DocumentType.Jpeg },
			{ ".jpeg", DocumentType.Jpeg },
			{ ".gif", DocumentType.Gif },
			{ ".tiff", DocumentType.Tiff },
			{ ".png", DocumentType.Png },
		};

		/// <remarks>
		/// We can't open stream ourself.
		/// </remarks>
		private Document(string fullName)
		{
			FullName = fullName;
			Name = Path.GetFileName(fullName);
			NameWithoutExtension = Path.GetFileNameWithoutExtension(fullName);
			Extension = Path.GetExtension(fullName).ToLowerInvariant();
		}

		public Document(string fullName, Stream stream)
			: this(fullName)
		{
			var document = BuildDocument(stream);
			Count = document.Count;
			Type = document.Type;
		}

		public bool IsSupported(string extension)
		{
			return _extensionsSupported.ContainsKey(extension);
		}

		// replace with a static dictionary ?
		// could be a constructor
		private IDocument BuildDocument(Stream stream)
		{
			switch (Extension)
			{
				case ".pdf": return new Pdf(stream);
				case ".docx": return new Docx(stream);
				case ".pptx": return new Pptx(stream);
				case ".odt": return new Odt(stream);
				case ".bmp": return new Bmp(stream);
				case ".jpg":
				case ".jpeg": return new Jpeg(stream);
				case ".gif": return new Gif(stream);
				case ".tiff": return new Tiff(stream);
				case ".png": return new Png(stream);
				default: return new Unknow();
			}
		}
	}
}