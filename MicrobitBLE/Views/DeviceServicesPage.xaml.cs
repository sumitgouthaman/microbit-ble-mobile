using System;
using System.Collections.Generic;
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
		}
	}
}

