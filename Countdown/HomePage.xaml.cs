using Plugin.Maui.Audio;
using Microsoft.Extensions.DependencyInjection;

namespace Countdown;

public partial class HomePage : ContentPage
{

    public HomePage()
	{
		InitializeComponent();
	}


    private void StartBtn_Clicked(object sender, EventArgs e)
    {
        var audioManager = new AudioManager();
		App.Current.MainPage = new NavigationPage(new MainPage(audioManager));
    }

    private void HistoryBtn_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new HistoryPage());
    }

    private void SettingsBtn_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new SettingsPage());
    }
}