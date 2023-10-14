using Gameshop_App_Seller.Pages;
using Firebase.Database;



namespace Gameshop_App_Seller
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}