using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Service;
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
        // nagigation store
        private readonly NavigationStore _navigationStore;
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        // saved post
        private readonly ObservableCollection<PostCardViewModel> _viewablePosts;
        public IEnumerable<PostCardViewModel> ViewablePosts => _viewablePosts;

        // loading bool for loading circle
        public bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
        public SavedPostsViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _viewablePosts = new ObservableCollection<PostCardViewModel>();
        }

        public static ViewModelBase LoadSavedPostsViewModel(NavigationStore navigationStore)
        {
            var savedPostViewModel = new SavedPostsViewModel(navigationStore);
            savedPostViewModel.LoadSavedPosts(navigationStore);
            
            return savedPostViewModel;
        }
        
        public async void LoadSavedPosts(NavigationStore navigationStore)
        {
            IsLoading = true;
            await PostService.GetAllSavedPost().ContinueWith(
                (task) =>
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        foreach (var post in task.Result)
                        {
                            _viewablePosts.Add(new PostCardViewModel(navigationStore, post, this));
                        }
                        IsLoading = false;
                    });
                });
        }
        
    }
}
