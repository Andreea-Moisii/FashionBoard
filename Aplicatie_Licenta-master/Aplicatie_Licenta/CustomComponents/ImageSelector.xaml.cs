using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Aplicatie_Licenta.CustomComponents
{
    /// <summary>
    /// Interaction logic for ImageSelector.xaml
    /// </summary>
    public partial class ImageSelector : UserControl
    {
        public bool HasValue
        {
            get => (bool)GetValue(HasValueProperty); 
            set => SetValue(HasValueProperty, value); 
        }
        // Using a DependencyProperty as the backing store for HasValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasValueProperty =
            DependencyProperty.Register("HasValue", typeof(bool), typeof(ImageSelector), new PropertyMetadata(false));



        public string Image
        {
            get => (string)GetValue(ImageProperty); 
            set => SetValue(ImageProperty, value);
        }
        // Using a DependencyProperty as the backing store for Image.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(string), typeof(ImageSelector), new PropertyMetadata(""));


        public string Filter
        {
            get => (string)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }
        public static readonly DependencyProperty FilterProperty = 
            DependencyProperty.Register("Filter", typeof(string), typeof(ImageSelector), new PropertyMetadata("(.jpg)|*.jpg|(.png)|*.png"));


        public ICommand ImgCommand
        {
            get => (ICommand)GetValue(CommandProperty);
            set =>  SetValue(CommandProperty, value);
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("ImgCommand", typeof(ICommand), typeof(ImageSelector), new UIPropertyMetadata(null));



        public ImageSelector()
        {
            InitializeComponent();
            HasValue = false;
            Image = "";
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!HasValue)
            {
                var dialog = new OpenFileDialog
                {
                    RestoreDirectory = true,
                    Filter = Filter
                };

                if (dialog.ShowDialog() == true)
                {
                    SetValue(ImageProperty, new Uri(dialog.FileName, UriKind.RelativeOrAbsolute).AbsoluteUri);
                    SetValue(HasValueProperty, true);

                    ImgChangeArgs parameter = new ImgChangeArgs
                    {
                        Image = Image,
                        Operation = true
                    };
                    ImgCommand.Execute(parameter);
                }
            }
            else
            {
                var lst_img = Image;
                SetValue(ImageProperty, default(Image));
                SetValue(HasValueProperty, false);

                ImgChangeArgs parameter = new ImgChangeArgs
                {
                    Image = lst_img,
                    Operation = false
                };
                ImgCommand.Execute(parameter);
            }
        }
    }
}
