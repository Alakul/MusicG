using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MusicG.Pages
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
