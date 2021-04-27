using System;
using QRCoder;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileMarket.Model;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Essentials;
using MobileMarket.ViewModel;

namespace MobileMarket.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartPage : ContentPage
    {
        protected ChartPageViewModel ViewModel
        {
            get { return (ChartPageViewModel)BindingContext; }
        }

        public ChartPage()
        {
            InitializeComponent();
        }
    }
}