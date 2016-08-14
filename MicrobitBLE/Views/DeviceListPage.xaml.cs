using MicrobitBLE.ViewModels;
using Xamarin.Forms;

namespace MicrobitBLE.Views
{
	public partial class DeviceListPage : ContentPage
	{
		private DevicesViewModel vm;
		public DeviceListPage()
		{
			InitializeComponent();
			vm = new DevicesViewModel();
			BindingContext = vm;
		}

		protected override async void OnAppearing()
		{
			await vm.StartScanning();
		}

		protected override async void OnDisappearing()
		{
			await vm.StopScanning();
		}
	}
}

