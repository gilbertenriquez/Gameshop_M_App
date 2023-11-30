namespace Gameshop_App_Seller
{
    public partial class AppShell : Shell
    {

        public string UserKey { get; set; }
        public AppShell()
        {
            InitializeComponent();
        }

        public AppShell(string userKey) : this()
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