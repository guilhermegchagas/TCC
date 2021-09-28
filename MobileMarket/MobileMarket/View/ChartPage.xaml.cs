using System;
using QRCoder;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileMarket.Model;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Essentials;
using MobileMarket.ViewModel;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace MobileMarket.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartPage : Xamarin.Forms.TabbedPage
    {
        protected ChartPageViewModel ViewModel
        {
            get { return (ChartPageViewModel)BindingContext; }
        }

        public ChartPage(Ponto ponto)
        {
            InitializeComponent();
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            ViewModel.ponto = ponto;
        }
    }
}