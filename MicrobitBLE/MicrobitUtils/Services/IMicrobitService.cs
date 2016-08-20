using System;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;

namespace MicrobitBLE.MicrobitUtils.Services
{
	public interface IMicrobitService
	{
		String FriendlyName { get; }
		Guid Id { get; }
		String Description { get; }
		IService ServiceInstance { get; }
		ContentPage Page { get; }
	}
}

