using System;
using Plugin.BLE.Abstractions.Contracts;

namespace MicrobitBLE.MicrobitUtils.Services
{
	public class MicrobitServiceProvider : IMicrobitServiceProvider
	{
		public MicrobitServiceProvider(
			String serviceName,
			string serviceDescription,
			Guid serviceId,
			IService serviceInstance,
			Func<String, String, Guid, IService, IMicrobitService> serviceProvider)
		{
			ServiceName = serviceName;
			ServiceDescription = serviceDescription;
			_serviceId = serviceId;
			_serviceInstance = serviceInstance;
			_serviceProvider = serviceProvider;
		}

		public string ServiceDescription { get; private set;}

		public string ServiceName { get; private set; }

		private Guid _serviceId;

		private IService _serviceInstance;

		private Func<String, String, Guid, IService, IMicrobitService> _serviceProvider;

		public IMicrobitService GetServiceInstance()
		{
			return _serviceProvider(ServiceName, ServiceDescription, _serviceId, _serviceInstance);
		}
	}
}
