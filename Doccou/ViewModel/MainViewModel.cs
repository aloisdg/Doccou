using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Doccou.Pcl;
using Doccou.ViewModel.Extensions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Doccou.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		public uint PageCount { get; set; }
		private ObservableCollection<Document> Documents { get; set; }

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
			if (e != null && e.Data.GetDataPresent(DataFormats.FileDrop, false))
				await ReadFiles((string[])e.Data.GetData(DataFormats.FileDrop)).ConfigureAwait(false);
		}

		private async Task ReadFiles(IEnumerable<string> droppedPaths)
		{
			try
			{

				var paths = droppedPaths.SelectMany(path => path.GetAllFiles()).ToArray();

				var tasks = new Task<Document>[paths.Length];
				for (var i = 0; i < paths.Length; i++)
				{
					var path = paths[i];
					var stream = new FileStream(path, FileMode.Open);
					tasks[i] = Task.Run(() => new Document(path, stream));
				}
				var documents = await Task.WhenAll(tasks).ConfigureAwait(false);

				Documents  = new ObservableCollection<Document>(documents);

				PageCount = Convert.ToUInt32(documents.Sum(doc => doc.Count));

				RaisePropertyChanged("PageCount");
				RaisePropertyChanged("Documents");
				//SwitchLoaderVisibility();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void SwitchLoaderVisibility()
		{
			LoaderVisibility = LoaderVisibility.Equals(Visibility.Visible)
				? Visibility.Hidden : Visibility.Visible;
			RaisePropertyChanged("LoaderVisibility");
		}
	}
}