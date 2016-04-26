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
        List<Player> highscoreList = new List<Player>();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // get player name from last game
            if (e.Parameter is Player)
            {
                player = (Player)e.Parameter;
                playerName = player.PlayerName;
                PlayerNameTextBox.Text = player.PlayerName;
                highscoreList.Add(player);
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
            populateHighscoreList();

        }

        private void populateHighscoreList()
        {
            highscoreList.Add(new Player("BOT George", 20));
            highscoreList.Add(new Player("BOT Xavier", 100));
            highscoreList.Add(new Player("BOT Allu", 200));
            highscoreList.Add(new Player("BOT Quintin", 250));
            highscoreList.Add(new Player("BOT Pheonix", 375));
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

            showHighscores();

        }

        private void showHighscores()
        {
            foreach (Player highscore in highscoreList)
            {
                Debug.WriteLine(highscore.PlayerName + highscore.PlayerScore);
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
