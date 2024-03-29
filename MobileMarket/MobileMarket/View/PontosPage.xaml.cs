﻿using MobileMarket.Model;
using MobileMarket.ViewController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileMarket.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PontosPage : ContentPage
    {
        List<Ponto> listaPontos = null;

        public PontosPage()
        {
            InitializeComponent();
            GradiantStyles.SetContentPageGradiant(this);
            UpdateLista();
        }

        public void UpdateLista()
        {
            try
            {
                listaPontos = HTTPRequest.GetMyPontos();
                listaPontosControl.ItemsSource = listaPontos;
            }
            catch
            {
                listaPontos = null;
            }
            if (listaPontos == null)
            {
                StackLayout stack = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.Center
                };
                Image medicaoIcon = new Image
                {
                    Source = "ponto_medicao_icon.png",
                    HeightRequest = 100,
                    WidthRequest = 100
                };
                Label label = new Label 
                { 
                    Text = "Você não possui nenhum ponto de medição cadastrado.",
                    HorizontalOptions = LayoutOptions.Center, 
                    VerticalOptions = LayoutOptions.Center, 
                    TextColor = Color.White,
                    Margin = new Thickness (10,30,10,30)
                };

                stack.Children.Add(medicaoIcon);
                stack.Children.Add(label);

                scrollView.Content = stack;
            }
            else
            {
                scrollView.Content = listaPontosControl;
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

        private void ShowDetailButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Ponto ponto = (Ponto)button.BindingContext;
            Navigation.PushAsync(new ChartPage(ponto));
        }

        private void EditButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Ponto ponto = (Ponto)button.BindingContext;
            Navigation.PushAsync(new CriarPontoPage(this,ponto));
        }

        private async void DeleteButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Ponto ponto = (Ponto)button.BindingContext;
            bool accepted = await DisplayAlert("Deletar Ponto", "Ao deletar o ponto você perderá todas as medições, é recomendado exportar os dados primeiro. Desejar prosseguir?", "SIM", "NÃO");
            if (accepted)
            {
                HTTPRequest.DeletePonto(this, ponto);
                UpdateLista();
            }
        }

        private void CriarPontoButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CriarPontoPage(this));
        }
    }
}