using Aplicatie_Licenta.Commands;
using Aplicatie_Licenta.Service;
using Aplicatie_Licenta.Stores;
using System.Windows.Input;

namespace Aplicatie_Licenta.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public ICommand HomeCommand { get; }
        public ICommand ProfileCommand { get; }
        public ICommand SavesCommand { get; }
        public ICommand SettingsCommand { get; }


        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentModelChanged;

            HomeCommand = new NavigateCommand(() => new HomeViewModel(navigationStore), navigationStore);
            
            ProfileCommand = new NavigateCommand(
                () => ProfileViewModel.LoadProfileViewModel(UserService.CurrentUser?.Username, navigationStore),
                navigationStore);
            
            SavesCommand = new NavigateCommand(
                () => SavedPostsViewModel.LoadSavedPostsViewModel(navigationStore),
                navigationStore);
            
            SettingsCommand = new NavigateCommand(() => new SettingsViewModel(navigationStore), navigationStore);
        }

        private void OnCurrentModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
