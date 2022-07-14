using System.Windows;
using System.Windows.Controls;

namespace MusicG.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        public void GenerateMusicButton(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new GenerateMusic());
        }

        public void LicenseButton(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new License());
        }

        public void CloseButton(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
