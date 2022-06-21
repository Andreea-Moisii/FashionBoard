using Aplicatie_Licenta.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
