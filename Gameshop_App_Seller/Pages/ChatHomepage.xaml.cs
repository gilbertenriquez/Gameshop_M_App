namespace Gameshop_App_Seller.Pages;

public partial class ChatHomepage : ContentPage
{
	public ChatHomepage()
	{
		InitializeComponent();

		NavigationPage.SetHasNavigationBar(this, false);
			
	}

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		await Navigation.PushModalAsync(new MessagePage());
    }
}