using Gameshop_App_Seller.Models;
using System.Collections.ObjectModel;
using static Gameshop_App_Seller.App;
using static System.Net.Mime.MediaTypeNames;


namespace Gameshop_App_Seller.Pages;

public partial class HomePage : ContentPage
{
   private Users dusers = new Users();
    public HomePage()
	{
		InitializeComponent();
        OnAppearing();

        
	}
    protected override async void OnAppearing()
    {
        try
        {
            // Obtain the user email from your source; replace this with your actual logic
            string userEmail = App.email;

            // Use the App.FirebaseService.GetUserKeyByEmail method to get the user key
            string userKey = await App.FirebaseService.GetUserKeyByEmail(userEmail);

            if (!string.IsNullOrEmpty(userKey))
            {
                // Retrieve and display the user's products
                var userProducts = await dusers.GetUserProducts(userKey);

                if (userProducts.Any())
                {
                    listViewProducts.ItemsSource = userProducts;
                }
                else
                {
                    await DisplayAlert("Warning", "User has no products.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Warning", "User key not found.", "OK");
            }
        }
        catch (Exception ex)
        {
            // Log or display the exception for debugging
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async void AddProdsBTN_Clicked(object sender, EventArgs e)
           {
		await Navigation.PushModalAsync(new addproductPage());
          }

    private async void EditProdsBTN_Clicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(App.key))
		{
			await Navigation.PushAsync(new EditProductPage());
		}
		else
		{
			await DisplayAlert("Alert", "Please Select A Data To Edit", "OK!");
		}
    }
    private async void ChatBTN_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new ChatHomepage());
    }

    private async void listproducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        App.email = (e.CurrentSelection.FirstOrDefault() as Users)?.MAIL;
        App.key = await dusers.GetUserKey(App.email);
    }

}