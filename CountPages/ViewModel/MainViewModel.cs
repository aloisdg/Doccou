using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
			if (e != null && e.Data.GetDataPresent(DataFormats.FileDrop, false))
				await ReadFiles((string[])e.Data.GetData(DataFormats.FileDrop));
		}

		private async Task ReadFiles(IEnumerable<string> droppedPaths)
		{
			//try
			//{

			var paths = droppedPaths.SelectMany(path => path.GetAllFiles()).ToArray();

			var tasks = new Task<Document>[paths.Length];
			for (var i = 0; i < paths.Length; i++)
			{
				var path = paths[i];
				var stream = new FileStream(path, FileMode.Open);
				tasks[i] = Task.Run(() => new Document(path, stream));
			}
			var documents = await Task.WhenAll(tasks);

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


	public static class TreeReader
	{
		public static IEnumerable<string> GetAllFiles(this string path)
		{
			var queue = new Queue<string>();
			queue.Enqueue(path);
			while (queue.Count > 0)
			{
				path = queue.Dequeue();
				if (String.IsNullOrEmpty(Path.GetExtension(path)))
				{
					try
					{
						foreach (var subDir in Directory.GetDirectories(path))
							queue.Enqueue(subDir);
					}
					catch (Exception ex)
					{
						Console.Error.WriteLine(ex);
					}
					string[] files = null;
					try
					{
						files = Directory.GetFiles(path);
					}
					catch (Exception ex)
					{
						Console.Error.WriteLine(ex);
					}
					if (files != null)
						foreach (var t in files)
							yield return t;
				}
				else
					yield return path;
			}
		}
	}
}