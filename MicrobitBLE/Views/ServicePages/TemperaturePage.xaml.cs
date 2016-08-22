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

		protected override void OnDisappearing()
		{
			_service.StopUpdates();
		}
	}
}

