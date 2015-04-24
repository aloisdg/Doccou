using Counter;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

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

		private void ExecuteDropped(object o)
		{
			var e = o as DragEventArgs;
			if (e != null)
				ReadFiles(e);
		}

		private void ReadFiles(DragEventArgs e)
		{
			if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
			//SwitchLoaderVisibility();
			try
			{
				var paths = (string[])e.Data.GetData(DataFormats.FileDrop);
				PageCount = Convert.ToUInt32(paths
					.Sum(path => new Document(path, new FileStream(path, FileMode.Open)).Count));

				RaisePropertyChanged("PageCount");
				//SwitchLoaderVisibility();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
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