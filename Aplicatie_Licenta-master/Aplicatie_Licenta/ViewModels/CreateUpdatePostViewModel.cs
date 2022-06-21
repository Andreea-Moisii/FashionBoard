using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.ViewModels
{
    public class CreateUpdatePostViewModel: ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        private Post _post;

        public CreateUpdatePostViewModel(NavigationStore navigationStore, Post post)
        {
            _navigationStore = navigationStore;
            _post = post;
        }
    }
}
