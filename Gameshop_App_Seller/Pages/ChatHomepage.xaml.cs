using Gameshop_App_Seller.Models;
using System.Collections.ObjectModel;
using static Gameshop_App_Seller.App;


namespace Gameshop_App_Seller.Pages
{
    public partial class ChatHomepage : ContentPage
    {
        private Users ChatUsers = new Users();
        private string userEmail;
        private string userKey;
        private ObservableCollection<Users> usersList;
        public ChatHomepage()
        {
            InitializeComponent();
            usersList = new ObservableCollection<Users>();
            listViewProducts.ItemsSource = usersList;

        }

        public ChatHomepage(string userKey) : this()
        {
            InitializeComponent();
            InitializeAsync(userKey);
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
                    // Retrieve and display the user's FNAME and LNAME
                    var userProducts = await ChatUsers.GetAllUsers(userKey);

                    if (userProducts != null)
                    {
                        usersList.Clear();
                        foreach (var user in userProducts)
                        {
                            usersList.Add(user);
                        }
                    }
                    else
                    {
                        await DisplayAlert("Warning", "User not found.", "OK");
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

        private async void InitializeAsync(string userKey)
        {
            try
            {
                string userEmail = App.email;

                // Use the App.FirebaseService.GetUserKeyByEmail method to get the user key
                string obtainedUserKey = await App.FirebaseService.GetUserKeyByEmail(userEmail);

                UserKey = obtainedUserKey;
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately (log, display, etc.)
                Console.WriteLine($"Error in App initialization: {ex.Message}");
            }
        }

        //private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        //{
        //    // Use the class-level userKey instead of redeclaring it
        //    if (!string.IsNullOrEmpty(userKey))
        //    {
        //        bool user = await DisplayAlert("Confirmation", "Do you want to continue?", "Yes", "No");
        //        if (user)
        //        {
        //            App.key = userKey;
        //            await Navigation.PushModalAsync(new MessagePage(userKey));
        //        }
        //    }
        //    else
        //    {
        //        await DisplayAlert("Warning", "No user key found", "OK");
        //    }
        //}

        private async void listproducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.email = (e.CurrentSelection.FirstOrDefault() as Users)?.MAIL;
            App.key = await ChatUsers.GetUserKey(App.email);
        }
    }
}