using Aplicatie_Licenta.Service;
using Aplicatie_Licenta.Stores;
using Aplicatie_Licenta.ViewModels;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Aplicatie_Licenta.Commands.Async
{
    public class DeleteProfileCommand : AsyncCommandBase
    {
        private readonly NavigationStore _navigationStore;

        public DeleteProfileCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            var results = HandyControl.Controls.MessageBox.Show("Are you sure you whant to delete this post?", "Delete post",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (results == MessageBoxResult.Yes)
            {
                await UserService.DeleteUser().ContinueWith(t =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        UserService.CurrentUser = null;
                        _navigationStore.CurrentViewModel = new LogInViewModel(_navigationStore);
                    }
                    else
                    {
                        Growl.Fatal(t.Exception?.InnerException?.Message, "Notff");
                    }
                });
            }
        }
    }
}
