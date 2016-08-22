using System;
using System.Collections.Generic;
using System.Linq;
using MicrobitBLE.Views.ServicePages;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;

namespace MicrobitBLE.MicrobitUtils.Services
{
	public class ButtonService : AMicrobitService
	{
		private static Guid ButtonACharacteristicId = new Guid("E95DDA90251D470AA062FA1922DFA9A8");
		private static Guid ButtonBCharacteristicId = new Guid("E95DDA91251D470AA062FA1922DFA9A8");

		private ICharacteristic _aButtonCharacteristic = null;
		private ICharacteristic _bButtonCharacteristic = null;

		private int _aButton = -1;
		public int AButton
		{
			get
			{
				return _aButton;
			}
			set
			{
				_aButton = value;
				OnPropertyChanged();
			}
		}

		private int _bButton = -1;
		public int BButton
		{
			get
			{
				return _bButton;
			}
			set
			{
				_bButton = value;
				OnPropertyChanged();
			}
		}

		private ButtonService(String name, String description, Guid id, IService service)
			: base(name,
				   description,
				   id,
				   service)
		{ }

		public static IMicrobitService GetInstance(String name, String description, Guid id, IService service)
		{
			return new ButtonService(name, description, id, service);
		}

		public override ContentPage Page
		{
			get
			{
				return new ButtonPage(this);
			}
		}

		public async void LoadCharacteristics()
		{
			IEnumerable<ICharacteristic> characteristics = await ServiceInstance.GetCharacteristicsAsync();
			foreach (ICharacteristic characteristic in characteristics)
			{
				if (characteristic.Id == ButtonACharacteristicId)
				{
					characteristic.ValueUpdated += (sender, e) =>
					{
						byte[] tempBytes = e.Characteristic.Value;
						AButton = (sbyte)(tempBytes.First());
					};
					MarkCharacteristicForUpdate(characteristic);
				} 
				else if (characteristic.Id == ButtonBCharacteristicId)
				{
					characteristic.ValueUpdated += (sender, e) =>
					{
						byte[] tempBytes = e.Characteristic.Value;
						BButton = (sbyte)(tempBytes.First());
					};
					MarkCharacteristicForUpdate(characteristic);
				}
			}

			StartUpdates();
		}
	}
}

