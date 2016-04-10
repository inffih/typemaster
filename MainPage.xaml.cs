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
        public string playerName;
        private MediaElement gameMusic;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // get player name from last game
            if (e.Parameter is string)
            {             
                playerName = e.Parameter.ToString();
                PlayerNameTextBox.Text = playerName;
            }
            base.OnNavigatedTo(e);
        }


        private async void PlayGameMusic()
        {
            gameMusic = new MediaElement();
            StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file = await folder.GetFileAsync("gamemusic.wav");
            var stream = await file.OpenAsync(FileAccessMode.Read);
            gameMusic.AutoPlay = true;
            gameMusic.IsLooping = true;
            gameMusic.SetSource(stream, file.ContentType);
        }


        public MainPage()
        {
            this.InitializeComponent();
            // enable name change button if textbox value changes
            PlayerNameTextBox.TextChanged += PlayerNameTextBox_TextChanged;
            // init bg music
            PlayGameMusic();
        }

        private void PlayerNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlayerNameButton.IsEnabled = true;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            // navitage to game page
            Debug.WriteLine(playerName);
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
