using System;
using System.Collections.Generic;
using MicrobitBLE.MicrobitUtils.Services;
using Xamarin.Forms;

namespace MicrobitBLE.Views.ServicePages
{
	public partial class TemperaturePage : ContentPage
	{
		private TemperatureService _service;

		public TemperaturePage(TemperatureService service)
		{
			InitializeComponent();
			_service = service;
			BindingContext = _service;
		}

		protected override void OnAppearing()
		{
			_service.LoadCharacteristics();
		}
	}
}

