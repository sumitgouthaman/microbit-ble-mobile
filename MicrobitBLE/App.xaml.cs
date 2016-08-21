using System;
using Xamarin.Forms;
using MicrobitBLE.Views;
using MicrobitBLE.Views.ServicePages;

namespace MicrobitBLE
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new DeviceListPage());
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

