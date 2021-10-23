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
using System.Collections.Generic;
using MobileMarket.ViewController;

namespace MobileMarket.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartPage : Xamarin.Forms.TabbedPage
    {
        public ChartPageViewModel ViewModel
        {
            get { return (ChartPageViewModel)BindingContext; }
        }

        public ChartPage(Ponto ponto)
        {
            InitializeComponent();
            GradiantStyles.SetContentPageGradiant(chartPage);
            GradiantStyles.SetContentPageGradiant(dataPage);
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            ViewModel.ponto = ponto;
            ViewModel.dataGrid = dataGrid;
        }

        private void DataInicio_Clicked(object sender, EventArgs e)
        {
            dataInicioControl.IsOpen = true;
        }

        private void DataFim_Clicked(object sender, EventArgs e)
        {
            dataFimControl.IsOpen = true;
        }

        private void DataInicio2_Clicked(object sender, EventArgs e)
        {
            dataInicioControl2.IsOpen = true;
        }

        private void DataFim2_Clicked(object sender, EventArgs e)
        {
            dataFimControl2.IsOpen = true;
        }

        private void PotenciaTotal_Clicked(object sender, EventArgs e)
        {
            ViewModel.MedicaoSelecionada = "PotenciaTotal";
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

        private void dataInicioControl_OkButtonClicked(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            ViewModel.DataInicioOKClicked();
        }

        private void dataFimControl_OkButtonClicked(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
           ViewModel.DataFimOKClicked();
        }

        private void ExportarDados_Clicked(object sender, EventArgs e)
        {
            ViewModel.ExportarMedicoes();
        }

        #region AlarmPage
        List<Alarme> listaAlarmes = null;

        public void UpdateListaAlarmes()
        {
            try
            {
                listaAlarmes = HTTPRequest.BuscarAlarmesPorPonto(ViewModel.ponto.Codigo);
                listaAlarmesControl.ItemsSource = listaAlarmes;
            }
            catch
            {
                listaAlarmes = null;
            }
            if (listaAlarmes == null)
            {
                StackLayout stack = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.Center
                };
                Image alarmIcon = new Image
                {
                    Source = "alarm_icon.png",
                    HeightRequest = 100,
                    WidthRequest = 100
                };
                Label label = new Label
                {
                    Text = "Você não possui nenhum alarme cadastrado.",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.White,
                    Margin = new Thickness(10, 30, 10, 30)
                };

                stack.Children.Add(alarmIcon);
                stack.Children.Add(label);

                scrollView.Content = stack;
            }
            else
            {
                scrollView.Content = listaAlarmesControl;
            }
        }

        private void ArrowTapped(object sender, EventArgs e)
        {
            StackLayout stackPai = (StackLayout)sender;
            Grid grid = (Grid)stackPai.Children[0];
            StackLayout stack = (StackLayout)stackPai.Children[1];
            Image imagem = (Image)grid.Children[1];
            if (imagem.Source.ToString() == "File: arrowdown.png")
            {
                stack.IsVisible = true;
                imagem.Source = "arrowup.png";

            }
            else
            {
                stack.IsVisible = false;
                imagem.Source = "arrowdown.png";
            }
        }

        private void AlarmEditButtonClicked(object sender, EventArgs e)
        {
            Xamarin.Forms.Button button = sender as Xamarin.Forms.Button;
            Alarme alarme = (Alarme)button.BindingContext;
            Navigation.PushAsync(new CriarAlarmePage(this, alarme));
        }

        private async void AlarmDeleteButtonClicked(object sender, EventArgs e)
        {
            Xamarin.Forms.Button button = sender as Xamarin.Forms.Button;
            Alarme alarme = (Alarme)button.BindingContext;
            bool accepted = await DisplayAlert("Deletar Alarme", "Deseja deletar o alarme?", "SIM", "NÃO");
            if (accepted)
            {
                HTTPRequest.DeleteAlarme(this, alarme);
                UpdateListaAlarmes();
            }
        }

        private void CriarAlarmeButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CriarAlarmePage(this));
        }
        #endregion
    }
}