using Aplicatie_Licenta.Service;
using Aplicatie_Licenta.Service.Interface;
using Aplicatie_Licenta.Stores;
using Aplicatie_Licenta.ViewModels;
using System;
namespace Aplicatie_Licenta.Commands
{
    internal class NavigateCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<ViewModelBase> _createViewModel;

        public NavigateCommand(Func<ViewModelBase> createViewModel, NavigationStore navigationStore)
        {
            _createViewModel = createViewModel;
            _navigationStore = navigationStore;
        }

        public override void Execute(object? parameter)
        {
            if (IAuthService.LoginToken != null)
            {
                _navigationStore.CurrentViewModel = _createViewModel();
            }
            else
            {
                _navigationStore.CurrentViewModel = new LogInViewModel(_navigationStore);
            }
        }
    }
}
