using Firebase.Database;
using Gameshop_M_App.Handlers;
using Gameshop_M_App.Pages;

namespace Gameshop_M_App;

public partial class App : Application
{
    public static FirebaseClient players = new("https://gameshopdb-default-rtdb.asia-southeast1.firebasedatabase.app/");
    public App()
	{
		InitializeComponent();      

        MainPage = new WelcomePage();
	}
}
