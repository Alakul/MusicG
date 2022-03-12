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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicG.Menu
{
    /// <summary>
    /// Logika interakcji dla klasy License.xaml
    /// </summary>
    public partial class License : UserControl
    {
        public License()
        {
            InitializeComponent();
        }

        private void BackButton(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainMenu());
        }

        private void EnterButton(object sender, RoutedEventArgs e)
        {
            imageArrow.Source = new BitmapImage(new Uri(@"/Images/arrowGray.png", UriKind.Relative));
        }

        private void LeaveButton(object sender, RoutedEventArgs e)
        {
            imageArrow.Source = new BitmapImage(new Uri(@"/Images/arrowBlack.png", UriKind.Relative));
        }
    }
}
