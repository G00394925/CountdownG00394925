using Plugin.Maui.Audio;

namespace Countdown
{
    public partial class MainPage : ContentPage
    {
        // Initialise the audio manager to be able to play the countdown theme
        private readonly IAudioManager audioManager;

        IDispatcherTimer timer;

        // Initialise random number generator
        Random r = new Random();

        // Variables
        public int buttonPressed = 0;
        public int pts1;
        public int pts2;
        public int totalScore1;
        public int totalScore2;
        public int playerTurn;
        public int currentRound;
        public int rounds = 6;
        public int timeRemaining = 30;

        // Letter Arrays
        public char[] vowels = ['A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'U', 'U', 'U', 'U', 'U', 'U'];
        public char[] consonants = ['B', 'B', 'C', 'C', 'C', 'D', 'D', 'D', 'D', 'D', 'D', 'F', 'F', 'G', 'G', 'G', 'H', 'H', 'J', 'K', 'L', 'L', 'L', 'L', 'L', 'M', 'M', 'M', 'M', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'P', 'P', 'P', 'P', 'Q', 'R', 'R', 'R', 'R', 'R', 'R', 'R', 'R', 'R', 'S', 'S', 'S', 'S', 'S', 'S', 'S', 'S', 'S', 'T', 'T', 'T', 'T', 'T', 'T', 'T', 'T', 'T', 'V', 'W', 'X', 'Y', 'Z'];
        public char[] drawnLetters = new char[9]; // Store the drawn letters

        public MainPage(IAudioManager audioManager) // Audiomanager parameters were added to get and set audioManager object
        {
            InitializeComponent();
            this.audioManager = audioManager;
            StartGame();
        }

        private async void StartGame()
        {
            timer = Dispatcher.CreateTimer(); // Create the timer when game starts
            playerTurn = r.Next(1, 3); // Decides which player to go first
            totalScore1 = 0;
            totalScore2 = 0;
            currentRound = 1;

            // Periodically update the status message to reflect on what is currently happening in the game
            GameStatus.Text = "Game Starting!";

            // Wait 1 second to pace the game
            await Task.Delay(1000);

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

                    GameStatus.Text = "Letters chosen..."; // Update game status
                    await Task.Delay(3000);
                    StartRound();
                }
            }
        }
        private char GetLetter(int num)
        {
            char currentLetter = ' ';

            // Random Vowel
            if (num == 1)
            {
                int v = r.Next(67);
                currentLetter = vowels[v];
            }

            // Random Consonant
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

        }
        private async void PlayAudio()
        {
            var player = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("clock.mp3"));

            player.Play();
            await Task.Delay(32000); // Wait 32 seconds to prevent premature Dispose call
            player.Dispose(); // Dispose when not needed to prevent memory leak
        }

        private void StartTimer()
        {
            timeRemaining = 30; // Reset the time
            timer.Interval = TimeSpan.FromMilliseconds(1000); // Counts down for every 1 second
            timer.Tick += (s, e) =>
            {
                Timer.Text = (--timeRemaining).ToString(); // Update the timer after every second

                if(timeRemaining == 0)
                {
                    timer.Stop(); // Stop timer once time reaches 0
                }
            };

            timer.Start();
        }

        private void NextRound()
        {
            ++currentRound;
        }

        private void GetPoints()
        {

        }

        private void SaveGame()
        {

        }

        private void LoadGame()
        {
        
        }
        private void AnswerBox1_Completed(object sender, EventArgs e)
        {

        }

        private void AnswerBox2_Completed(object sender, EventArgs e)
        {

        }

    }
}
