using Aplicatie_Licenta.Commands.Async;
using Aplicatie_Licenta.Stores;
using System.Windows.Input;

namespace Aplicatie_Licenta.ViewModels
{
    public class LogInViewModel : ViewModelBase
    {

        private string? _errorLogin;
        public string? ErrorLogin
        {
            get { return _errorLogin; }
            set
            {
                _errorLogin = value;
                OnPropertyChanged(nameof(ErrorLogin));
            }
        }
        public string? LoginUsername { get; set; }
        public string? LoginPassword { get; set; }
        public ICommand LogInCommand { get; }


        private string? _errorRegister;
        public string? ErrorRegister
        {
            get { return _errorRegister; }
            set
            {
                _errorRegister = value;
                OnPropertyChanged(nameof(ErrorRegister));
            }
        }
        public string? RegisterUsername { get; set; }
        public string? RegisterPassword { get; set; }
        public string? RegisterConfirmPassword { get; set; }
        public string? RegisterEmail { get; set; }
        public ICommand RegisterCommand { get; }



        public LogInViewModel(NavigationStore navigationStore)
        {
            LogInCommand = new LoginCommand(this, navigationStore);
            RegisterCommand = new RegisterCommand(this);
        }

        public bool CheckRegister()
        {
            if (RegisterUsername == null || RegisterPassword == null || RegisterConfirmPassword == null || RegisterEmail == null)
            {
                ErrorRegister = "Please fill in all fields";
                return false;
            }
            if (RegisterPassword != RegisterConfirmPassword)
            {
                ErrorRegister = "Passwords do not match";
                return false;
            }
            if (RegisterUsername.Length < 3)
            {
                ErrorRegister = "Username must be at least 3 characters long";
                return false;
            }
            return true;
        }

        public bool CheckLogin()
        {
            if (LoginUsername == null || LoginPassword == null)
            {
                ErrorLogin = "Please fill in all fields";
                return false;
            }
            return true;
        }
    }
}
