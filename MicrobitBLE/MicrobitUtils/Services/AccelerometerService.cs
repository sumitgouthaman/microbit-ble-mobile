using System;
using System.Collections.Generic;
using System.IO;
using MicrobitBLE.MicrobitUtils.Helpers;
using MicrobitBLE.Views.ServicePages;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;

namespace MicrobitBLE.MicrobitUtils.Services
{
	public class AccelerometerService : AMicrobitService
	{
		private static Guid AccelerometerCharacteristicId = new Guid("E95DCA4B251D470AA062FA1922DFA9A8");
		private static Guid AccelerometerPeriodCharacteristicId = new Guid("E95DFB24251D470AA062FA1922DFA9A8");

		private double _x = double.NaN;
		public Double X
		{
			get
			{
				return _x;
			}
			set
			{
				_x = value;
				OnPropertyChanged();
			}
		}

		private double _y = double.NaN;
		public Double Y
		{
			get
			{
				return _y;
			}
			set
			{
				_y = value;
				OnPropertyChanged();
			}
		}

		private double _z = double.NaN;
		public Double Z
		{
			get
			{
				return _z;
			}
			set
			{
				_z = value;
				OnPropertyChanged();
			}
		}

		private int _accelerometerPeriod = int.MinValue;
		public int AccelerometerPeriod
		{
			get
			{
				return _accelerometerPeriod;
			}
			set
			{
				_accelerometerPeriod = value;
				OnPropertyChanged();
			}
		}

		public override ContentPage Page
		{
			get
			{
				return new AccelerometerPage(this);
			}
		}

		private AccelerometerService(String name, String description, Guid id, IService service)
			: base(name,
				   description,
				   id,
				   service)
		{ }

		public static IMicrobitService GetInstance(String name, String description, Guid id, IService service)
		{
			return new AccelerometerService(name, description, id, service);
		}

		public async void LoadCharacteristics()
		{
			IEnumerable<ICharacteristic> characteristics = await ServiceInstance.GetCharacteristicsAsync();
			foreach (ICharacteristic characteristic in characteristics)
			{
				if (characteristic.Id == AccelerometerCharacteristicId)
				{
					characteristic.ValueUpdated += (sender, e) =>
					{
						byte[] rawBytes = e.Characteristic.Value;
						if (rawBytes.Length != 6)
							throw new InvalidDataException("Accelerometer characteristic should have 6 bytes");

						short rawX = ConversionHelpers.ByteArrayToShort16BitLittleEndian(new byte[] { rawBytes[0], rawBytes[1] });
						short rawY = ConversionHelpers.ByteArrayToShort16BitLittleEndian(new byte[] { rawBytes[2], rawBytes[3] });
						short rawZ = ConversionHelpers.ByteArrayToShort16BitLittleEndian(new byte[] { rawBytes[4], rawBytes[5] });

						X = ((double)rawX) / 1000.0;
						Y = ((double)rawY) / 1000.0;
						Z = ((double)rawZ) / 1000.0;
					};
					MarkCharacteristicForUpdate(characteristic);
				}
				else if (characteristic.Id == AccelerometerPeriodCharacteristicId)
				{
					byte[] val = await characteristic.ReadAsync();
					int period = ConversionHelpers.ByteArrayToShort16BitLittleEndian(val);
					AccelerometerPeriod = period;
				}
			}

			StartUpdates();
		}
	}
}



