using MobileMarket.Model;
using MobileMarket.ViewController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileMarket.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AvailableCuponsPage : ContentPage
    {
        public AvailableCuponsPage()
        {
            InitializeComponent();
            List<Cupom> cuponsDisponiveis = HTTPRequest.GetAvailableCupons();
            if(cuponsDisponiveis != null)
                carousel.ItemsSource = cuponsDisponiveis;
            if (cuponsDisponiveis.Count <= 10)
                carousel.ShowIndicators = true;
            else
                carousel.ShowIndicators = false;
            carousel.ShowArrows = true;

            lbl_creditos.Text = ClienteInfo.Creditos.ToString("#0.00");
        }

        private async void ResgatarCupomClicked(object sender, EventArgs e)
        {
            Button botao = (Button)sender;
            var answer = await DisplayAlert("Resgatar Cupom", "Deseja resgatar este cupom?", "Não", "Sim");
            if(!answer)
            {
                try
                {
                    if(HTTPRequest.ResgatarCupom(botao.CommandParameter.ToString()))
                    {
                        HTTPRequest.UpdateClientInfo();
                        lbl_creditos.Text = ClienteInfo.Creditos.ToString("#0.00");
                        await DisplayAlert("Cupom Resgatado", "O cupom foi resgatado com sucesso!\nSeu cupom aparecerá na aba \"Meus cupons\".", "OK");
                    }
                    else
                        await DisplayAlert("Erro", "Falha ao resgatar cupom.", "OK");
                }
                catch
                {
                    await DisplayAlert("Erro", "Falha ao resgatar cupom.", "OK");
                }   
            }
        }
    }
}