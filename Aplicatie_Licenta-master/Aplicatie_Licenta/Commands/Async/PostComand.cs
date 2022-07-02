using Aplicatie_Licenta.Stores;
using Aplicatie_Licenta.ViewModels;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Commands.Async
{
    internal class PostComand : AsyncCommandBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly CreateUpdatePostViewModel _model;

        public PostComand(NavigationStore navigationStore, CreateUpdatePostViewModel model)
        {
            _navigationStore = navigationStore;
            _model = model;

        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _model.CreatePost();
            _navigationStore.CurrentViewModel = new HomeViewModel(_navigationStore);
        }
    }
}
