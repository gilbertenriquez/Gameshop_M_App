using Gameshop_App_Seller.Models;
using static Gameshop_App_Seller.App;

namespace Gameshop_App_Seller.Pages;

public partial class SellerSearch : ContentPage
{
    private Users Data = new Users();
    private string userId;
	public SellerSearch()
	{
		InitializeComponent();
        OnAppearing();

    }


    public SellerSearch(string userId)
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
  
    
}