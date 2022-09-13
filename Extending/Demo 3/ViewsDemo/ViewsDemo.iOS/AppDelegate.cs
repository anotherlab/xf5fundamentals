﻿using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Microsoft.Extensions.DependencyInjection;
using UIKit;
using UserNotifications;
using ViewsDemo.Interfaces;
using Xamarin.Forms;

namespace ViewsDemo.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsMaps.Init();

            // register a new user notification center delegate to process the notifications
            UNUserNotificationCenter.Current.Delegate = new iOSNotificationReceiver();

            Startup.Init(ConfigureServices);

            LoadApplication(new App());

            //DependencyService.Register<IToastMessage, ToastMessage>();

            return base.FinishedLaunching(app, options);
        }

        void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IToastMessage, ToastMessage>();
        }
    }
}