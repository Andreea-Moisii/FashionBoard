using Aplicatie_Licenta.Service;
using Aplicatie_Licenta.ViewModels;
using System.Threading.Tasks;
using System.Windows;

namespace Aplicatie_Licenta.Commands.Async
{
    public class DeletePostComand : AsyncCommandBase
    {
        private readonly PostViewModelBase _postViewModelBase;

        public DeletePostComand(PostViewModelBase postViewModelBase)
        {
            _postViewModelBase = postViewModelBase;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            var results = HandyControl.Controls.MessageBox.Show("Are you sure you whant to delete this post?", "Delete post",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (results == MessageBoxResult.Yes)
            {
                await PostService.DeletePost(_postViewModelBase.Id_Post).ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        // go to current thread //
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            _postViewModelBase.PostDeleted();
                        });
                    }
                    else
                    {
                        HandyControl.Controls.MessageBox.Error("Something went wrong");
                    }
                });
            }
            else
            {
                return;
            }
        }
    }
}
