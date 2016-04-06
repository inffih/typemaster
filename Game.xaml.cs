using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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

    public sealed partial class Game : Page
    {

        // variables
        private string[] WordsArray = File.ReadAllLines("words.txt");
        private string pressedKey;
        private string currentWord;

        private int RoundTime = 30;
        private int keyPressIndex = 0;
        private int randomIndex;
        private DispatcherTimer timer;

        Random rnd = new Random();    
        Stopwatch stopwatch = new Stopwatch();
        Player player = new Player();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // get player's name from mainpage and set it to player object
            string playerName = e.Parameter.ToString();
            player.PlayerName = playerName;
            PlayerNameTextBox.Text = player.PlayerName;
            base.OnNavigatedTo(e);
        }

        // main thing ...? whatever
        public Game()
        {
            this.InitializeComponent();          
            UpdateScore();
            loadNewWord();

            // timer for round time
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            stopwatch.Start();
            timer.Start();

            // Add key listener
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
         
        }

        private void Timer_Tick(object sender, object e)
        {
            // check if still time left or if gameover
            CountDownTextBox.Text = (RoundTime - stopwatch.Elapsed.Seconds).ToString();
            if ( (RoundTime - stopwatch.Elapsed.Seconds) <= 0)
            {
                GameOver();
            }

        }

        private void GameOver()
        {
            this.Frame.Navigate(typeof(GameOverPage), player);
        }

        private void loadNewWord()
        {
            // load new word
            randomIndex = rnd.Next(0, WordsArray.Length);
            currentWord = WordsArray[randomIndex].ToUpper();
            currentWordTextBox.Text = currentWord;
        }

        // detect current pressed key 
        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {

            pressedKey = args.VirtualKey.ToString();
            keyPressIndex++;

           // check if correct key is pressed
            if (pressedKey == currentWord[keyPressIndex - 1].ToString())
            {
                // if word is complete, draw new word and update score
                if (keyPressIndex < currentWord.Length)
                {
                    createTextBox(pressedKey);
                }
                else
                {
                    createTextBox(pressedKey);
                    loadNewWord();
                    keyPressIndex = 0;
                    PressedKeysStackPanel.Children.Clear();
                    player.AddPoints(20);
                    UpdateScore();      
                } 
            }
            // if wrong key is pressed clear word and update score
            else
            {
                PressedKeysStackPanel.Children.Clear();
                keyPressIndex = 0;
                player.DecreasePoints(5);
                UpdateScore();
            }
        }

        private void UpdateScore()
        {
            PlayerScoreTextBox.Text = player.PlayerScore.ToString();
        }

        private void createTextBox(string keyToPrint)
        {
            TextBox txt = new TextBox();
            txt.Width = 5;
            txt.Height = 20;
            txt.Text = keyToPrint;
            txt.TextAlignment = TextAlignment.Center;
            PressedKeysStackPanel.Children.Add(txt);
        }
    }
}
