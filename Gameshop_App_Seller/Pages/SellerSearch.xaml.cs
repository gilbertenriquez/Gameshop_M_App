using Gameshop_App_Seller.Models;

namespace Gameshop_App_Seller.Pages;

public partial class SellerSearch : ContentPage
{
    private Users Data = new Users();
	public SellerSearch()
	{
		InitializeComponent();
        OnAppearing();

    }

    protected override async void OnAppearing()
    {
        try
        {
            string userKey = App.key;
            Console.WriteLine($"User key: {userKey}");

            var usersInfo = await Data.GetUsersinfoAsync(userKey);

            if (usersInfo != null)
            {
                username.ItemsSource = usersInfo;
                Console.WriteLine($"Users info count: {usersInfo.Count}");
            }
            else
            {
                Console.WriteLine("User information is null. Handle this case if needed.");
                // Handle the case where user information is not available
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions as needed
            Console.WriteLine($"Exception in OnAppearing: {ex.Message}");
        }
    }
}