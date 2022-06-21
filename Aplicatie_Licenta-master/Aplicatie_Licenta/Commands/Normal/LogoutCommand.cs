using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Service;
using Aplicatie_Licenta.Stores;
using Aplicatie_Licenta.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Commands.Normal
{
    public class LogoutCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        public ViewModelBase currentViewModel => _navigationStore.CurrentViewModel;

        public LogoutCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object? parameter)
        {
            UserService.CurrentUser = new User();
            AuthService.LoginToken = "";

            _navigationStore.CurrentViewModel = new LogInViewModel(_navigationStore);
        }
    }
}
