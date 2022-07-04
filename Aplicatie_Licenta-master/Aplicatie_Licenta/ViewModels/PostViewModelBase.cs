using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Service;
using System;

namespace Aplicatie_Licenta.ViewModels
{
    public class PostViewModelBase : ViewModelBase
    {
        protected readonly Post _post;

        public int Id_Post => _post.Id_post;
        public string Username => _post.User.Username;
        public string ProfilePictureUrl => _post.User.ProfileImage ?? "https://data.whicdn.com/images/353151432/original.jpg";
        public string MainImageUrl => _post.Images?[0].url ?? "https://t4.ftcdn.net/jpg/04/00/24/31/360_F_400243185_BOxON3h9avMUX10RsDkt3pJ8iQx72kS3.jpg";
        public int NumberLikes => _post.Saves;
        public bool HasLike => _post.Saved;
        public bool IsOwner => _post.User.Username == UserService.CurrentUser?.Username;

        public event Action<PostViewModelBase> PostDeletedEvent;
        
        public PostViewModelBase(Post post)
        {
            _post = post;
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

        public void PostDeleted()
        {
            PostDeletedEvent?.Invoke(this);
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
