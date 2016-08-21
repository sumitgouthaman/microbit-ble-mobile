using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MicrobitBLE.MicrobitUtils.Helpers;
using MicrobitBLE.Views.ServicePages;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;

namespace MicrobitBLE.MicrobitUtils.Services
{
	public class LedService : AMicrobitService
	{
		private static Guid LedMatrixStateCharacteristicId = new Guid("E95D7B77251D470AA062FA1922DFA9A8");
		private static Guid LedTextCharacteristicId = new Guid("E95D93EE251D470AA062FA1922DFA9A8");
		private static Guid ScrollingDelayCharacteristicId = new Guid("E95D0D2D251D470AA062FA1922DFA9A8");

		private ICharacteristic _ledTextCharacteristic = null;

		private bool _ledTextCharacteristicAvailable;
		public bool LedTextCharacteristicAvailable
		{
			get
			{
				return _ledTextCharacteristicAvailable;
			}
			set
			{
				_ledTextCharacteristicAvailable = value;
				OnPropertyChanged();
			}
		}

		public Command SendTextCommand { get; set;}
		public String TextToSend { get; set; }

		private LedService(String name, String description, Guid id, IService service)
			: base(name,
				   description,
				   id,
				   service)
		{
			SendTextCommand = new Command(
				async ()=> await SendText(),
				() => !IsBusy);
		}

		public static IMicrobitService GetInstance(String name, String description, Guid id, IService service)
		{
			return new LedService(name, description, id, service);
		}

		public override ContentPage Page
		{
			get
			{
				return new LEDPage(this);
			}
		}

		public async void LoadCharacteristics()
		{
			IEnumerable<ICharacteristic> characteristics = await ServiceInstance.GetCharacteristicsAsync();
			foreach (ICharacteristic characteristic in characteristics)
			{
				if (characteristic.Id == LedTextCharacteristicId)
				{
					_ledTextCharacteristic = characteristic;
					LedTextCharacteristicAvailable = true;
				}
			}
		}

		public async Task SendText()
		{
			IsBusy = true;
			try
			{
				byte[] rawBytes = Encoding.UTF8.GetBytes(TextToSend).Take(20).ToArray();
				await _ledTextCharacteristic.WriteAsync(rawBytes);
			}
			finally
			{
				IsBusy = false;
			}
		}

		private bool _busy;
		public bool IsBusy
		{
			get { return _busy; }
			set
			{
				_busy = value;
				OnPropertyChanged();
				SendTextCommand.ChangeCanExecute();
			}
		}
	}
}
