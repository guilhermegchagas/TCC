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

        private void DataInicio_Clicked(object sender, EventArgs e)
        {
            dataInicioControl.IsOpen = true;
        }

        private void DataFim_Clicked(object sender, EventArgs e)
        {
            dataFimControl.IsOpen = true;
        }

        private void PotenciaTotal_Clicked(object sender, EventArgs e)
        {
            ViewModel.MedicaoSelecionada = "PotenciaTotal";
        }

        private void PotenciaAtiva_Clicked(object sender, EventArgs e)
        {
            ViewModel.MedicaoSelecionada = "PotenciaAtiva";
        }

        private void PotenciaReativa_Clicked(object sender, EventArgs e)
        {
            ViewModel.MedicaoSelecionada = "PotenciaReativa";
        }

        private void FatorPotencia_Clicked(object sender, EventArgs e)
        {
            ViewModel.MedicaoSelecionada = "FatorPotencia";
        }

        private void Corrente_Clicked(object sender, EventArgs e)
        {
            ViewModel.MedicaoSelecionada = "Corrente";
        }

        private void Tensao_Clicked(object sender, EventArgs e)
        {
            ViewModel.MedicaoSelecionada = "Tensao";
        }

        private void Frequencia_Clicked(object sender, EventArgs e)
        {
            ViewModel.MedicaoSelecionada = "Frequencia";
        }
    }
}