using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace harjoitustyo
{

    public sealed partial class MainPage : Page
    {
        public string playerName = "Player";
        private Player player;     

        public MainPage()
        {
            this.InitializeComponent();
            PlayerNameTextBox.TextChanged += PlayerNameTextBox_TextChanged;
        }

        private void PlayerNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlayerNameButton.IsEnabled = true;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            // add and navigate to a new page
            this.Frame.Navigate(typeof(Game), playerName);
        }

        private void PlayerNameButton_Click(object sender, RoutedEventArgs e)
        {
            playerName = PlayerNameTextBox.Text;
        }


        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            AfterGameStackPanel.Visibility = Visibility.Collapsed;
            HowToPlayStackPanel.Visibility = Visibility.Collapsed;
            HighscoresStackPanel.Visibility = Visibility.Collapsed;
            NewGameStackPanel.Visibility = Visibility.Visible;
        }

        private void HighscoresButton_Click(object sender, RoutedEventArgs e)
        {
            AfterGameStackPanel.Visibility = Visibility.Collapsed;
            HowToPlayStackPanel.Visibility = Visibility.Collapsed;
            NewGameStackPanel.Visibility = Visibility.Collapsed;
            HighscoresStackPanel.Visibility = Visibility.Visible;
        }

        private void HowToPlayButton_Click(object sender, RoutedEventArgs e)
        {
            AfterGameStackPanel.Visibility = Visibility.Collapsed;
            NewGameStackPanel.Visibility = Visibility.Collapsed;
            HighscoresStackPanel.Visibility = Visibility.Collapsed;
            HowToPlayStackPanel.Visibility = Visibility.Visible;
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }

    }
}
