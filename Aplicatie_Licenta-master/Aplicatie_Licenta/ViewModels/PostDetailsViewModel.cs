using Aplicatie_Licenta.Commands;
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
        public IEnumerable<string> Images => _post.Images;
        public string Details => _post.Description;
        public string Price => $"{_post.Price} lei";
        public IEnumerable<string> Colors => _post.Colors;

        private int index = 0;
        public string Image => _post.Images.Skip(index).First();


        // comands 
        public ICommand LikeCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand NextImageCommand { get; }
        public ICommand PreviousImageCommand { get; }
        public ICommand ViewProfileCommand { get; }


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
