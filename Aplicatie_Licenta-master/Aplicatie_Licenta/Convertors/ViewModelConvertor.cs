using Aplicatie_Licenta.ViewModels;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Aplicatie_Licenta.Convertors
{
    internal class ViewModelConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is LogInViewModel model ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
