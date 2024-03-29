﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileMarket.View;
using System.Globalization;

namespace MobileMarket
{
    public partial class App : Application
    {
        public static int screenHeightUnits, screenWidthUnits, screenHeightPixels, screenWidthPixels;
        public static double dpi;

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTI5Mjg5QDMxMzkyZTMzMmUzME9zb0ZwdytGUnY5UGRSZTV1SGJ1bXUzZ2tOazZwbTNTbTFQK0NaQkdWQTQ9");
            InitializeComponent();
            CultureInfo BRCulture = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentCulture = BRCulture;
            MainPage = new NavigationPage(new LoginPage());
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
