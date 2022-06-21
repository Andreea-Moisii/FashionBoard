using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Themes;


namespace Aplicatie_Licenta
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // set sideMenu selected item to the first item
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            // show window size in console
            Console.WriteLine(this.Width + " " + this.Height);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // show window size in console
            Console.WriteLine(this.Width + " " + this.Height);
        }
    }
}
