using MobileMarket.Model;
using MobileMarket.ViewController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileMarket.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyCuponsPage : ContentPage
    {
        public MyCuponsPage()
        {
            InitializeComponent();
            List<CupomResgatado> listaCupons = HTTPRequest.GetMyCupons();
            if (listaCupons != null)
            {
                listaMeusCupons.ItemsSource = listaCupons;
            }
            else
            {
                Label label = new Label { Text = "Você não possui nenhum cupom ainda.", BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, TextColor = Color.Black };
                Content = label;
            }
        }

        private void ArrowTapped(object sender, EventArgs e)
        {
            StackLayout stackPai = (StackLayout)sender;
            StackLayout stack1 = (StackLayout)stackPai.Children[2];
            StackLayout stack2 = (StackLayout)stackPai.Children[3];
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
    }
}