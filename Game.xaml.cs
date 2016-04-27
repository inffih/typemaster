using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace harjoitustyo
{

    public sealed partial class Game : Page
    {

        // variables
        private string[] WordsArray = File.ReadAllLines("words.txt");
        private string pressedKey;
        private string currentWord;

        private int RoundTime = 10;
        private int keyPressIndex = 0;
        private int randomIndex;

        private bool allowTyping = false;

        private DispatcherTimer delayNewWord;
        private DispatcherTimer timer;
        private MediaElement keyClick;
        private MediaElement wrongKeyClick;

        Random rnd = new Random();    
        Stopwatch stopwatch = new Stopwatch();
        Player player = new Player();
        private DispatcherTimer flashScore;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // get player's name from mainpage and set it to player object
            string playerName = e.Parameter.ToString();
            player.PlayerName = playerName;
            // if player did not insert a name, use default name "Player"
            if (player.PlayerName == "")
            {
                player.PlayerName = "Player";
            }
            // show player name to the player
            PlayerNameTextBox.Text = player.PlayerName;
            base.OnNavigatedTo(e);
        }

        private async void PlayKeyClick(int key)
        {

            keyClick = new MediaElement();
            StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets\\Keyclicks");
            // save keypressindex to string to get the corresponding wav file
            string filename = key.ToString() + ".wav";
            // use filename variable to get the correct file
            StorageFile file = await folder.GetFileAsync(filename);
            var stream = await file.OpenAsync(FileAccessMode.Read);
            keyClick.SetSource(stream, file.ContentType);
            keyClick.MediaOpened += KeyClick_MediaOpened;            

        }

        private async void PlayWrongKeyClick()
        {

            wrongKeyClick = new MediaElement();
            StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets\\Keyclicks");
            StorageFile file = await folder.GetFileAsync("wrong.wav");
            var stream = await file.OpenAsync(FileAccessMode.Read);
            wrongKeyClick.SetSource(stream, file.ContentType);
            wrongKeyClick.MediaOpened += WrongKeyClick_MediaOpened;

        }

        private void WrongKeyClick_MediaOpened(object sender, RoutedEventArgs e)
        {
            wrongKeyClick.Play();
        }

        private void KeyClick_MediaOpened(object sender, RoutedEventArgs e)
        {
            keyClick.Play();
        }


        public Game()
        {
            this.InitializeComponent();
            UpdateScore();
            loadNewWord();

            // Add key listener
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;

            // timer for round time
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            stopwatch.Start();
            timer.Start();

            // delay to load new word
            delayNewWord = new DispatcherTimer();
            delayNewWord.Tick += DelayNewWord_Tick;
            delayNewWord.Interval = new TimeSpan(0, 0, 1);

            // flash score textbox
            flashScore = new DispatcherTimer();
            flashScore.Tick += FlashScore_Tick;
            flashScore.Interval = new TimeSpan(0, 0, 1);
         
        }

        // this sets Score textbox's bg color back to white
        // after it has been changed to green or red
        // regarding the players typing skills
        private void FlashScore_Tick(object sender, object e)
        {
            flashScore.Stop();
            PlayerScoreTextBox.Background = new SolidColorBrush(Colors.White);
        }

        private void DelayNewWord_Tick(object sender, object e)
        {
            delayNewWord.Stop();
            loadNewWord();
        }

        private void Timer_Tick(object sender, object e)
        {
            // check if still time left or if gameover
            CountDownTextBox.Text = (RoundTime - stopwatch.Elapsed.Seconds).ToString();
            if ( (RoundTime - stopwatch.Elapsed.Seconds) <= 0)
            {
                stopwatch.Stop();
                timer.Stop();
                GameOver();
            }
        }

        private void GameOver()
        {
            // remove key listener  
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
            this.Frame.Navigate(typeof(GameOverPage), player);
        }

        private void loadNewWord()
        {
            // clear last word from screen
            PressedKeysStackPanel.Children.Clear();
            // reset keypress index
            keyPressIndex = 0;
            // load new word
            randomIndex = rnd.Next(0, WordsArray.Length);
            currentWord = WordsArray[randomIndex].ToUpper();
            currentWordTextBlock.Text = currentWord;
            // allow typing
            allowTyping = true;
        }

  
        // detect current pressed key 
        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            pressedKey = args.VirtualKey.ToString();
            keyPressIndex++;

            // check if typing is allowed
            // this ensures that the user can not register
            // key events while delayNewWord is running
            if (allowTyping)
            {

                // check if correct key is pressed
                if (pressedKey == currentWord[keyPressIndex - 1].ToString())
                {
                    // call and send info of pressed key to PlayKeyClick
                    PlayKeyClick(keyPressIndex);
                    // if word is complete, draw new word and update score
                    if (keyPressIndex < currentWord.Length)
                    {
                        createTextBox(pressedKey);
                    }
                    else
                    {
                        createTextBox(pressedKey);                                        
                        // give points based on given word lenght
                        player.AddPoints(currentWord.Length * 5);
                        foreach (UIElement txt in PressedKeysStackPanel.Children)
                        {
                            TextBox box = txt as TextBox;
                            box.Background = new SolidColorBrush(Colors.LightGreen);
                        }             
                        PlayerScoreTextBox.Background = new SolidColorBrush(Colors.LightGreen);
                        UpdateScore();
                        flashScore.Start();
                        // disable typing while delayNewWord runs
                        allowTyping = false;
                        delayNewWord.Start();              
                    }
                }
                // if wrong key is pressed clear word and update score
                else
                {
                    PlayWrongKeyClick();
                    PressedKeysStackPanel.Children.Clear();
                    keyPressIndex = 0;
                    player.DecreasePoints(5);
                    PlayerScoreTextBox.Background = new SolidColorBrush(Colors.Red);
                    UpdateScore();               
                    flashScore.Start();               
                }
            }
        }

        private void UpdateScore()
        {
            PlayerScoreTextBox.Text = player.GetScore().ToString();
        }

        private void createTextBox(string keyToPrint)
        {
            TextBox txt = new TextBox();
            txt.MinWidth = 40;
            txt.Height = 10;
            txt.Text = keyToPrint;
            txt.TextAlignment = TextAlignment.Center;           
            PressedKeysStackPanel.Children.Add(txt);
        }

        private void QuitToMainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            stopwatch.Stop();
            timer.Stop();
            // remove key listener  
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
            this.Frame.Navigate(typeof(MainPage), player);
        }
    }
}
