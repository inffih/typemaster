using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace harjoitustyo
{

    public sealed partial class GameOverPage : Page
    {
        private Player player;
        private string lastPlayerName;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            player = (Player)e.Parameter;
            PlayerNameTextBox.Text = player.PlayerName;
            TotalScoreTextBox.Text = player.PlayerScore.ToString();
            lastPlayerName = player.PlayerName;
            base.OnNavigatedTo(e);
        }


        public GameOverPage()
        {
            this.InitializeComponent();
        }

        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), lastPlayerName);
        }
    }
}
