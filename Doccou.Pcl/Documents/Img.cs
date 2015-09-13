using System.IO;
using System.Linq;

namespace Doccou.Pcl.Documents
{
	internal abstract class AImg : IDocument
	{
		public abstract DocumentType Type { get; }
		public uint Count { get; private set; }

		/// <remarks>
		/// Count is hardcoded as one page.
		/// </remarks>
		/// <param name="stream">A stream representation of the file.</param>
		protected AImg(Stream stream)
		{
			//Type = GetDocumentType(StreamToBytes(stream));
			Count = 1;
		}

		#region get Type

		/// <summary>
		/// Get image type from file in bytes form.
		/// </summary>
		/// <see cref="http://stackoverflow.com/a/9446045/1248177"/>
		/// <seealso cref="http://www.mikekunz.com/image_file_header.html"/>
		/// <param name="bytes">A bytes based form of the string.</param>
		/// <returns>A DocumentType of the file.</returns>
		private static DocumentType GetDocumentType(byte[] bytes)
		{
			var bmp = StringToAscii("BM");     // BMP
			var gif = StringToAscii("GIF");    // GIF
			var png = new byte[] { 137, 80, 78, 71 };    // PNG
			var tiff = new byte[] { 73, 73, 42 };         // TIFF
			var tiff2 = new byte[] { 77, 77, 42 };         // TIFF
			var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
			var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon

			if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
				return DocumentType.Bmp;

			if (gif.SequenceEqual(bytes.Take(gif.Length)))
				return DocumentType.Gif;

			if (png.SequenceEqual(bytes.Take(png.Length)))
				return DocumentType.Png;

			if (tiff.SequenceEqual(bytes.Take(tiff.Length))
			    || tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
				return DocumentType.Tiff;

			if (jpeg.SequenceEqual(bytes.Take(jpeg.Length))
			    || jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
				return DocumentType.Jpeg;

			return DocumentType.Unknow;
		}

		/// <summary>
		/// Gets an encoding for the ASCII (7-bit) character set.
		/// </summary>
		/// <see cref="http://stackoverflow.com/a/4022893/1248177"/>
		/// <param name="s">A character set.</param>
		/// <returns>An encoding for the ASCII (7-bit) character set.</returns>
		private static byte[] StringToAscii(string s)
		{
			return (from char c in s select (byte)((c <= 0x7f) ? c : '?')).ToArray();
		}

		/// <summary>
		/// Convert a stream into bytes.
		/// </summary>
		/// <see cref="http://stackoverflow.com/a/7073124/1248177"/>
		/// <param name="stream">A stream representation of the file.</param>
		/// <returns>A bytes based form of the string.</returns>
		private static byte[] StreamToBytes(Stream stream)
		{
			using (var memoryStream = new MemoryStream())
			{
				stream.CopyTo(memoryStream);
				return memoryStream.ToArray();
			}
		}

		#endregion

	}

	internal class Bmp : AImg
	{
		public override DocumentType Type { get { return DocumentType.Bmp; } }

		public Bmp(Stream stream) : base(stream) { }
	}

	internal class Jpeg : AImg
	{
		public override DocumentType Type { get { return DocumentType.Jpeg; } }

		public Jpeg(Stream stream) : base(stream) { }
	}

	internal class Gif : AImg
	{
		public override DocumentType Type { get { return DocumentType.Gif; } }

		public Gif(Stream stream) : base(stream) { }
	}

	internal class Tiff : AImg
	{
		public override DocumentType Type { get { return DocumentType.Tiff; } }

		public Tiff(Stream stream) : base(stream) { }
	}

	internal class Png : AImg
	{
		public override DocumentType Type { get { return DocumentType.Png; } }

		public Png(Stream stream) : base(stream) { }
	}
}