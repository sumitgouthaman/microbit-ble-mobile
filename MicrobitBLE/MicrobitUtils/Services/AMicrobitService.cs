using System;
namespace MicrobitBLE.MicrobitUtils.Services
{
	public class AMicrobitService : IMicrobitService
	{
		public String FriendlyName { get; }

		public Guid Id { get; }

		public string Description { get; }

		public AMicrobitService(String friendlyName, Guid id, String descrption)
		{
			FriendlyName = friendlyName;
			Id = id;
			Description = descrption;
		}
	}
}

