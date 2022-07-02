using Aplicatie_Licenta.Commands;
using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Service;
using Aplicatie_Licenta.Stores;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Aplicatie_Licenta.ViewModels
{
    internal class ProfileViewModel : ViewModelBase
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

        // Filters for search //
        private string _color = "";
        public string Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        private bool _newestCheck = true;
        public bool NewestCheck
        {
            get => _newestCheck;
            set => _newestCheck = value;
        }

        public bool _popularCheck = false;
        public bool PopularCheck
        {
            get => _popularCheck;
            set => _popularCheck = value;
        }

        public bool _priceLHCheck = false;
        public bool PriceLHCheck
        {
            get => _priceLHCheck;
            set => _priceLHCheck = value;
        }

        public bool _priceHLCheck = false;
        public bool PriceHLCheck
        {
            get => _priceHLCheck;
            set => _priceHLCheck = value;
        }


        public ICommand SelectColorCmd { get; }
        public ICommand ClearColorCmd { get; }



        public ProfileViewModel(NavigationStore navigationStore, ViewModelBase? fromViewModel = null)
        {
            _navigationStore = navigationStore;
            _viewablePosts = new ObservableCollection<PostCardViewModel>();
            _user = new User();

            BackCommand = new NavigateCommand(() => fromViewModel != null ? fromViewModel : new HomeViewModel(navigationStore), navigationStore);
            // filter // 
            SelectColorCmd = new ExecuteCommand(() =>
            {
                var picker = SingleOpenHelper.CreateControl<ColorPicker>();
                var window = new PopupWindow
                {
                    PopupElement = picker,
                    Title = "Pick a color",
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    ShowInTaskbar = true,
                    AllowsTransparency = true,
                    WindowStyle = WindowStyle.None
                };
                picker.SelectedColorChanged += OnColorChange;
                picker.Canceled += delegate { window.Close(); };
                picker.Confirmed += delegate { window.Close(); };
                window.Show();
            });

            ClearColorCmd = new ExecuteCommand(() =>
            {
                Color = "";
            });
        }

        private void OnColorChange(object? sender, FunctionEventArgs<Color> e)
        {
            Color = e.Info.ToString();
        }

        public static ViewModelBase LoadProfileViewModel(string username, NavigationStore navigationStore, ViewModelBase? fromViewModel = null)
        {
            var profileViewModel = new ProfileViewModel(navigationStore, fromViewModel);
            profileViewModel.LoadProfile(username, navigationStore);

            return profileViewModel;
        }

        public async void LoadProfile(string username, NavigationStore navigation)
        {
            IsLoading = true;
            _viewablePosts.Clear();
            if (UserService.CurrentUser?.Username == username)
            {
                _user = UserService.CurrentUser;
            }
            else
            {
                await UserService.GetUserByUsername(username).ContinueWith(
                    (task) =>
                    {
                        _user = task.Result;
                        OnPropertyChanged(nameof(Username));
                        OnPropertyChanged(nameof(Description));
                        OnPropertyChanged(nameof(ProfilePictureUrl));

                    });
            }

            await PostService.GetAllPostsForUser(username).ContinueWith(
            (task) =>
            {
                // move to UI thread
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    foreach (var post in task.Result)
                    {
                        _viewablePosts.Add(new PostCardViewModel(navigation, post, this));
                    }
                    IsLoading = false;
                });
            });
        }

    }
}
