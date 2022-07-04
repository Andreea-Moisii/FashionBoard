using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Service;
using Aplicatie_Licenta.Stores;
using Aplicatie_Licenta.ViewModels;
using HandyControl.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Commands.Async
{
    internal class PostComand : AsyncCommandBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly CreateUpdatePostViewModel _model;
        private Post _post;

        public PostComand(NavigationStore navigationStore, CreateUpdatePostViewModel model, Post post)
        {
            _navigationStore = navigationStore;
            _model = model;
            _post = post;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                if (_model.ViewableImages.Count <= 1)
                {
                    Growl.Info("Add at least on image", "Notff");
                    return;
                }
                if (_model.Description == "")
                {
                    Growl.Info("Add a description", "Notff");
                    return;
                }
                if (_model.Price == "")
                {
                    Growl.Info("Add a price", "Notff");
                    return;
                }


                _model.IsLoading = true;
                var images = new List<string>(_model.ViewableImages);
                images.Remove("");
                foreach (string imgeUrl in images)
                {
                    _post.Images.Add(new Image { url = imgeUrl });
                }

                _post.Description = _model.Description;
                _post.Price = float.Parse(_model.Price);

                await PostService.CreatePost(_post).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        Growl.Error("Error creating post", "Notff");
                    }
                    else
                    {
                        _navigationStore.CurrentViewModel = new HomeViewModel(_navigationStore);
                        Growl.Success("New post created", "Notff");
                    }
                    _model.IsLoading = false;
                });
            }
            catch (System.Exception ex)
            {
                Growl.Warning(ex.Message, "Notff");
                _model.IsLoading = false;
            }

        }

    }
}
