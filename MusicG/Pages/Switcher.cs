using System.Windows.Controls;

namespace MusicG.Pages
{
    public static class Switcher
    {
        public static AppWindow pageSwitcher;

        public static void Switch(UserControl newPage)
        {
            pageSwitcher.Navigate(newPage);
        }
    }
}
