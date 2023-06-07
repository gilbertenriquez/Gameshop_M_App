using CommunityToolkit.Maui.Core;
using Font = Microsoft.Maui.Font;
using CommunityToolkit.Maui.Alerts;
using System.Threading;
using static Gameshop_M_App.App;

namespace Gameshop_M_App.Pages;

public partial class AddProductPage : ContentPage
{

    CancellationTokenSource cancellationTokenSource = new();
    public AddProductPage()
	{
		InitializeComponent();
	}
    private async void addimageBTN_Clicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Select main image",
            FileTypes = FilePickerFileType.Images
        });
        if (result == null) return;

        FileInfo fi = new(result.FullPath);
        var size = fi.Length;

        if (size > 524288)
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromRgb(32, 32, 40),
                TextColor = Colors.WhiteSmoke,
                ActionButtonTextColor = Colors.White,
                CornerRadius = new CornerRadius(10),
                Font = Font.SystemFontOfSize(10),
                ActionButtonFont = Font.SystemFontOfSize(10)
            };
            const string text = "The image you have selected is more than 0.50MB please ensure that the size of the image is less than the maximum size. Thank you!";
            const string actionButtonText = "Got it!";
            var duration = TimeSpan.FromSeconds(10);
            var snackbar = Snackbar.Make(text, null, actionButtonText, duration, snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
            return;
        }
        var stream = await result.OpenReadAsync();
        App._mainimgResult = result;
        mainimage.Source = ImageSource.FromStream(() => stream);


        //progressLoading.IsVisible = false;

    }

    private void saveproductBTN_Clicked(object sender, EventArgs e)
    {

    }

    private async void backBTN_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}