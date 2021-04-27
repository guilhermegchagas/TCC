using CHD;
using MobileMarket.Model;
using MobileMarket.ViewController;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileMarket.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarrinhoPage : ContentPage
    {
        public CarrinhoPage()
        {
            InitializeComponent();
            ReciclagemSocket.reciclagemSocket.carrinhoPage = this;
            GradiantStyles.SetContentPageGradiant(this);
            lbl_total.Text = "0";
        }

        public void AtualizarCarrinho()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ListCarrinhoReciclagem.ItemsSource = null;
                ListCarrinhoReciclagem.ItemsSource = ReciclagemSocket.reciclagemSocket.CarrinhoReciclagem.GetItems();
                lbl_total.Text = ReciclagemSocket.reciclagemSocket.CarrinhoReciclagem.GetItems().Sum(item => item.total).ToString();
            });
        }

        private void FinalizarColeta(object sender, EventArgs e)
        {
            ReciclagemSocket.reciclagemSocket.FinalizarColeta();
        }

        public void FinalizarOK()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DisplayAlert("Sucesso", "A coleta foi finalizada e os créditos foram adicionados à sua conta.", "OK");
                HTTPRequest.UpdateClientInfo();
                GoToMainPage();
            });
        }

        public void FinalizarFailed()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DisplayAlert("Erro", "Falha ao finalizar coleta.", "OK");
            });
        }

        private void GoToMainPage()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PopModalAsync();
            });
        }

        #region BlockBackButton
        protected override void OnAppearing()
        {
            base.OnAppearing();
            bool OnBackButtonPressed()
            {
                return false;
            }
            HWBackButtonManager.OnBackButtonPressedDelegate onBackButtonPressedDelegate = new HWBackButtonManager.OnBackButtonPressedDelegate(OnBackButtonPressed);
            HWBackButtonManager.Instance.SetHWBackButtonListener(onBackButtonPressedDelegate);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            HWBackButtonManager.Instance.RemoveHWBackButtonListener();
        }
        #endregion
    }
}