using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using iTextSharp.text.pdf;
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;
using Window = System.Windows.Window;

namespace CountPages.ViewModel
{
	/// <summary>
	/// This class contains properties that the main View can data bind to.
	/// <para>
	/// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
	/// </para>
	/// <para>
	/// You can also use Blend to data bind with the tool's support.
	/// </para>
	/// <para>
	/// See http://www.galasoft.ch/mvvm
	/// </para>
	/// </summary>
	public class MainViewModel : ViewModelBase
	{
		public uint PageCount { get; set; }
		public RelayCommand<object> Dropped { get; private set; }
		/// <summary>
		/// Initializes a new instance of the MainViewModel class.
		/// </summary>
		public MainViewModel()
		{
			////if (IsInDesignMode)
			////{
			////    // Code runs in Blend --> create design time data.
			////}
			////else
			////{
			////    // Code runs "for real"
			Dropped = new RelayCommand<object>(ExecuteDropped);
			////}
		}

		private void ExecuteDropped(object o)
		{
			var e = o as DragEventArgs;
			if (e != null)
				ReadFiles(e);
		}

		private void ReadFiles(DragEventArgs e)
		{
			if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
			try
			{
				var paths = (string[])e.Data.GetData(DataFormats.FileDrop);
				var count = paths.Select(path => new FileInfo(path))
					//.Where(file => file.Extension.Equals(".pdf"))
					.Sum(file => CountPages(file));

				PageCount = (uint)count;
				RaisePropertyChanged("PageCount");
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