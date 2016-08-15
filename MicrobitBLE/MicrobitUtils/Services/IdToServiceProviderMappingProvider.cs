using System;
using System.Collections.Generic;
using Plugin.BLE.Abstractions.Contracts;

namespace MicrobitBLE.MicrobitUtils.Services
{
	public static class IdToServiceProviderMappingProvider
	{
		private static Dictionary<Guid, Func<IService, IMicrobitService>> _mapping = new Dictionary<Guid, Func<IService, IMicrobitService>>()
		{
			{ServiceIds.DeviceInformationServiceId, (service) => new DeviceInformationService(service)},
			{ServiceIds.TemperatureServiceId, (service) => new TemperatureService(service)}
		};

		public static Func<IService, IMicrobitService> ServiceProvider(Guid serviceGuid)
		{
			Func<IService, IMicrobitService> provider;
			if (!_mapping.TryGetValue(serviceGuid, out provider))
			{
				return null;
			}
			return provider;
		}
	}
}

