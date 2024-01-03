using Gameshop_App_Seller.Models;
using static Gameshop_App_Seller.App;

namespace Gameshop_App_Seller.Pages;

public partial class SellerSettings : ContentPage
{
    private Users Info = new Users();
    private string userId;
	public SellerSettings()
	{
		InitializeComponent();
	}

    public SellerSettings(string userId)
    {
        InitializeComponent();
        this.userId = userId; // Store the user ID
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();

        var userData = App.key;

        var userAccountPath = $"Account/{userData}";

        // Retrieve the user data from the database
        var userSnapshot = await ClientUsers
            .Child(userAccountPath)
            .OnceSingleAsync<Users>();


        FirstNameUser.Text = userSnapshot.FNAME;
        LastNameUser.Text = userSnapshot.LNAME;
    }

    private async void ProfileDetailBTN_Tapped(object sender, TappedEventArgs e)
    {
        progressLoading.IsVisible = true;
        await Navigation.PushModalAsync(new ProfileDetail(App.key));
        progressLoading.IsVisible = false;
    }
    //private async void profileBTN_Clicked(object sender, EventArgs e)
    //{
    //    await Navigation.PushModalAsync(new ProfileDetail(App.key));
    //}
}