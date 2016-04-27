using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
        

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // get player name from last game
            if (e.Parameter is Player)
            {
                player = (Player)e.Parameter;
                // add player to global players List
                (Application.Current as App).players.Add(player);
                playerName = player.PlayerName;
                PlayerNameTextBox.Text = player.PlayerName;
            }
            base.OnNavigatedTo(e);
        }


        public MainPage()
        {
            this.InitializeComponent();
            // enable name change button if textbox value changes
            PlayerNameTextBox.TextChanged += PlayerNameTextBox_TextChanged;
            // check if game music is already running. If not, then play it
            // basically this is run only once when the game starts and is never initialized again
            if (!App.musicIsRunning == true)
            {
                (App.Current as App).PlayGameMusic();
            }          
        }


        private void PlayerNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlayerNameButton.IsEnabled = true;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            // navitage to game page
            this.Frame.Navigate(typeof(Game), playerName);
        }

        private void PlayerNameButton_Click(object sender, RoutedEventArgs e)
        {
            // change player name 
            playerName = PlayerNameTextBox.Text;
            PlayerNameButton.Content = "Name changed!";
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
            drawHighscores();
        }

        private void drawHighscores()
        {
            // sort scores
            (App.Current as App).players.Sort((x, y) => -1 * x.PlayerScore.CompareTo(y.PlayerScore));
            // loop through every player in List
            foreach (Player plr in (App.Current as App).players)
            {
                TextBlock scoretxt = new TextBlock();
                scoretxt.Text = plr.PlayerName + ": " + plr.PlayerScore;
                HighscoresStackPanel.Children.Add(scoretxt);
            }

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
