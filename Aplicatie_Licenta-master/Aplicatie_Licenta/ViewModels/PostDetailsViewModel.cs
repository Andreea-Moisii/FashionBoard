using Aplicatie_Licenta.Commands;
using Aplicatie_Licenta.Commands.Normal;
using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Aplicatie_Licenta.ViewModels
{
    public class PostDetailsViewModel : PostViewModelBase
    {
        // naviagation store
        private readonly NavigationStore _navigationStore;
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;


        // display data
        public IEnumerable<Image> Images => _post.Images;
        public string Details => _post.Description;
        public string Price => $"{_post.Price} lei";
        public IEnumerable<string> Colors
        {
            get
            {
                var colors = new List<string>();
                foreach (var image in _post.Images)
                {
                    colors.Add(image.color1);
                    colors.Add(image.color2);
                    colors.Add(image.color3);
                }
                return colors;
            }
        }

        private int index = 0;
        public string Image => _post.Images.Skip(index).First().url;


        // comands 
        public ICommand LikeCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand NextImageCommand { get; }
        public ICommand PreviousImageCommand { get; }
        public ICommand ViewProfileCommand { get; }
        public ICommand SearchColorCmd { get; }
        


        public PostDetailsViewModel(NavigationStore navigationStore, Post post, ViewModelBase lastViewModel) : base(post)
        {
            _navigationStore = navigationStore;

            LikeCommand = new LikeCommand(this);
            BackCommand = new NavigateCommand(() => lastViewModel, navigationStore);
            NextImageCommand = new ExecuteCommand(NextImage);
            PreviousImageCommand = new ExecuteCommand(PreviousImage);
            ViewProfileCommand = new NavigateCommand(
                () => ProfileViewModel.LoadProfileViewModel(Username, navigationStore, this),
                navigationStore);
            SearchColorCmd = new SearchColorPostsCommand(navigationStore);
        }

        public void NextImage()
        {
            index++;
            if (index >= _post.Images.Count())
                index = 0;
            OnPropertyChanged(nameof(Image));

        }

        public void PreviousImage()
        {
            index--;
            if (index < 0)
            {
                index = _post.Images.Count() - 1;
                index = index < 0 ? 0 : index;
            }
            OnPropertyChanged(nameof(Image));
        }



    }
}
