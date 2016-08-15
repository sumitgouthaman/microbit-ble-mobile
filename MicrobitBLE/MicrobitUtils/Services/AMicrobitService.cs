using System;
using System.ComponentModel;
using MicrobitBLE.Views.ServicePages;
using Xamarin.Forms;

namespace MicrobitBLE.MicrobitUtils.Services
{
	public abstract class AMicrobitService : IMicrobitService, INotifyPropertyChanged
	{
		public String FriendlyName { get; }

		public Guid Id { get; }

		public string Description { get; }

		public abstract ContentPage Page { get; }

		public AMicrobitService(String friendlyName, Guid id, String descrption)
		{
			FriendlyName = friendlyName;
			Id = id;
			Description = descrption;
		}

		public abstract event PropertyChangedEventHandler PropertyChanged;
	}
}

