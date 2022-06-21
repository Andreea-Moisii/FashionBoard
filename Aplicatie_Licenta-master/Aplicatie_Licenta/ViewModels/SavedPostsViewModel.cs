using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.ViewModels
{
    public class SavedPostsViewModel: ViewModelBase
    {
        private readonly ObservableCollection<PostCardViewModel> _viewablePosts;

        public IEnumerable<PostCardViewModel> ViewablePosts => _viewablePosts;
        public SavedPostsViewModel(NavigationStore navigationStore)
        {
            _viewablePosts = new ObservableCollection<PostCardViewModel>();


        }
    }
}
