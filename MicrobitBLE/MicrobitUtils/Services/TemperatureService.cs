using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using MicrobitBLE.MicrobitUtils.Helpers;
using MicrobitBLE.Views.ServicePages;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;

namespace MicrobitBLE.MicrobitUtils.Services
{
	public class TemperatureService: AMicrobitService
	{
		private static Guid TemperatureCharacteristicId = new Guid("E95D9250251D470AA062FA1922DFA9A8");
		private static Guid TemperaturePeriodCharacteristicId = new Guid("E95D1B25251D470AA062FA1922DFA9A8");

		private int _temperature = int.MinValue;
		public int Temperature
		{
			get
			{
				return _temperature;
			}
			set
			{
				_temperature = value;
				OnPropertyChanged();
			}
		}

		private int _temperaturePeriod = int.MinValue;
		public int TemperaturePeriod
		{
			get
			{
				return _temperaturePeriod;
			}
			set
			{
				_temperaturePeriod = value;
				OnPropertyChanged();
			}
		}

		public override ContentPage Page
		{
			get
			{
				return new TemperaturePage(this);
			}
		}

		private TemperatureService(String name, String description, Guid id, IService service)
			: base(name,
				   description,
				   id,
				   service)
		{ }

		public static IMicrobitService GetInstance(String name, String description, Guid id, IService service)
		{
			return new TemperatureService(name, description, id, service);
		}

		public async void LoadCharacteristics()
		{
			IEnumerable<ICharacteristic> characteristics = await ServiceInstance.GetCharacteristicsAsync();
			foreach (ICharacteristic characteristic in characteristics)
			{
				if (characteristic.Id == TemperatureCharacteristicId)
				{
					characteristic.ValueUpdated += (sender, e) =>
					{
						byte[] tempBytes = e.Characteristic.Value;
						Temperature = (sbyte)(tempBytes.First());
					};
					MarkCharacteristicForUpdate(characteristic);
				}
				else if (characteristic.Id == TemperaturePeriodCharacteristicId)
				{
					byte[] val = await characteristic.ReadAsync();
					int period = ConversionHelpers.ByteArrayToShort16BitLittleEndian(val);
					TemperaturePeriod = period;
				}
			}

			StartUpdates();
		}
	}
}

