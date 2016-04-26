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
        private bool allowTyping;

        private int RoundTime = 10;
        private int keyPressIndex = 0;
        private int randomIndex;

        private DispatcherTimer delayNewWord;
        private DispatcherTimer timer;
        private MediaElement keyClick;

        Random rnd = new Random();    
        Stopwatch stopwatch = new Stopwatch();
        Player player = new Player();

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

        private void KeyClick_MediaOpened(object sender, RoutedEventArgs e)
        {
            keyClick.Play();
        }


        public Game()
        {
            this.InitializeComponent();
            allowTyping = true;
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

            // delay to load new word
            delayNewWord = new DispatcherTimer();
            delayNewWord.Tick += DelayNewWord_Tick;
            delayNewWord.Interval = new TimeSpan(0, 0, 1);
         
        }

        private void DelayNewWord_Tick(object sender, object e)
        {       
            delayNewWord.Stop();
            loadNewWord();
            allowTyping = true;
        }

        private void Timer_Tick(object sender, object e)
        {
            // check if still time left or if gameover
            CountDownTextBox.Text = (RoundTime - stopwatch.Elapsed.Seconds).ToString();
            if ( (RoundTime - stopwatch.Elapsed.Seconds) <= 0)
            {
                timer.Stop();
                allowTyping = false;
                GameOver();
            }
        }

        private void GameOver()
        {
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
            currentWordTextBox.Text = currentWord;
        }

  

        // detect current pressed key 
        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {

            if (allowTyping == true)
            {
                pressedKey = args.VirtualKey.ToString();
                keyPressIndex++;            

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
                            box.Background = new SolidColorBrush(Colors.Green);
                        }
                        allowTyping = false;
                        UpdateScore();
                        delayNewWord.Start();              
                    }
                }
                // if wrong key is pressed clear word and update score
                else
                {
                    Debug.WriteLine("incorrect key");
                    PressedKeysStackPanel.Children.Clear();
                    keyPressIndex = 0;
                    player.DecreasePoints(5);
                    UpdateScore();
                }
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
