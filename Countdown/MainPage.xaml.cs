namespace Countdown
{
    public partial class MainPage : ContentPage
    {
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

        // Letter Arrays
        public char[] vowels = ['A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'U', 'U', 'U', 'U', 'U', 'U'];
        public char[] consonants = ['B', 'B', 'C', 'C', 'C', 'D', 'D', 'D', 'D', 'D', 'D', 'F', 'F', 'G', 'G', 'G', 'H', 'H', 'J', 'K', 'L', 'L', 'L', 'L', 'L', 'M', 'M', 'M', 'M', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'P', 'P', 'P', 'P', 'Q', 'R', 'R', 'R', 'R', 'R', 'R', 'R', 'R', 'R', 'S', 'S', 'S', 'S', 'S', 'S', 'S', 'S', 'S', 'T', 'T', 'T', 'T', 'T', 'T', 'T', 'T', 'T', 'V', 'W', 'X', 'Y', 'Z'];
        public char[] drawnLetters = new char[9]; // Store the drawn letters

        public MainPage()
        {
            InitializeComponent();
            StartGame();
        }

        private async void StartGame()
        {
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


        private void VButton_Clicked(object sender, EventArgs e)
        {
            if (buttonPressed < drawnLetters.Length) 
            {
                char letter = GetLetter(1); // Aquire letter
                drawnLetters[buttonPressed] = letter;
                buttonPressed++;
                UpdateLetterBox();
                
                // Disable the buttons once there are enough letters
                if (buttonPressed == 9)
                {
                    VButton.IsEnabled = false;
                    CButton.IsEnabled = false;
                }
            }
        }

        private void CButton_Clicked(object sender, EventArgs e)
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

        private void GetPoints()
        {

        }

        private void UpdateTimer()
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
