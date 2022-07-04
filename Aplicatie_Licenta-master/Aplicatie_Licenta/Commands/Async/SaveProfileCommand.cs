using Aplicatie_Licenta.Service;
using Aplicatie_Licenta.ViewModels;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Commands.Async
{
    public class SaveProfileCommand : AsyncCommandBase
    {
        private readonly SettingsViewModel _viewModel;

        public SaveProfileCommand(SettingsViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                _viewModel.IsLoading = true;
                var user = _viewModel.User;
                if (user.ProfileImage != UserService.CurrentUser.ProfileImage && user.ProfileImage != "")
                {
                    var image = await ImageService.UploadImage(user.ProfileImage.Remove(0, 8));
                    user.ProfileImage = image.url;
                }

                await UserService.UpdateUser(user).ContinueWith(t =>
                {
                    _viewModel.IsLoading = false;
                    if (t.IsFaulted)
                    {
                        Growl.Error(t.Exception?.InnerException?.Message, "Notff");
                    }
                    else
                    {
                        Growl.Success("Profile updated", "Notff");
                        UserService.CurrentUser = user;
                    }
                });
            }
            catch (Exception ex)
            {
                Growl.Fatal(ex.Message, "Notff");
            }
        }
    }
}
