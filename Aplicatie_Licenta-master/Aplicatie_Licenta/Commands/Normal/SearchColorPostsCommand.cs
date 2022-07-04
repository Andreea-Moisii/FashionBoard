using Aplicatie_Licenta.Stores;
using Aplicatie_Licenta.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Commands.Normal
{
    public class SearchColorPostsCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public SearchColorPostsCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object? parameter)
        {
            string color = parameter as string ?? "";
            // add opasity co collor //
            color = color.Insert(1, "FF");
            _navigationStore.CurrentViewModel = new HomeViewModel(_navigationStore, color);
        }
    }
}
