using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ViewsDemo.Helpers
{
    public class ThemeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (Enum.TryParse(value.ToString(), out OSAppTheme parsedTheme))
            {
                return parsedTheme;
            }

            return OSAppTheme.Unspecified;
        }
    }
}
