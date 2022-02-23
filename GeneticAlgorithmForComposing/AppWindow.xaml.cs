using GeneticAlgorithmForComposing.Menu;
using Manufaktura.Controls.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GeneticAlgorithmForComposing
{
    /// <summary>
    /// Logika interakcji dla klasy AppWindow.xaml
    /// </summary>
    public partial class AppWindow : Window
    {
        public static Score score;

        public AppWindow()
        {
            InitializeComponent();
            Switcher.pageSwitcher = this;
            Switcher.Switch(new MainMenu());
        }

        public void Navigate(UserControl nextPage)
        {
            contentControl.Content = nextPage;
        }

        public void BackButton(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainMenu());
        }
    }
}
