using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileMarket.Model;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileMarket.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IndexPageDetail : ContentPage
    {
        public IndexPageDetail()
        {
            InitializeComponent();
            GradiantStyles.SetContentPageGradiant(this);
            lbl_bemvindo.Text = "Bem vindo, " + ClienteInfo.Nome;
        }

        private void GoToIniciarColeta(object sender, EventArgs e)
        {
            Navigation.PushAsync(new QRConnectPage());
        }

        private void GoToResgatarCupons(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AvailableCuponsPage());
        }

        private void GoToMeusCupons(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MyCuponsPage());
        }
    }
}