using System;
namespace MicrobitBLE.MicrobitUtils.Services
{
	public class DeviceInformationService : AMicrobitService
	{
		public DeviceInformationService()
			: base("Device Information",
			       ServiceIds.DeviceInformationServiceId,
			      "Model number, Serial number, etc.")
		{
		}
	}
}

