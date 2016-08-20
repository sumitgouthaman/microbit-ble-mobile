using System;
using System.Collections.Generic;
using MicrobitBLE.MicrobitUtils.Services;
using MicrobitBLE.ViewModels;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;

namespace MicrobitBLE.Views
{
	public partial class DeviceServicesPage : ContentPage
	{
		private DeviceServicesViewModel vm;

		public DeviceServicesPage(IDevice device)
		{
			InitializeComponent();

			vm = new DeviceServicesViewModel(device);
			BindingContext = vm;

			ServicesList.ItemSelected += ServicesList_ItemSelected;
		}

		async void ServicesList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
				return;

			IMicrobitServiceProvider serviceProvider = e.SelectedItem as IMicrobitServiceProvider;
			await Navigation.PushAsync(serviceProvider.GetServiceInstance().Page);
			((ListView)sender).SelectedItem = null;
		}
	}
}

