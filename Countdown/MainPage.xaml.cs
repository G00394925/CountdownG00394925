using Microsoft.Maui.Graphics.Text;
using Plugin.Maui.Audio;
using Microsoft.Maui.Storage;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Countdown
{
    public partial class MainPage : ContentPage
    {
        private List<GameStats> gameStatsList;

        // Fields
        private readonly IAudioManager _audioManager;
        private readonly DownloadManager _downloadManager;

        IDispatcherTimer timer;

        // Create random number generator
        Random r = new Random();

        // Variables
        public int buttonPressed;
        public int playerTurn;
        public int currentRound;
        public int rounds = 6;
        public int timeRemaining;
        public int wordLength1;
        public int wordLength2;
        public string word1;
        public string word2;

        // Letter Arrays
        public char[] vowels = ['A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'U', 'U', 'U', 'U', 'U', 'U'];
        public char[] consonants = ['B', 'B', 'C', 'C', 'C', 'D', 'D', 'D', 'D', 'D', 'D', 'F', 'F', 'G', 'G', 'G', 'H', 'H', 'J', 'K', 'L', 'L', 'L', 'L', 'L', 'M', 'M', 'M', 'M', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'P', 'P', 'P', 'P', 'Q', 'R', 'R', 'R', 'R', 'R', 'R', 'R', 'R', 'R', 'S', 'S', 'S', 'S', 'S', 'S', 'S', 'S', 'S', 'T', 'T', 'T', 'T', 'T', 'T', 'T', 'T', 'T', 'V', 'W', 'X', 'Y', 'Z'];
        public char[] drawnLetters = new char[9]; // Store the drawn letters

        public MainPage(IAudioManager audioManager) // Audiomanager parameters were added to get and set audioManager object
        {
            InitializeComponent();
            this._audioManager = audioManager;
            _downloadManager = new DownloadManager();
            DownloadDictionary();
            StartGame();
        }

        private async void StartGame()
        {
            // Set variables
            playerTurn = r.Next(1, 3); // Decides which player to go first
            currentRound = 1;
            buttonPressed = 0;
            Timer.Text = "30";
            Score1.Text = "0";
            Score2.Text = "0";

            // Periodically update the status message to reflect on what is currently happening in the game
            GameStatus.Text = "Welcome to Countdown!";

            await Task.Delay(1000); // Pacing

            // Prompt user to input names
            Name1.Text = await DisplayPromptAsync("Welcome to Countdown", "Enter the name for Player 1", maxLength: 9);
            Name2.Text = await DisplayPromptAsync("Welcome to Countdown", "Enter the name for Player 2", maxLength: 9);

            // Round Begins
            if (playerTurn == 1)
            {
                GameStatus.Text = Name1.Text + ", choose your letters";
            }

            else if (playerTurn == 2)
            {
                GameStatus.Text = Name2.Text + ", choose your letters";
            }

            // Enable buttons
            VButton.IsEnabled = true;
            CButton.IsEnabled = true;

        }

        // Vowel button pressed
        private async void VButton_Clicked(object sender, EventArgs e)
        {
            if (buttonPressed < drawnLetters.Length)
            {
                char letter = GetLetter(1); // Aquire letter... number sent to differentiate between a vowel or consonant 
                drawnLetters[buttonPressed] = letter;
                buttonPressed++; // Keeps track of how many letters have been drawn
                UpdateLetterBox();

                // Disable the buttons once there are enough letters
                if (buttonPressed == 9)
                {
                    VButton.IsEnabled = false;
                    CButton.IsEnabled = false;

                    GameStatus.Text = "Letters chosen..."; // Update game status
                    await Task.Delay(3000);
                    StartRound();
                }
            }
        }

        // Consonant button prssed
        private async void CButton_Clicked(object sender, EventArgs e)
        {
            if (buttonPressed < drawnLetters.Length)
            {
                char letter = GetLetter(2);
                drawnLetters[buttonPressed] = letter;
                buttonPressed++;
                UpdateLetterBox();
                if (buttonPressed == 9)
                {
                    VButton.IsEnabled = false;
                    CButton.IsEnabled = false;

                    GameStatus.Text = "Letters chosen...";
                    await Task.Delay(2000);
                    StartRound();
                }
            }
        }
        private char GetLetter(int num)
        {
            char currentLetter = ' ';

            // Vowel
            if (num == 1)
            {
                int v = r.Next(67);
                currentLetter = vowels[v];
            }

            // Consonant
            else if (num == 2)
            {
                int c = r.Next(74);
                currentLetter = consonants[c];
            }

            return currentLetter;
        }

        private void UpdateLetterBox()
        {
            LetterBox.Text = new string(drawnLetters, 0, buttonPressed); // The letter is added to the UI element
        }

        private async void StartRound()
        {
            GameStatus.Text = "Get Ready...";
            await Task.Delay(2000);
            GameStatus.Text = "Your time starts...";
            await Task.Delay(2000);
            GameStatus.Text = "Now!";

            StartTimer();
            PlayAudio();

            await Task.Delay(31000);

            // Ask for length of words and convert string to int
            wordLength1 = Int32.Parse(await DisplayPromptAsync(Name1.Text, "How many letters in your word?"));
            wordLength2 = Int32.Parse(await DisplayPromptAsync(Name2.Text, "How many letters in your word?"));

            await Task.Delay(1000);

            // Ask for words
            word1 = await DisplayPromptAsync(Name1.Text, "What is your word?");
            word1 = word1.ToUpper(); // Convert word to uppercase to avoid Case-Sensitive issues 

            word2 = await DisplayPromptAsync(Name2.Text, "What is your word?");
            word2 = word2.ToUpper();

            // Verify the words
            VerifyWord(word1, wordLength1, Name1.Text);
            VerifyWord(word2, wordLength2, Name2.Text);

            // Update the score
            UpdateScore(wordLength1, wordLength2);

            await Task.Delay(2000);

            GameStatus.Text = "Scores have been updated";

            await Task.Delay(2000);

            // DEBUG
            currentRound = 6;

            // Begin next round out of no. of max rounds specified
            if (currentRound < rounds)
            {
                GameStatus.Text = "Starting next round...";

                await Task.Delay(2000);

                NextRound();
            }

            // Game end
            else
            {
                SaveResults(); // Save results of the game to History page

                if (Int32.Parse(Score1.Text) > Int32.Parse(Score2.Text))
                {
                    await DisplayAlert("Congratulations " + Name1.Text, "You have won this game of countdown.", "OK");
                }

                else if (Int32.Parse(Score2.Text) > Int32.Parse(Score1.Text))
                {
                    await DisplayAlert("Congratulations " + Name2.Text, "You have won this game of countdown.", "OK");
                }

                else if (Int32.Parse(Score1.Text) == Int32.Parse(Score2.Text))
                {
                    await DisplayAlert("It's a Tie!", Name1.Text + " and " + Name2.Text + ", you two ended the game with the same score!", "OK");
                }

                bool answer = await DisplayAlert("Game Over", "Would you like to play another game?", "Yes", "No");
                Debug.WriteLine("Answer: " + answer);

                if (answer == true)
                {
                    StartGame();
                }

                else if (answer == false) 
                {
                    App.Current.MainPage = new NavigationPage(new HomePage(_audioManager));
                }

            }
        }
        private async void PlayAudio()
        {
            var player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("clock.mp3"));

            player.Play(); // Play the clock music 
            await Task.Delay(32000); // Wait 32 seconds to allow the music to play fully
            player.Dispose(); // Dispose when not needed to prevent memory leak
        }

        private void StartTimer()
        {
            timeRemaining = 30; // Reset the time

            if (timer != null)
            {
                timer.Tick -= TimerTick;
            }

            timer = Dispatcher.CreateTimer(); // Create the timer 
            timer.Interval = TimeSpan.FromMilliseconds(1000); // Counts down timer for every 1 second
            timer.Tick += TimerTick;

            timer.Start(); // Start the timer
        }

        private void TimerTick(object sender, EventArgs e)
        {
            Timer.Text = (--timeRemaining).ToString();

            // Stop timer when time reaches 0
            if (timeRemaining == 0)
            {
                timer.Stop();
                GameStatus.Text = "Time's up!";
            }
        }

        private async void NextRound()
        {
            // Reset variabes
            Timer.Text = "30";
            ++currentRound;
            buttonPressed = 0;

            // Switch the starting player
            if (playerTurn == 1)
            {
                playerTurn = 2;
            }
            else
            {
                playerTurn = 1;
            }

            GameStatus.Text = "Round " + currentRound + " begin";
            await Task.Delay(2000);

            if (playerTurn == 1)
            {
                GameStatus.Text = Name1.Text + ", choose your letters";
            }

            else if (playerTurn == 2)
            {
                GameStatus.Text = Name2.Text + ", choose your letters";
            }

            VButton.IsEnabled = true;
            CButton.IsEnabled = true;

            LetterBox.Text = null;
        }


        private async void VerifyWord(string word, int length, string name)
        {
            // Convert word to a character array
            char[] wordChars = word.ToCharArray();

            // Check that the word matches the length that was given by the player
            if (wordChars.Length != length)
            {

                if (name == Name1.Text)
                {
                    wordLength1 = 0; // Player gets 0 points if there is an issue with their word
                    await DisplayAlert("Error", Name1.Text + ", the word you have chosen does not match the specified length of your word", "Okay");
                    return;
                }

                else if (name == Name2.Text)
                {
                    wordLength2 = 0;
                    await DisplayAlert("Error", Name2.Text + ", the word you have chosen does not match the specified length of your word", "Okay");
                    return;
                }
            }

            // Clone drawnLetters[] to be able to keep track of what letters were checked and to only check them once
            char[] lettersAvailable = (char[])drawnLetters.Clone();

            foreach (char letter in wordChars)
            {
                int found = 0;

                for (int i = 0; i < lettersAvailable.Length; i++)
                {
                    if (lettersAvailable[i] == letter)
                    {
                        lettersAvailable[i] = '\0'; // Remove found letter
                        found = 1;
                        break;
                    }
                }

                // If a letter is incorrectly used in the player's word
                if (found != 1)
                {
                    if (name == Name1.Text)
                    {
                        wordLength1 = 0;
                        await DisplayAlert("Error", Name1.Text + ", the word you chose contains an invalid letter", "Okay");
                        return;
                    }

                    else if (name == Name2.Text)
                    {
                        wordLength2 = 0;
                        await DisplayAlert("Error", Name2.Text + ", the word you chose contains an invalid letter", "Okay");
                        return;

                    }
                }
            }

            // Verify word in dictionary

            word = word.ToLower(); // Convert to lowercase to avoid Case-Sensitive issues

            StreamReader s = new StreamReader(Path.Combine(FileSystem.AppDataDirectory, "dictionary.txt")); // Open File
            string line = "";

            while ((line = s.ReadLine()) != null)
            {
                if (word == line) // Word found
                {
                    s.Close();
                    return;
                }
            }

            // If word is not found in the dictionary
            if (name == Name1.Text)
            {
                wordLength1 = 0;
                await DisplayAlert("Error", Name1.Text + ", your word does not exist in the dictionary", "Okay");
                return;
            }

            else if (name == Name2.Text)
            {
                wordLength2 = 0;
                await DisplayAlert("Error", Name2.Text + ", your word does not exist in the dictionary", "Okay");
                return;
            }
        }

        private async void UpdateScore(int length1, int length2)
        {
            // Convert the current scores to integers
            int currentScore1 = Int32.Parse(Score1.Text);
            int currentScore2 = Int32.Parse(Score2.Text);

            if (length1 > length2) // Player 1 has longer word
            {
                Score1.Text = (currentScore1 + length1).ToString(); // Add to player 1's score
                GameStatus.Text = (Name1.Text + " wins the round!");
            }

            if (length2 > length1) // Player 2 has longer word
            {
                Score2.Text = (currentScore2 + length2).ToString(); // Add to player 2's score
                GameStatus.Text = (Name2.Text + " wins the round!");
            }

            if (length1 == length2) // Both players' words are the same length
            {
                Score1.Text = (currentScore1 + length1).ToString();
                Score2.Text = (currentScore2 = length2).ToString();
                GameStatus.Text = ("It's a tie!");
            }
        }

        private async void DownloadDictionary()
        {
            try
            {
                string url = "https://raw.githubusercontent.com/DonH-ITS/jsonfiles/main/cdwords.txt";
                string file = "dictionary.txt";
                string filePath = Path.Combine(FileSystem.AppDataDirectory, file);

                if (!File.Exists(filePath)) // Check if dictionary already exists
                {
                    filePath = await _downloadManager.DownloadAsync(file, url); // Download the file 
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void SaveResults()
        {
            // Make new stats object for finished game
            var gameStat = new GameStats(Name1.Text, Name2.Text, Int32.Parse(Score1.Text), Int32.Parse(Score2.Text), DateTime.Now.ToString("yyyy-MM-dd hh:mm"));

            string filepath = Path.Combine(FileSystem.AppDataDirectory, "gamestats.json");

            List<GameStats> gameStatsList = new List<GameStats>();

            if (File.Exists(filepath))
            {
                // Get exisiting file data
                string existingJson = File.ReadAllText(filepath);
                var existingStats = JsonConvert.DeserializeObject<List<GameStats>>(existingJson); // Deserialize JSON

                if (existingStats != null)
                {
                    gameStatsList = existingStats;
                }
            }

            gameStatsList.Add(gameStat);

            string json = JsonConvert.SerializeObject(gameStatsList, Formatting.Indented); // Reserialize JSON and add to file

            File.WriteAllText(filepath, json);
        }
    }
}