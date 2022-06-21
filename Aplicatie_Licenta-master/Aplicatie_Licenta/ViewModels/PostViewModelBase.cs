using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Service.Schemas.Post;
using System;
using System.Linq;

namespace Aplicatie_Licenta.ViewModels
{
    public abstract class PostViewModelBase:ViewModelBase
    {
        protected readonly Post _post;
        public String Username => _post.User.Username;

        public String ProfilePictureUrl => _post.User.ProfileImage ?? "https://data.whicdn.com/images/353151432/original.jpg";
        public String MainImageUrl => _post.MainImage;
        public int NumberLikes => _post.Saves;
        public bool HasLike => _post.Saved; 
        public string HearthIcon => HasLike ? "icons/heart_solid.png" : "icons/heart_white.png";

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
            OnPropertyChanged(nameof(HearthIcon));
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
            OnPropertyChanged(nameof(HearthIcon));
        }
    }
}
