using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using iTextSharp.text.pdf;

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

			var paths = (string[])e.Data.GetData(DataFormats.FileDrop);
			var count = paths.Select(path => new FileInfo(path)).Where(file => file.Extension.Equals(".pdf")).Sum(file => CountPdfPages(file));

			MessageBox.Show(count.ToString());
			
		}

		public static int CountPdfPages(FileSystemInfo file)
		{
			var pdfReader = new PdfReader(file.FullName);
			return pdfReader.NumberOfPages;
		}
	}
}
