using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Aplicatie_Licenta.Convertors
{
    public class InverseBooleanConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool valueBool && valueBool ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
