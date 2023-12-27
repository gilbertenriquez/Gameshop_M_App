﻿using SkiaSharp.Extended.UI;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using CommunityToolkit.Maui;
using Plugin.LocalNotification;

namespace Gameshop_App_Seller
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseSkiaSharp()
                .UseMauiCommunityToolkit()
                .UseMauiApp<App>()
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Metropolis-Black.otf", "Metropolis Black");
                    fonts.AddFont("Metropolis-Light.otf", "Metropolis Light");
                    fonts.AddFont("Metropolis-Medium.otf", "Metropolis Medium");
                    fonts.AddFont("Metropolis-Regular.otf", "Metropolis Regular");
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}