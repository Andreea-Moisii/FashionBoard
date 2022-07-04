using Aplicatie_Licenta.Commands;
using Aplicatie_Licenta.Commands.Async;
using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Aplicatie_Licenta.ViewModels
{
    public class CreateUpdatePostViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        // images added //
        private ObservableCollection<string> _viewableImages;
        public ObservableCollection<string> ViewableImages
        {
            set => _viewableImages = value;
            get => _viewableImages;
        }


        // the new post greated //
        private Post _post;
        public string Description { get; set; } = "";
        public string Price { get; set; } = "";

        // loading bar //
        private bool _isLoading = false;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public ICommand ImgChangeCmd { get; }
        public ICommand CancelCommand { get; }
        public ICommand PostCommand { get; }
        public CreateUpdatePostViewModel(NavigationStore navigationStore, Post post)
        {
            _navigationStore = navigationStore;
            _post = post;

            _viewableImages = new ObservableCollection<string>();
            _viewableImages.Add("");
            ImgChangeCmd = new ImageChangeCommand(this);
            CancelCommand = new NavigateCommand(() => new HomeViewModel(navigationStore), navigationStore);
            PostCommand = new PostComand(navigationStore, this, post);
        }


        public void AddImage(string image)
        {
            _viewableImages.Add(image);
        }
        public void RemoveImage(string image)
        {
            _viewableImages.Remove(image);
        }
    }
}
