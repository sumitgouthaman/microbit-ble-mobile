using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;

namespace MicrobitBLE.ViewModels
{
	public class DevicesViewModel : INotifyPropertyChanged
	{
		private IAdapter _adapter;
		private IBluetoothLE _bluetoothLe;

		public ObservableCollection<IDevice> DetectedDevices { get; private set; }

		public DevicesViewModel()
		{
			DetectedDevices = new ObservableCollection<IDevice>();

			_bluetoothLe = CrossBluetoothLE.Current;
			_adapter = _bluetoothLe.Adapter;

			_adapter.DeviceDiscovered += DeviceDiscovered;
		}

		public Task StartScanning()
		{
			return _adapter.StartScanningForDevicesAsync();
		}

		public Task StopScanning()
		{
			return _adapter.StopScanningForDevicesAsync();
		}

		void DeviceDiscovered(object sender, DeviceEventArgs e)
		{
			DetectedDevices.Add(e.Device);
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

