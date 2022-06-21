using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Service;
using Aplicatie_Licenta.Service.Schemas.Post;
using System;
using System.Linq;

namespace Aplicatie_Licenta.ViewModels
{
    public class PostViewModelBase:ViewModelBase
    {
        protected readonly Post _post;

        public int Id_Post => _post.Id_post;
        public string Username => _post.User.Username;
        public string ProfilePictureUrl => _post.User.ProfileImage ?? "https://data.whicdn.com/images/353151432/original.jpg";
        public string MainImageUrl => _post.MainImage;
        public int NumberLikes => _post.Saves;
        public bool HasLike => _post.Saved;
        public bool IsOwner => _post.User.Username == UserService.CurrentUser?.Username;

        public event Action OnPostDelete;
        public PostViewModelBase(Post post)
        {
            _post = post;
        }

        public void OnDeletePost()
        {
            OnPostDelete?.Invoke();
        }

        public virtual void Like()
        {
            if (!HasLike)
            {
                _post.Saves++;
                _post.Saved = true;
            }
            OnPropertyChanged(nameof(NumberLikes));
            OnPropertyChanged(nameof(HasLike));
        }

        public virtual void Dislike()
        {
            if (HasLike)
            {
                _post.Saves--;
                _post.Saved = false;
            }
            OnPropertyChanged(nameof(NumberLikes));
            OnPropertyChanged(nameof(HasLike));
        }

    }
}
