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
            get { return (bool)GetValue(HasValueProperty); }
            set { SetValue(HasValueProperty, value); }
        }
        // Using a DependencyProperty as the backing store for HasValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasValueProperty =
            DependencyProperty.Register("HasValue", typeof(bool), typeof(ImageSelector), new PropertyMetadata(false));



        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { 
                SetValue(ImageProperty, value);
                if (value == null || value == "") HasValue = false;
                else HasValue = true;
            }
        }
        // Using a DependencyProperty as the backing store for Image.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(string), typeof(ImageSelector), new PropertyMetadata(""));


        public string Filter
        {
            get => (string)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(
       "Filter", typeof(string), typeof(ImageSelector), new PropertyMetadata("(.jpg)|*.jpg|(.png)|*.png"));


        public ICommand ImgCommand
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }
            set
            {
                SetValue(CommandProperty, value);
            }
        }
        public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register("ImgCommand", typeof(ICommand), typeof(ImageSelector), new UIPropertyMetadata(null));

       

        public ImageSelector()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Image == null || Image == "")
            {
                var dialog = new OpenFileDialog
                {
                    RestoreDirectory = true,
                    Filter = Filter
                };

                if (dialog.ShowDialog() == true)
                {
                    var uri = new Uri(dialog.FileName, UriKind.RelativeOrAbsolute);
                    Image = uri.AbsoluteUri;
                    HasValue = true;

                    ImgChangeArgs parameter = new ImgChangeArgs {
                        Image = Image,
                        Operation = true
                    };
                    ImgCommand.Execute(parameter);
                }
            }
            else
            {
                var lst_img = Image;
                Image = "";
                HasValue = false;

                ImgChangeArgs parameter = new ImgChangeArgs
                {
                    Image = Image,
                    Operation = false
                };
                ImgCommand.Execute(parameter);
            }
        }
    }
}
