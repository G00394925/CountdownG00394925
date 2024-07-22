using Plugin.Maui.Audio;

namespace Countdown;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();

        RoundsPicker.SelectedItem = Preferences.Get("SelectedRounds", 6);
        TimePicker.SelectedItem = Preferences.Get("SelectedTime", 30);
    }

    private void BackButton_Clicked(object sender, EventArgs e)
    {
        Preferences.Set("SelectedRounds", (int)RoundsPicker.SelectedItem);
        Preferences.Set("SelectedTime", (int)TimePicker.SelectedItem);

		App.Current.MainPage = new NavigationPage(new HomePage());
    }
}