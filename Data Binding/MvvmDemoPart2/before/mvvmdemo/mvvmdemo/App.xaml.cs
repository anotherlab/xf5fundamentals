﻿using mvvmdemo.Models;
using mvvmdemo.Services;
using mvvmdemo.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mvvmdemo
{
    public partial class App : Application
    {
        //public User user = new User();
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
