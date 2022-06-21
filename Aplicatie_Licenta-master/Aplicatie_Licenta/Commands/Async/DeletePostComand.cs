using Aplicatie_Licenta.ViewModels;
using HandyControl.Controls;
using System.Threading.Tasks;
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
            
            Growl.Ask("Are you sure you whant to delete?", isConfirmed =>
            {
                if (isConfirmed)
                {
                    _postViewModelBase.OnDeletePost();
                }
                return true;
            }, "Notff");
        }
    }
}
