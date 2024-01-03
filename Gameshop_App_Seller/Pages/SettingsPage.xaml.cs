namespace Gameshop_App_Seller.Pages;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
	}

    private async void LogoutBTN_Tapped(object sender, TappedEventArgs e)
    {
        bool answer = await DisplayAlert("Logout", "Are you sure you want to log out?", "Yes", "No");

        if (answer)
        {
            progressLoading.IsVisible = true;

            try
            {
                App.email = null;
                App.key = null;
                App.UserKey = null;
                


                // Simulate a delay for demonstration purposes
                await Task.Delay(3000);

                // Navigate to the login page or the initial page of your app 
                await Navigation.PushModalAsync(new LoginPage());
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the logout process
                Console.WriteLine($"Error during logout: {ex.Message}");
            }
            finally
            {
                progressLoading.IsVisible = false;
            }
        }
    }

    private void BTNshopDetails_Tapped(object sender, TappedEventArgs e)
    {

    }
}