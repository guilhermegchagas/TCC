using MobileMarket.Model;
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
                Label label = new Label { Text = "Você não possui nenhum ponto de medição cadastrado.", BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, TextColor = Color.Black };
                Content = label;
            }
        }

        private void ArrowTapped(object sender, EventArgs e)
        {
            StackLayout stackPai = (StackLayout)sender;
            StackLayout stack1 = (StackLayout)stackPai.Children[1];
            StackLayout stack2 = (StackLayout)stackPai.Children[2];
            Image imagem = (Image)stack1.Children[1];
            if (imagem.Source.ToString() == "File: arrowdown.png")
            {
                stack2.IsVisible = true;
                imagem.Source = "arrowup.png";
            }
            else
            {
                stack2.IsVisible = false;
                imagem.Source = "arrowdown.png";
            }       
        }

        private void ShowDetailButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Ponto ponto = (Ponto)button.BindingContext;
        }

        private void CriarPontoButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CriarPontoPage(this));
        }
    }
}