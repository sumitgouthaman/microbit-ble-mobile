using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MicrobitBLE.Views.ServicePages;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;

namespace MicrobitBLE.MicrobitUtils.Services
{
	public abstract class AMicrobitService : IMicrobitService, INotifyPropertyChanged
	{
		public String FriendlyName { get; }

		public Guid Id { get; }

		public string Description { get; }

		public IService ServiceInstance { get; }

		public abstract ContentPage Page { get; }

		private IList<ICharacteristic> CharacteristicsToUpdate;
		private HashSet<Guid> SeenCharacteristics;

		public AMicrobitService(String friendlyName, String description, Guid id, IService service)
		{
			FriendlyName = friendlyName;
			Id = id;
			Description = description;
			ServiceInstance = service;
			CharacteristicsToUpdate = new List<ICharacteristic>();
			SeenCharacteristics = new HashSet<Guid>();
		}

		public void MarkCharacteristicForUpdate(ICharacteristic characteristic)
		{
			if (characteristic == null || SeenCharacteristics.Contains(characteristic.Id))
				return;

			CharacteristicsToUpdate.Add(characteristic);
		}

		public async void StartUpdates()
		{
			foreach (ICharacteristic characteristic in CharacteristicsToUpdate.ToList())
			{
				characteristic.StartUpdates();

				// This is due to a bug in the BLE library: https://github.com/xabre/xamarin-bluetooth-le/issues/64
				await Task.Delay(300);
			}
		}

		public async void StopUpdates()
		{
			foreach (ICharacteristic characteristic in CharacteristicsToUpdate)
			{
				characteristic.StopUpdates();

				// This is due to a bug in the BLE library: https://github.com/xabre/xamarin-bluetooth-le/issues/64
				//await Task.Delay(500);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string name = null)
		{
			var changed = PropertyChanged;

			if (changed == null)
				return;

			changed.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}

