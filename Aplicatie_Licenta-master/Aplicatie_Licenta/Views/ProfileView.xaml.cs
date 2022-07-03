using Aplicatie_Licenta.ViewModels;
using System.Windows.Controls;

namespace Aplicatie_Licenta.Views
{
    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    public partial class ProfileView : UserControl
    {
        public ProfileView()
        {
            InitializeComponent();
        }

        private void DrawerBottom_Closed(object sender, System.Windows.RoutedEventArgs e)
        {
            var context = DataContext as ProfileViewModel;
            context?.LoadProfile(context.Username);
        }
    }
}
