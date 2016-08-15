using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using MicrobitBLE.Views.ServicePages;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;

namespace MicrobitBLE.MicrobitUtils.Services
{
	public class TemperatureService: AMicrobitService
	{
		private IService _service;
		private static Guid TemperatureCharacteristicId = new Guid("E95D9250251D470AA062FA1922DFA9A8");

		private int _temperatureCelsius = Int32.MinValue;
		public String Temperature
		{
			get
			{
				if (_temperatureCelsius == Int32.MinValue)
					return "[Not Available]";
				
				return _temperatureCelsius + "° C / " + ((double)_temperatureCelsius * (180.0 / 100.0) + 32) + "° F";
			}
		}

		public override ContentPage Page
		{
			get
			{
				return new TemperaturePage(this);
			}
		}

		public TemperatureService(IService service)
			: base("Temperature",
				  ServiceIds.TemperatureServiceId,
				   "Room temperature")
		{
			_service = service;
		}

		public async void LoadCharacteristics()
		{
			IEnumerable<ICharacteristic> characteristics = await _service.GetCharacteristicsAsync();
			foreach (ICharacteristic characteristic in characteristics)
			{
				if (characteristic.Id == TemperatureCharacteristicId)
				{
					byte[] tempBytes = await characteristic.ReadAsync();
					_temperatureCelsius = (int)(tempBytes.First());
					OnPropertyChanged(nameof(Temperature));
				}
			}
		}

		public override event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged([CallerMemberName] string name = null)
		{
			var changed = PropertyChanged;

			if (changed == null)
				return;

			changed.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}

