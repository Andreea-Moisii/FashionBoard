using Aplicatie_Licenta.Service;
using Aplicatie_Licenta.Service.Interface;
using Aplicatie_Licenta.Stores;
using Aplicatie_Licenta.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Aplicatie_Licenta
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore _navigationStore;

        public App()
        {
            _navigationStore = new NavigationStore();
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationStore.CurrentViewModel = new LogInViewModel(_navigationStore);
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

    }
}
