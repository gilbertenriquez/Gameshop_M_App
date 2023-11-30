namespace Gameshop_App_Seller
{
    public partial class MainPage : TabbedPage
    {

        private string UserKey;
        public MainPage()
        {
            InitializeComponent();
        }


        public MainPage(string userKey) : this()
        {
            InitializeAsync(userKey);
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
                Console.WriteLine($"Error in AppShell initialization: {ex.Message}");
            }
        }

    }
}