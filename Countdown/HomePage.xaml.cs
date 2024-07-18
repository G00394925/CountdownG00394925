using Plugin.Maui.Audio;

namespace Countdown;

public partial class HomePage : ContentPage
{
    private readonly IAudioManager audioManager;

    public HomePage(IAudioManager audiomanager)
	{
        this.audioManager = audiomanager;
		InitializeComponent();
	}

    private void StartBtn_Clicked(object sender, EventArgs e)
    {
		App.Current.MainPage = new NavigationPage(new MainPage(audioManager));
    }

    private void LoadBtn_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new MainPage(audioManager));
    }
}