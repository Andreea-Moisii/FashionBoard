using Aplicatie_Licenta.Commands;
using Aplicatie_Licenta.Commands.Async;
using Aplicatie_Licenta.Commands.Normal;
using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Service;
using Aplicatie_Licenta.Stores;
using System;
using System.Windows.Input;

namespace Aplicatie_Licenta.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private ViewModelBase CurrentViewMovel => _navigationStore.CurrentViewModel;

        // profile values //
        private User _user = new User();
        public User User => _user;

        public string Username => _user.Username;
        public string ProfilePictureUrl
        {
            get => _user.ProfileImage;
            set => _user.ProfileImage = value;
        }

        public string Description
        {
            get => _user.Description;
            set => _user.Description = value;
        }
        public string Email
        {
            get => _user.Email;
            set => _user.Email = value;
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public ICommand SaveProfileCmd { get;}
        public ICommand DeleteProfileCmd { get;}
        public ICommand ImgChangeCmd { get;}

        public SettingsViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _user.Username = UserService.CurrentUser?.Username ?? "";
            ProfilePictureUrl = UserService.CurrentUser?.ProfileImage ?? "";
            Description = UserService.CurrentUser?.Description ?? "";
            Email = UserService.CurrentUser?.Email ?? "";

            
            SaveProfileCmd = new SaveProfileCommand(this);
            DeleteProfileCmd = new DeleteProfileCommand(navigationStore);
            ImgChangeCmd = new ProfileImgChangeCmd(this);
        }

        public void RemoveImage(string image)
        {
            ProfilePictureUrl = "";
        }

        public void AddImage(string image)
        {
            ProfilePictureUrl = image;
        }
    }

}
