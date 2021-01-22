using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SismoNica_WP8._0.Common
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double dValue = double.Parse(value.ToString(), CultureInfo.InvariantCulture);

            if (dValue <= 2.0)
                return new SolidColorBrush(Colors.Gray);

            else if (dValue > 2.0 && dValue <= 4.0)
                return new SolidColorBrush(Colors.Black);

            else if (dValue > 4.0 && dValue <= 7.0)
                return new SolidColorBrush(Colors.Orange);

            else if (dValue > 7.0 && dValue <= 12.9)
                return new SolidColorBrush(Colors.Red);

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}