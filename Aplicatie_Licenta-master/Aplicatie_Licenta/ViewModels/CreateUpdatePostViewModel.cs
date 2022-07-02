using Aplicatie_Licenta.Commands;
using Aplicatie_Licenta.Commands.Async;
using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Service;
using Aplicatie_Licenta.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Aplicatie_Licenta.ViewModels
{
    public class CreateUpdatePostViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        // images added //
        private ObservableCollection<string> _viewableImages;
        public IEnumerable<string> ViewableImages => _viewableImages;
        private void Set(ref ObservableCollection<string> viewableImages, IEnumerable<string> value)
        {
            viewableImages.Clear();
            foreach (var item in value)
            {
                viewableImages.Add(item);
            }
        }

        // the new post greated //
        private Post _post;
        public string Description { get; set; }
        public string Price { get; set; }
        
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
            PostCommand = new PostComand(navigationStore, this);
        }


        public async void CreatePost()
        {
            // need red
            _viewableImages.Remove("");
            _post.Images = _viewableImages.ToArray();
            _post.Description = Description;
            _post.Price = float.Parse(Price);

            await PostService.CreatePost(_post);
        }


        public void AddImage(string image)
        {
            //_viewableImages.Remove("");
            _viewableImages.Insert(0,image);
            //_viewableImages.Add("");

            OnPropertyChanged(nameof(_viewableImages));
        }

        public void RemoveImage(string image)
        {
            _viewableImages.Remove(image);

            OnPropertyChanged(nameof(_viewableImages));
        }

    }
}
