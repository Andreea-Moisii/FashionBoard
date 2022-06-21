using Aplicatie_Licenta.Service;
using Aplicatie_Licenta.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Aplicatie_Licenta.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly ObservableCollection<PostCardViewModel> _viewablePosts;
        public IEnumerable<PostCardViewModel> ViewablePosts => _viewablePosts;
        private readonly NavigationStore _navigationStore;


        public bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public HomeViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _viewablePosts = new ObservableCollection<PostCardViewModel>();
            LoadPosts();
        }

        private async void LoadPosts()
        {
            IsLoading = true;
            await PostService.Instance.GetAllPosts().ContinueWith(t =>
            {
                // move to UI thread
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    foreach (var post in t.Result)
                    {
                        _viewablePosts.Add(new PostCardViewModel(_navigationStore, post, this));
                    }

                    IsLoading = false;
                });
            });
        }
    }
}
