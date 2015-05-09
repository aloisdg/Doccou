using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Counter;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace CountPages.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		public uint PageCount { get; set; }
		public Visibility LoaderVisibility { get; set; }

		public RelayCommand<object> Dropped { get; private set; }

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

			SwitchLoaderVisibility();
		}

		private async void ExecuteDropped(object o)
		{
			var e = o as DragEventArgs;
			if (e != null)
				await ReadFiles(e);
		}

		private async Task ReadFiles(DragEventArgs e)
		{
			if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
			//try
			//{
			var paths = (string[])e.Data.GetData(DataFormats.FileDrop);

			var tasks = new Task<Document>[paths.Length];
			for (var i = 0; i < paths.Length; i++)
			{
				var path = paths[i];
				var stream = new FileStream(path, FileMode.Open);
				tasks[i] = Task.Run<Document>(() =>
					new Document(path, stream));
			}
			var documents =  await Task.WhenAll(tasks);

			PageCount = Convert.ToUInt32(documents.Sum(doc => doc.Count));

			RaisePropertyChanged("PageCount");
			//SwitchLoaderVisibility();
			//}
			//catch (Exception ex)
			//{
			//	MessageBox.Show(ex.Message);
			//}
		}

		private void SwitchLoaderVisibility()
		{
			LoaderVisibility = LoaderVisibility.Equals(Visibility.Visible)
				? Visibility.Hidden : Visibility.Visible;
			RaisePropertyChanged("LoaderVisibility");
		}
	}
}