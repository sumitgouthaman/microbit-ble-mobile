using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Xamarin.Forms;

namespace MicrobitBLE.ViewModels
{
	public class DevicesViewModel : INotifyPropertyChanged
	{
		private IAdapter _adapter;
		private IBluetoothLE _bluetoothLe;
		private bool _isScanning;

		public ObservableCollection<IDevice> DetectedDevices { get; private set; }
		public Command ReScanCommand { get; set; }

		public bool IsScanning
		{
			get { return _isScanning; }
			set
			{
				_isScanning = value;
				OnPropertyChanged();
				ReScanCommand.ChangeCanExecute();
			}
		}

		public DevicesViewModel()
		{
			DetectedDevices = new ObservableCollection<IDevice>();

			_bluetoothLe = CrossBluetoothLE.Current;
			_adapter = _bluetoothLe.Adapter;

			_adapter.DeviceDiscovered += DeviceDiscovered;
			_adapter.ScanTimeoutElapsed += ScanTimeoutElapsed;

			ReScanCommand = new Command(
				StartScanning,
				() => !IsScanning);
		}

		public async void StartScanning()
		{
			if (IsScanning)
			{
				return;
			}
			DetectedDevices.Clear();
			//_adapter.DiscoveredDevices.Clear();
			foreach (IDevice _connectedDevice in _adapter.ConnectedDevices)
			{
				await _adapter.DisconnectDeviceAsync(_connectedDevice);
			}
			IsScanning = true;
			await _adapter.StartScanningForDevicesAsync();
		}

		public async void StopScanning()
		{
			if (!IsScanning)
			{
				return;
			}
			IsScanning = false;
			await _adapter.StopScanningForDevicesAsync();
		}

		public void UpdateList()
		{
			DetectedDevices.Clear();
			foreach (IDevice device in _adapter.DiscoveredDevices)
			{
				DetectedDevices.Add(device);
			}
		}

		void DeviceDiscovered(object sender, DeviceEventArgs e)
		{
			if (!String.IsNullOrWhiteSpace(e.Device.Name))
				DetectedDevices.Add(e.Device);
		}

		void ScanTimeoutElapsed(object sender, EventArgs e)
		{
			IsScanning = false;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged([CallerMemberName] string name = null)
		{
			var changed = PropertyChanged;

			if (changed == null)
				return;

			changed.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}

