using HandyControl.Controls;

namespace Aplicatie_Licenta.CustomComponents
{
    /// <summary>
    /// Interaction logic for ImageSelectorUrl.xaml
    /// </summary>
    public partial class ImageSelectorUrl : ImageSelector
    {
        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set
            {
                SetValue(ImageProperty, value);
            }
        }
        // Using a DependencyProperty as the backing store for Image.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(string), typeof(ImageSelector), new PropertyMetadata(""));

        public ImageSelectorUrl()
        {
            InitializeComponent();
        }
    }
}
