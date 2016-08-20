using System;
using System.Globalization;
using Xamarin.Forms;

namespace MicrobitBLE.MicrobitUtils.Helpers
{
	public class IntToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int num;
			if (!int.TryParse(value.ToString(), out num))
			{
				return "[Not Available]";
			}

			if (num == int.MinValue)
				return "[Not Available]";

			return num.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

