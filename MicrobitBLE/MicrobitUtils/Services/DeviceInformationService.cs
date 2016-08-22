using System;
using System.Collections.Generic;
using MicrobitBLE.Views.ServicePages;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;

namespace MicrobitBLE.MicrobitUtils.Services
{
	public class DeviceInformationService : AMicrobitService
	{
		private enum CharacteristicField
		{
			ModelNumber,
			SerialNumber,
			HardwareRevision,
			FirmwareRevision,
			ManufacturerName
		}
		private static IDictionary<Guid, CharacteristicField> _characteristicMapping = new Dictionary<Guid, CharacteristicField>()
		{
			{new Guid("00002A2400001000800000805F9B34FB"), CharacteristicField.ModelNumber},
			{new Guid("00002A2500001000800000805F9B34FB"), CharacteristicField.SerialNumber},
			{new Guid("00002A2700001000800000805F9B34FB"), CharacteristicField.HardwareRevision},
			{new Guid("00002A2600001000800000805F9B34FB"), CharacteristicField.FirmwareRevision},
			{new Guid("00002A2900001000800000805F9B34FB"), CharacteristicField.ManufacturerName}
		};

		public String ModelNumber { get; private set; } = "[Not Available]";
		public String SerialNumber { get; private set; } = "[Not Available]";
		public String HardwareRevision { get; private set; } = "[Not Available]";
		public String FirmwareRevision { get; private set; } = "[Not Available]";
		public String ManufacturerName { get; private set; } = "[Not Available]";

		private DeviceInformationService(String name, String description, Guid id, IService service)
			: base(name,
				   description,
				   id,
				   service)
		{ }

		public static IMicrobitService GetInstance(String name, String description, Guid id, IService service)
		{
			return new DeviceInformationService(name, description, id, service);
		}

		public override ContentPage Page
		{
			get
			{
				return new DeviceInformationPage(this);
			}
		}

		public async void LoadCharacteristics()
		{
			IEnumerable<ICharacteristic> characteristics = await ServiceInstance.GetCharacteristicsAsync();
			foreach (ICharacteristic characteristic in characteristics)
			{
				CharacteristicField field;
				if (_characteristicMapping.TryGetValue(characteristic.Id, out field))
				{
					await characteristic.ReadAsync();
					switch (field)
					{
						case CharacteristicField.ModelNumber:
							ModelNumber = characteristic.StringValue;
							OnPropertyChanged(nameof(ModelNumber));
							break;
						case CharacteristicField.SerialNumber:
							SerialNumber = characteristic.StringValue;
							OnPropertyChanged(nameof(SerialNumber));
							break;
						case CharacteristicField.HardwareRevision:
							HardwareRevision = characteristic.StringValue;
							OnPropertyChanged(nameof(HardwareRevision));
							break;
						case CharacteristicField.FirmwareRevision:
							FirmwareRevision = characteristic.StringValue;
							OnPropertyChanged(nameof(FirmwareRevision));
							break;
						case CharacteristicField.ManufacturerName:
							ManufacturerName = characteristic.StringValue;
							OnPropertyChanged(nameof(ManufacturerName));
							break;
					}
				}
			}
		}
	}
}

