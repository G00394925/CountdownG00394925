namespace Countdown
{
    public partial class MainPage : ContentPage
    {

        // Variables
        int pts1;
        int pts2;
        int totalScore1;
        int totalScore2;
        int playerTurn;
        int currentRound;
        int rounds = 6;

        public MainPage()
        {
            InitializeComponent();
            StartGame();
        }

        private void StartGame()
        {
            totalScore1 = 0;
            totalScore2 = 0;
            playerTurn = 1;
            currentRound = 0;
        }

        private void GetLetters()
        {

        }

        private void VButton_Clicked(object sender, EventArgs e)
        {

        }

        private void CButton_Clicked(object sender, EventArgs e)
        {

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
    }
}
