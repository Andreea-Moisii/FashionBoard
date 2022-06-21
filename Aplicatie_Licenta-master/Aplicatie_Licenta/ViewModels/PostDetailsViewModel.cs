using Aplicatie_Licenta.Commands;
using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Service.Schemas.Post;
using Aplicatie_Licenta.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aplicatie_Licenta.ViewModels
{
    public class PostDetailsViewModel: PostViewModelBase
    {
        public ICommand LikeCommand { get; }
        public ICommand BackCommand { get; }

        public IEnumerable<string> Images => _post.Images;
        public string Description => _post.Description;
        
        public PostDetailsViewModel(NavigationStore navigationStore, Post post, ViewModelBase lastViewModel) : base(post)
        {
            LikeCommand = new LikeCommand(this);
            BackCommand = new NavigateCommand(() => lastViewModel, navigationStore);
        }
        
    }
}
