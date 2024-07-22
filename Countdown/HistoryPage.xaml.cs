using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Plugin.Maui.Audio;

namespace Countdown;

public partial class HistoryPage : ContentPage
{
	private readonly IAudioManager _audioManager;
	public ObservableCollection<GameStats> GameStats { get; set; } = new ObservableCollection<GameStats>();

	public HistoryPage(IAudioManager audioManager)
	{
		this._audioManager = audioManager;
		InitializeComponent();
		BindingContext = this;
		LoadStats();
	}

	private void LoadStats()
	{
		string filePath = Path.Combine(FileSystem.AppDataDirectory, "gamestats.json");

		if (File.Exists(filePath))
		{
			string json = File.ReadAllText(filePath);
			var gameStatsList = JsonConvert.DeserializeObject<List<GameStats>>(json);

			if (gameStatsList != null)
			{
				foreach(var gameStats in gameStatsList) 
				{
					GameStats.Add(gameStats);
				}
			}
		}
	}

    private void BackButton_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new HomePage(_audioManager));
    }
}