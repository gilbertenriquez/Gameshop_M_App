using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui;


namespace Gameshop_App_Seller
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Subscribe to connectivity changes
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        public override void OnBackPressed()
        {
            // Disable the physical back button
            // Remove the line below if you want the physical back button to work
             //base.OnBackPressed();
        }
        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                ShowInternetConnectionAlert("Internet Connection Established", "You are now connected to the internet.");
            }
            else
            {
                ShowInternetConnectionAlert("No Internet Connection", "Please check your internet connection.");
            }
        }

        private void ShowInternetConnectionAlert(string title, string message)
        {
            // Replace this with your actual alert implementation (snackbar, dialog, etc.)
            // For example, using AndroidX AppCompat library:
            Android.Widget.Toast.MakeText(this, $"{title}: {message}", Android.Widget.ToastLength.Long).Show();
        }
    }
}