using Aplicatie_Licenta.ViewModels;

namespace Aplicatie_Licenta.Commands
{
    public class LikeCommand : CommandBase
    {
        private readonly PostViewModelBase _postViewModel;

        public LikeCommand(PostViewModelBase postViewModel)
        {
            _postViewModel = postViewModel;
        }
        public override void Execute(object? parameter)
        {
            if (_postViewModel.HasLike) _postViewModel.Dislike();
            else _postViewModel.Like();
        }
    }
}
