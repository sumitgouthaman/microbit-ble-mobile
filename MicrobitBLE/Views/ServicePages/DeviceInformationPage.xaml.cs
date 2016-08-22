using MicrobitBLE.MicrobitUtils.Services;
using Xamarin.Forms;

namespace MicrobitBLE.Views.ServicePages
{
	public partial class DeviceInformationPage : ContentPage
	{
		private DeviceInformationService _service;

		public DeviceInformationPage(DeviceInformationService service)
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

