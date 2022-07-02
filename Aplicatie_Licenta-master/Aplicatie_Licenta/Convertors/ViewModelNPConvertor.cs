using Aplicatie_Licenta.ViewModels;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Aplicatie_Licenta.Convertors
{
    internal class ViewModelNPConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LogInViewModel || value is PostDetailsViewModel || value is SettingsViewModel || value is CreateUpdatePostViewModel)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
