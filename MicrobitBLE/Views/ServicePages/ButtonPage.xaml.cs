using MicrobitBLE.MicrobitUtils.Services;
using Xamarin.Forms;

namespace MicrobitBLE.Views.ServicePages
{
	public partial class ButtonPage : ContentPage
	{
		private ButtonService _service;

		public ButtonPage(ButtonService service)
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

