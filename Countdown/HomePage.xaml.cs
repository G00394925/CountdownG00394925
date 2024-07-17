namespace Countdown;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}

    private void StartBtn_Clicked(object sender, EventArgs e)
    {
		App.Current.MainPage = new NavigationPage(new MainPage());
    }

    private void LoadBtn_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new MainPage());
    }
}