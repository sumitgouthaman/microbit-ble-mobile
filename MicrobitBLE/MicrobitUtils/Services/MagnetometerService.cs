using System;
using System.Collections.Generic;
using System.IO;
using MicrobitBLE.MicrobitUtils.Helpers;
using MicrobitBLE.Views.ServicePages;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;

namespace MicrobitBLE.MicrobitUtils.Services
{
	public class MagnetometerService : AMicrobitService
	{
		private static Guid MagnetometerCharacteristicId = new Guid("E95DFB11251D470AA062FA1922DFA9A8");
		private static Guid MagnetometerPeriodCharacteristicId = new Guid("E95D386C251D470AA062FA1922DFA9A8");
		private static Guid MagnetometerBearingCharacteristicId = new Guid("E95D9715251D470AA062FA1922DFA9A8");

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

		private int _magnetometerPeriod = int.MinValue;
		public int MagnetometerPeriod
		{
			get
			{
				return _magnetometerPeriod;
			}
			set
			{
				_magnetometerPeriod = value;
				OnPropertyChanged();
			}
		}

		private int _magnetometerBearing = int.MinValue;
		public int MagnetometerBearing
		{
			get
			{
				return _magnetometerBearing;
			}
			set
			{
				_magnetometerBearing = value;
				OnPropertyChanged();
			}
		}

		public override ContentPage Page
		{
			get
			{
				return new MagnetometerPage(this);
			}
		}

		private MagnetometerService(String name, String description, Guid id, IService service)
			: base(name,
				   description,
				   id,
				   service)
		{ }

		public static IMicrobitService GetInstance(String name, String description, Guid id, IService service)
		{
			return new MagnetometerService(name, description, id, service);
		}

		public async void LoadCharacteristics()
		{
			IEnumerable<ICharacteristic> characteristics = await ServiceInstance.GetCharacteristicsAsync();
			foreach (ICharacteristic characteristic in characteristics)
			{
				if (characteristic.Id == MagnetometerCharacteristicId)
				{
					characteristic.ValueUpdated += (sender, e) =>
					{
						byte[] rawBytes = e.Characteristic.Value;
						if (rawBytes.Length != 6)
							throw new InvalidDataException("Magnetometer characteristic should have 6 bytes");

						short rawX = ConversionHelpers.ByteArrayToShort16BitLittleEndian(new byte[] { rawBytes[0], rawBytes[1] });
						short rawY = ConversionHelpers.ByteArrayToShort16BitLittleEndian(new byte[] { rawBytes[2], rawBytes[3] });
						short rawZ = ConversionHelpers.ByteArrayToShort16BitLittleEndian(new byte[] { rawBytes[4], rawBytes[5] });

						X = ((double)rawX) / 1000.0;
						Y = ((double)rawY) / 1000.0;
						Z = ((double)rawZ) / 1000.0;
					};
					MarkCharacteristicForUpdate(characteristic);
				}
				else if (characteristic.Id == MagnetometerBearingCharacteristicId)
				{
					characteristic.ValueUpdated += (sender, e) =>
					{
						byte[] rawBytes = e.Characteristic.Value;
						if (rawBytes.Length != 2)
							throw new InvalidDataException("Magnetometer Bearing characteristic should have 2 bytes");

						MagnetometerBearing = ConversionHelpers.ByteArrayToShort16BitLittleEndian(rawBytes);
					};
					MarkCharacteristicForUpdate(characteristic);
				}
				else if (characteristic.Id == MagnetometerPeriodCharacteristicId)
				{
					byte[] val = await characteristic.ReadAsync();
					int period = ConversionHelpers.ByteArrayToShort16BitLittleEndian(val);
					MagnetometerPeriod = period;
				}
			}

			StartUpdates();
		}
	}
}



