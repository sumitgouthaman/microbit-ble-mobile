using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MicrobitBLE.Views
{
	public partial class InstructionsPage : ContentPage
	{
		public InstructionsPage()
		{
			InitializeComponent();

			OpenHexLinkButton.Clicked += (sender, e) =>
			{
				Device.OpenUri(new Uri("http://blog.bluetooth.com/bbc-microbit/"));
			};

			OpenMicrobitLinkButton.Clicked += (sender, e) =>
			{
				Device.OpenUri(new Uri("https://www.microbit.co.uk/device"));
			};

			OpenGithubLinkButton.Clicked += (sender, e) =>
			{
				Device.OpenUri(new Uri("https://github.com/sumitgouthaman/microbit-ble-mobile"));
			};
		}
	}
}
