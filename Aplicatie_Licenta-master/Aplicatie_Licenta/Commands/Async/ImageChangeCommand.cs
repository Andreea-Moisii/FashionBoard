using Aplicatie_Licenta.CustomComponents;
using Aplicatie_Licenta.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Commands.Async
{
    internal class ImageChangeCommand : CommandBase
    {
        private readonly CreateUpdatePostViewModel _viewModel;

        public ImageChangeCommand(CreateUpdatePostViewModel viewModel)
        {
            _viewModel = viewModel;
        }
            
        public override void Execute(object? parameter)
        {
            var p = parameter as ImgChangeArgs;
            if (p != null) {
                if (p.Operation)
                    _viewModel.AddImage(p.Image);
                else
                    _viewModel.RemoveImage(p.Image);
            }
        }
    }
}
