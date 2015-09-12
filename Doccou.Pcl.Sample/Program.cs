using System;
using System.IO;

namespace Doccou.Pcl.Sample
{
	class Program
	{
		static void Main()
		{
			const string path = "../../Example/example.pdf";

			var document = new Document(path, File.Open(path, FileMode.Open));

			Console.WriteLine("{0} contains {1} pages.",
				document.Name,
				document.Count);
		}
	}
}
