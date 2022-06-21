using Aplicatie_Licenta.Service;
using Aplicatie_Licenta.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Commands.Async
{
    public class RegisterCommand : AsyncCommandBase
    {
        private readonly LogInViewModel _logInViewModel;

        public RegisterCommand(LogInViewModel logInViewModel)
        {
            _logInViewModel = logInViewModel;
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            if (!_logInViewModel.CheckRegister()) return;

            await AuthService.Instance.Register(_logInViewModel.RegisterUsername, _logInViewModel.RegisterPassword, _logInViewModel.RegisterEmail)
                .ContinueWith(t =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        _logInViewModel.RegisterUsername = "";
                        _logInViewModel.OnPropertyChanged(nameof(_logInViewModel.RegisterUsername));

                        _logInViewModel.RegisterPassword = "";
                        _logInViewModel.OnPropertyChanged(nameof(_logInViewModel.RegisterPassword));

                        _logInViewModel.RegisterConfirmPassword = "";
                        _logInViewModel.OnPropertyChanged(nameof(_logInViewModel.RegisterConfirmPassword));

                        _logInViewModel.RegisterEmail = "";
                        _logInViewModel.OnPropertyChanged(nameof(_logInViewModel.RegisterEmail));

                        _logInViewModel.RegisterEmail = "";
                        _logInViewModel.OnPropertyChanged(nameof(_logInViewModel.RegisterEmail));

                        _logInViewModel.ErrorRegister = "Account created successfully!";
                    }
                    else
                    {
                        _logInViewModel.ErrorRegister = "Error creating account!";
                    }
                });
               
        }
    }
}
