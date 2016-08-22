using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
		private ICharacteristic _ledMatrixCharacteristic = null;

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

		private bool _ledMatrixCharacteristicAvailable;
		public bool LedMatrixCharacteristicAvailable
		{
			get
			{
				return _ledMatrixCharacteristicAvailable;
			}
			set
			{
				_ledMatrixCharacteristicAvailable = value;
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
				else if (characteristic.Id == LedMatrixStateCharacteristicId)
				{
					_ledMatrixCharacteristic = characteristic;
					LedMatrixCharacteristicAvailable = true;
				}
			}
		}

		public async Task SendText()
		{
			if (String.IsNullOrEmpty(TextToSend))
				return;
			
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

		public async Task FlipLed(Tuple<int, int> coordinate)
		{
			if (IsBusy)
				return;
			IsBusy = true;
			try
			{
				byte[] matrix = await _ledMatrixCharacteristic.ReadAsync();
				matrix[coordinate.Item1] ^= (byte)(16 >> coordinate.Item2); // 16 = 00010000 in binary
				await _ledMatrixCharacteristic.WriteAsync(matrix);
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
