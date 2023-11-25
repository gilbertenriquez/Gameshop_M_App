using Gameshop_App_Seller.Pages;
using Firebase.Database;



namespace Gameshop_App_Seller
{
    public partial class App : Application
    {
        public static FirebaseClient ClientUsers = new("https://gameshopdb-f4df3-default-rtdb.asia-southeast1.firebasedatabase.app/");
        public App()
        {
            InitializeComponent();

            MainPage = new WelcomePage();
        }
    }
}