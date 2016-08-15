using System;
using System.Collections.Generic;

namespace MicrobitBLE.MicrobitUtils.Services
{
	public static class IdToServiceProviderMappingProvider
	{
		private static Dictionary<Guid, Func<IMicrobitService>> _mapping = new Dictionary<Guid, Func<IMicrobitService>>()
		{
			{ServiceIds.DeviceInformationServiceId, () => new DeviceInformationService()}
		};

		public static Func<IMicrobitService> ServiceProvider(Guid serviceGuid)
		{
			Func<IMicrobitService> provider;
			if (!_mapping.TryGetValue(serviceGuid, out provider))
			{
				return null;
			}
			return provider;
		}
	}
}

