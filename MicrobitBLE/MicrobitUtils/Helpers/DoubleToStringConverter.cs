using System;
using System.Globalization;
using Xamarin.Forms;

namespace MicrobitBLE.MicrobitUtils.Helpers
{
	public class DoubleToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double num;
			if (!double.TryParse(value.ToString(), out num))
			{
				return "[Not Available]";
			}

			if (double.IsNaN(num))
				return "[Not Available]";

			return num.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

