using System;

namespace MicrobitBLE.MicrobitUtils.Services
{
	public static class ServiceIds
	{
		public static Guid DeviceInformationServiceId = new Guid("0000180A00001000800000805F9B34FB");
		public static Guid TemperatureServiceId = new Guid("E95D6100251D470AA062FA1922DFA9A8");
		public static Guid AccelerometerServiceId = new Guid("E95D0753251D470AA062FA1922DFA9A8");
		public static Guid ButtonServiceId = new Guid("E95D9882251D470AA062FA1922DFA9A8");
		public static Guid MagnetometerServiceId = new Guid("E95DF2D8251D470AA062FA1922DFA9A8");
		public static Guid LedServiceId = new Guid("E95DD91D251D470AA062FA1922DFA9A8");
	}
}

