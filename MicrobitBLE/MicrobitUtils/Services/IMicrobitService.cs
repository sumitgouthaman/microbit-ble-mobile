using System;
namespace MicrobitBLE.MicrobitUtils.Services
{
	public interface IMicrobitService
	{
		String FriendlyName { get; }
		Guid Id { get; }
		String Description { get; }
	}
}

