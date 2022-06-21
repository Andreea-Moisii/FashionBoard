using Aplicatie_Licenta.Commands;
using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Service;
using Aplicatie_Licenta.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aplicatie_Licenta.ViewModels
{
    internal class ProfileViewModel:ViewModelBase
    {
        // nagigation store
        private readonly NavigationStore _navigationStore;
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;


        // profile posts
        private readonly ObservableCollection<PostCardViewModel> _viewablePosts;
        public IEnumerable<PostCardViewModel> ViewablePosts => _viewablePosts;


        // profile data 
        private User _user;
        private readonly string _defaultPicture = "https://data.whicdn.com/images/353151432/original.jpg";
        public string ProfilePictureUrl => _user?.ProfileImage ?? _defaultPicture;
        public string Description => _user.Description;
        public string Username => _user.Username;



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

        public ICommand BackCommand { get; }

        public ProfileViewModel(NavigationStore navigationStore, ViewModelBase ?fromViewModel = null)
        {
            _navigationStore = navigationStore;
            _viewablePosts = new ObservableCollection<PostCardViewModel>();
            _user = new User();
            
            BackCommand = new NavigateCommand(() => fromViewModel != null ? fromViewModel : new HomeViewModel(navigationStore),navigationStore);
        }

        public ViewModelBase LoadProfileViewModel(string username, NavigationStore navigationStore, ViewModelBase? fromViewModel = null)
        {
            var profileViewModel = new ProfileViewModel(navigationStore, fromViewModel);
            profileViewModel.LoadProfile(username);

            return profileViewModel;
        }
        
        public async void LoadProfile(string username)
        {
            _viewablePosts.Clear();
            if (UserService.Instance?.CurrentUser?.Username == username)
            {
                _user = UserService.Instance.CurrentUser;
                _viewablePosts.Clear();
            }
            else
            {
                await UserService.Instance.GetUserByUsername(username).ContinueWith(
                    (task) =>
                    {
                        _user = task.Result;
                        _viewablePosts.Clear();
                    });

                await 
            }

    }
}
