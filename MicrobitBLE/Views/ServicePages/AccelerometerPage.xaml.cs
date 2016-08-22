using MicrobitBLE.MicrobitUtils.Services;
using Xamarin.Forms;

namespace MicrobitBLE.Views.ServicePages
{
	public partial class AccelerometerPage : ContentPage
	{
		private AccelerometerService _service;

		public AccelerometerPage(AccelerometerService service)
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

