using Aplicatie_Licenta.ViewModels;
using HandyControl.Controls;
using System.Windows;

namespace Aplicatie_Licenta.Commands.Async
{
    public class DeletePostComand : CommandBase
    {
        private readonly PostViewModelBase _postViewModelBase;

        public DeletePostComand(PostViewModelBase postViewModelBase)
        {
            _postViewModelBase = postViewModelBase;
        }
        public override void Execute(object? parameter)
        {

            var results =HandyControl.Controls.MessageBox.Show("Are you sure you whant to delete this post?", "Delete post", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (results == MessageBoxResult.Yes)
            {
                _postViewModelBase.OnDeletePost();
            }
            else
            {
                return;
            }
        }
    }
}
