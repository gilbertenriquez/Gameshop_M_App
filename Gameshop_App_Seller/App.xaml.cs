using Gameshop_App_Seller.Pages;
using Firebase.Database;
using Gameshop_App_Seller.Models;
using Firebase.Storage;

namespace Gameshop_App_Seller
{
    public partial class App : Application
    {
        public static string UserKey { get; set; }
        public static FirebaseClient ClientUsers = new("https://gameshopdb-f4df3-default-rtdb.asia-southeast1.firebasedatabase.app/");
        public static FirebaseStorage firebaseStorage = new("gameshopdb-f4df3.appspot.com");
        public static string key, email, imgLink, img1, img2, img3, img4, img5, img6, productname, productdesc, productprice, productquantity;
        public static FileResult _mainimgResult, _img1Result, _img2Result, _img3Result,
             _img4Result, _img5Result, _img6Result, _ValidIDFront, _ValidIDBack, _ValidIDBackSelfie, _ValidIDFrontSelfie;
        public static FirebaseService FirebaseService { get; set; } = new FirebaseService();

        public App()
        {
            InitializeComponent();

            MainPage = new WelcomePage();
        }

        public App(string userKey) : this()
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
                Console.WriteLine($"Error in App initialization: {ex.Message}");
            }
        }
    }
}