using MicrobitBLE.ViewModels;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;

namespace MicrobitBLE.Views
{
	public partial class DeviceListPage : ContentPage
	{
		private DevicesViewModel vm;
		private bool firstTime = true;

		public DeviceListPage()
		{
			InitializeComponent();
			vm = new DevicesViewModel();
			BindingContext = vm;

			DevicesList.ItemSelected += async (object sender, SelectedItemChangedEventArgs e) =>
			{
				if (e.SelectedItem == null)
					return;
				
				IDevice selectedDevice = e.SelectedItem as IDevice;
				await Navigation.PushAsync(new DeviceServicesPage(selectedDevice));
				((ListView)sender).SelectedItem = null;
			};

			InstructionsButton.Clicked += (sender, e) =>
			{
				Navigation.PushAsync(new InstructionsPage());
			};
		}

		protected override void OnAppearing()
		{
			if (firstTime)
			{
				firstTime = false;
				vm.StartScanning();
			}
			else
			{
				vm.UpdateList();
			}
		}

		protected override void OnDisappearing()
		{
			vm.StopScanning();
		}
	}
}

