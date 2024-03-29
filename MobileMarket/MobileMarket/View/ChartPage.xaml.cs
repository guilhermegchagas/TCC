﻿using System;
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
using System.Threading.Tasks;

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
            GradiantStyles.SetContentPageGradiant(statsPage);
            GradiantStyles.SetContentPageGradiant(alarmPage);
            GradiantStyles.SetContentPageGradiant(notificationPage);
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            ViewModel.Ponto = ponto;
            ViewModel.dataGrid = dataGrid;
            UpdateListaAlarmes();
            UpdateListaNotificacao();
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                Task.Run(() =>
                {
                    UpdateListaNotificacao();
                });
                return true;
            });
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

        private void DataInicio3_Clicked(object sender, EventArgs e)
        {
            dataInicioControl3.IsOpen = true;
        }

        private void DataFim3_Clicked(object sender, EventArgs e)
        {
            dataFimControl3.IsOpen = true;
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
            string result = ViewModel.ExportarMedicoes();
            if (!string.IsNullOrEmpty(result))
            {
                DisplayAlert("Dados exportados", "Os dados foram exportados com sucesso para: " + result, "OK");
            }
            else
            {
                DisplayAlert("Falha ao exportar dados", "Falha ao exportar os dados.", "OK");
            }
        }

        #region AlarmPage
        List<Alarme> listaAlarmes = null;

        public void UpdateListaAlarmes()
        {
            try
            {
                listaAlarmes = HTTPRequest.BuscarAlarmesPorPonto(ViewModel.Ponto.Codigo);
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

                scrollViewAlarm.Content = stack;
            }
            else
            {
                scrollViewAlarm.Content = listaAlarmesControl;
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

        #region NotificationPage
        List<Notificacao> listaNotificacao = null;

        public void UpdateListaNotificacao()
        {
            try
            {
                listaNotificacao = HTTPRequest.BuscarNotificacaoPorPonto(ViewModel.Ponto.Codigo);
                Device.BeginInvokeOnMainThread(() => {
                    listaNotificacaoControl.ItemsSource = listaNotificacao;
                });
            }
            catch
            {
                listaNotificacao = null;
            }
            if (listaNotificacao == null)
            {
                StackLayout stack = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.Center
                };
                Image notificationIcon = new Image
                {
                    Source = "notification_icon.png",
                    HeightRequest = 100,
                    WidthRequest = 100
                };
                Label label = new Label
                {
                    Text = "Você não possui nenhuma notificação.",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.White,
                    Margin = new Thickness(10, 30, 10, 30)
                };

                stack.Children.Add(notificationIcon);
                stack.Children.Add(label);

                Device.BeginInvokeOnMainThread(() => {
                    scrollViewNotification.Content = stack;
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() => {
                    scrollViewNotification.Content = listaNotificacaoControl;
                });
            }
        }

        private void NotificacaoDeleteButtonClicked(object sender, EventArgs e)
        {
            Xamarin.Forms.ImageButton button = sender as Xamarin.Forms.ImageButton;
            Notificacao notificacao = (Notificacao)button.BindingContext;
            HTTPRequest.DeleteNotificacao(notificacao);
            UpdateListaNotificacao();
        }

        private void LimparNotificacoesButtonClicked(object sender, EventArgs e)
        {
            HTTPRequest.LimparNotificacao(ViewModel.Ponto.Codigo);
            UpdateListaNotificacao();
        }
        #endregion

        private void PrecoKWH_Completed(object sender, EventArgs e)
        {
            if(ViewModel.PrecoKWH != ViewModel.Ponto.PrecoKWH)
            {
                ViewModel.Ponto.PrecoKWH = ViewModel.PrecoKWH;
                HTTPRequest.PutUpdateKWH(ViewModel.Ponto);
            }
        }

        private void CompressButton_Clicked(object sender, EventArgs e)
        {
            ViewModel.UsarMedicaoComprimida = true;
        }

        private void ExpandButton_Clicked(object sender, EventArgs e)
        {
            ViewModel.UsarMedicaoComprimida = false;
        }
    }
}