using iTextSharp.text.pdf;
using Microsoft.Office.Interop.Word;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Application = Microsoft.Office.Interop.Word.Application;
using Window = System.Windows.Window;

namespace CountPages
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		public void DropList_Drop(object sender, DragEventArgs e)
		{
			if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
			try
			{
				var paths = (string[])e.Data.GetData(DataFormats.FileDrop);
				var count = paths.Select(path => new FileInfo(path))
					//.Where(file => file.Extension.Equals(".pdf"))
					.Sum(file => CountPages(file));

				CountBlock.Text = count.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public static int CountPages(FileSystemInfo file)
		{
			if (file.Extension.Equals(".doc") || file.Extension.Equals(".docx"))
				return CountWordPages(file);
			if (file.Extension.Equals(".pdf"))
				return CountPdfPages(file);
			return 0;
		}

		public static int CountPdfPages(FileSystemInfo file)
		{
			try
			{
				var pdfReader = new PdfReader(file.FullName);
				return pdfReader.NumberOfPages;
			}
			catch (Exception ex)
			{
				// we could skip and return 0.
				throw new Exception(String.Format("There is a problem with {0}", file.FullName), ex);
			}
		}

		public static int CountWordPages(FileSystemInfo file)
		{
			const WdStatistic stat = WdStatistic.wdStatisticPages;
			_Application wordApp = new Application();

			object fileName = file.FullName;
			object readOnly = false;
			object isVisible = true;

			//  the way to handle parameters you don't care about in .NET
			object missing = Missing.Value;

			//   Make word visible, so you can see what's happening
			//wordApp.Visible = true;
			//   Open the document that was chosen by the dialog
			var aDoc = wordApp.Documents.Open(ref fileName,
						ref missing, ref readOnly, ref missing,
						ref missing, ref missing, ref missing,
						ref missing, ref missing, ref missing,
						ref missing, ref isVisible);

			return aDoc.ComputeStatistics(stat, ref missing);
		}
	}
}