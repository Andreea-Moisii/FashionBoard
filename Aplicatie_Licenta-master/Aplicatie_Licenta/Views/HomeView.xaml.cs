using Aplicatie_Licenta.ViewModels;
using System.Windows.Controls;

namespace Aplicatie_Licenta.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void DrawerBottom_Closed(object sender, System.Windows.RoutedEventArgs e)
        {
            var context = DataContext as HomeViewModel;
            context?.LoadPosts();
        }
    }
}
