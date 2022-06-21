using Aplicatie_Licenta.Service;
using Aplicatie_Licenta.Stores;
using Aplicatie_Licenta.ViewModels;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Commands.Async
{
    public class LoginCommand : AsyncCommandBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly LogInViewModel _logInViewModel;

        public LoginCommand(LogInViewModel logInViewModel, NavigationStore navigationStore)
        {
            _logInViewModel = logInViewModel;
            _navigationStore = navigationStore;
        }


        public override async Task ExecuteAsync(object? parameter)
        {
            if (!_logInViewModel.CheckLogin()) return;
            await AuthService.Login(_logInViewModel.LoginUsername, _logInViewModel.LoginPassword).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    if (task.Result)
                        _navigationStore.CurrentViewModel = new HomeViewModel(_navigationStore);
                    else
                        _logInViewModel.ErrorLogin = "Invalid username or password";
                }
            });
        }
    }
}
