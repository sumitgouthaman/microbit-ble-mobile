using System;
using Xamarin.Forms;

namespace MicrobitBLE.MicrobitUtils.Services
{
	public interface IMicrobitService
	{
		String FriendlyName { get; }
		Guid Id { get; }
		String Description { get; }
		ContentPage Page { get; }
	}
}

