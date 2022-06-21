using Aplicatie_Licenta.Commands;
using Aplicatie_Licenta.Commands.Async;
using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Service;
using Aplicatie_Licenta.Service.Schemas.Post;
using Aplicatie_Licenta.Stores;
using System;
using System.Windows.Input;

namespace Aplicatie_Licenta.ViewModels
{
    public class PostCardViewModel: PostViewModelBase
    {

        public ICommand ViewMoreCommand { get; }
        public ICommand LikeCommand { get; }
        public ICommand ViewProfileCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteComand { get; }

        
        public PostCardViewModel(NavigationStore navigationStore, Post post, ViewModelBase fromViewModel) :base(post)
        {   
            LikeCommand = new LikeCommand(this);
            ViewMoreCommand = new NavigateCommand(() => new PostDetailsViewModel(navigationStore, post, fromViewModel), navigationStore);
            ViewProfileCommand = new NavigateCommand(
               () => ProfileViewModel.LoadProfileViewModel(Username, navigationStore, fromViewModel),
               navigationStore);

            EditCommand = new ExecuteCommand(() => { });
            DeleteComand = new DeletePostComand(this);
        }

    }
}
