using Aplicatie_Licenta.Service;
using Aplicatie_Licenta.ViewModels;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Commands
{
    public class LikeCommand : AsyncCommandBase
    {
        private readonly PostViewModelBase _postViewModel;

        public LikeCommand(PostViewModelBase postViewModel)
        {
            _postViewModel = postViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (_postViewModel.HasLike)
            {
                _postViewModel.Dislike();
                await PostService.DeleteSave(_postViewModel.Id_Post);
            }
            else
            {
                _postViewModel.Like();
                await PostService.AddSave(_postViewModel.Id_Post);
            }
        }
    }
}
