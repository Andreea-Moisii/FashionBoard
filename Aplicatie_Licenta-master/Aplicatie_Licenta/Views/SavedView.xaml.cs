using Aplicatie_Licenta.ViewModels;
using System.Windows.Controls;

namespace Aplicatie_Licenta.Views
{
    /// <summary>
    /// Interaction logic for SavedView.xaml
    /// </summary>
    public partial class SavedView : UserControl
    {
        public SavedView()
        {
            InitializeComponent();
        }

        private void DrawerBottom_Closed(object sender, System.Windows.RoutedEventArgs e)
        {
            var context = DataContext as SavedPostsViewModel;
            context?.LoadSavedPosts();
        }
    }
}
