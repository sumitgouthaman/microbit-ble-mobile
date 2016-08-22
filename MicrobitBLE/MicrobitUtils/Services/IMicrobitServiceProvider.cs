using System;

namespace MicrobitBLE.MicrobitUtils.Services
{
	public interface IMicrobitServiceProvider
	{
		String ServiceName { get;}
		String ServiceDescription { get;}
		IMicrobitService GetServiceInstance();
	}
}
