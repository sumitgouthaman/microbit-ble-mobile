using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using MicrobitBLE.MicrobitUtils.Services;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Xamarin.Forms;

namespace MicrobitBLE.ViewModels
{
	public class DeviceServicesViewModel : INotifyPropertyChanged
	{
		private IDevice _microbit;
		private IBluetoothLE _bluetoothLe;
		private IAdapter _adapter;

		public ObservableCollection<IMicrobitService> DetectedServices { get; private set; }

		public String DeviceName => _microbit.Name;
		public Guid DeviceId => _microbit.Id;
		public String DeviceConnectionState => _microbit.State.ToString();

		public Command ToggleConnectionCommand { get; set; }
		public String ToggleConnectionString
		{
			get
			{
				return _microbit.State == DeviceState.Connected ? "Disconnect" : "Connect";
			}
		}

		public Command ServicesCommand { get; set; }

		public DeviceServicesViewModel(IDevice device)
		{
			_microbit = device;
			_bluetoothLe = CrossBluetoothLE.Current;
			_adapter = _bluetoothLe.Adapter;

			_adapter.DeviceConnected += HandleConnectionStateChanged;
			_adapter.DeviceDisconnected += HandleConnectionStateChanged;
			_adapter.DeviceConnectionLost += HandleConnectionStateChanged;

			ToggleConnectionCommand = new Command(
				() => 
				{
					if (_microbit.State == DeviceState.Connected)
						Disconnect();
					else if (_microbit.State == DeviceState.Disconnected)
						Connect();
				},
				() => !IsBusy);

			ServicesCommand = new Command(async () =>
			{
				DetectedServices.Clear();
				IList<IService> services = await _microbit.GetServicesAsync();
				foreach (IService service in services)
				{
					Func<IMicrobitService> microbitServiceProvider = IdToServiceProviderMappingProvider.ServiceProvider(service.Id);
					if (microbitServiceProvider != null)
					{
						DetectedServices.Add(microbitServiceProvider());
					}
				}

			}, () => !IsBusy
			);

			DetectedServices = new ObservableCollection<IMicrobitService>();
		}

		public async void Connect()
		{
			IsBusy = true;
			try
			{
				if (!_adapter.ConnectedDevices.Contains(_microbit))
				{
					await _adapter.ConnectToDeviceAsync(_microbit, true);
				}
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Exception", e.Message, "Ok");
			}
			finally
			{
				IsBusy = false;
			}
			RefreshServices();
		}

		public async void Disconnect()
		{
			IsBusy = true;
			try
			{
				if (_adapter.ConnectedDevices.Contains(_microbit))
				{
					await _adapter.DisconnectDeviceAsync(_microbit);
				}
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Exception", e.Message, "Ok");
			}
			finally
			{
				IsBusy = false;
			}
			DetectedServices.Clear();
		}

		public async void RefreshServices()
		{
			IsBusy = true;
			DetectedServices.Clear();
			try
			{
				IList<IService> services = await _microbit.GetServicesAsync();
				foreach (IService service in services)
				{
					Func<IMicrobitService> microbitServiceProvider = IdToServiceProviderMappingProvider.ServiceProvider(service.Id);
					if (microbitServiceProvider != null)
					{
						DetectedServices.Add(microbitServiceProvider());
					}
				}
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Exception", e.Message, "Ok");
			}
			finally
			{
				IsBusy = false;
			}
		}

		private void HandleConnectionStateChanged(object sender, DeviceEventArgs e)
		{
			if (e.Device == _microbit)
			{
				OnPropertyChanged(nameof(DeviceConnectionState));
				OnPropertyChanged(nameof(ToggleConnectionString));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged([CallerMemberName] string name = null)
		{
			var changed = PropertyChanged;

			if (changed == null)
				return;

			changed.Invoke(this, new PropertyChangedEventArgs(name));
		}

		private bool _busy;

		public bool IsBusy
		{
			get { return _busy; }
			set
			{
				_busy = value;
				OnPropertyChanged();
				ToggleConnectionCommand.ChangeCanExecute();
			}
		}
	}
}

