﻿using Android.App;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;
using Microsoft.Maui;


namespace Gameshop_App_Seller
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
       
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            MobileAds.Initialize(this);
        }

        public override void OnBackPressed()
        {
            // Disable the physical back button
            // Remove the line below if you want the physical back button to work
            //base.OnBackPressed();
        }
    }
}