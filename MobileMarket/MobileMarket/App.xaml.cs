using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileMarket.View;

namespace MobileMarket
{
    public partial class App : Application
    {
        public static int screenHeightUnits, screenWidthUnits, screenHeightPixels, screenWidthPixels;
        public static double dpi;

        public App()
        {
            InitializeComponent();

            //MainPage = new NavigationPage(new LoginPage());
            MainPage = new ChartPage();
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
