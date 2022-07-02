using Aplicatie_Licenta.Stores;

namespace Aplicatie_Licenta.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private ViewModelBase CurrentViewMovel => _navigationStore.CurrentViewModel;

        public SettingsViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
    }

}
