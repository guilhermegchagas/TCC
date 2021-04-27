using System;
using QRCoder;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileMarket.Model;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Essentials;

namespace MobileMarket.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRConnectPage : ContentPage
    {
        public QRConnectPage()
        {
            InitializeComponent();
            GerarQRCode(ClienteInfo.ShortToken);
            ReciclagemSocket.reciclagemSocket.ConectarWebSocket();
            ReciclagemSocket.reciclagemSocket.qrPage = this;
        }

        private void GerarQRCode(string text)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text,QRCodeGenerator.ECCLevel.M);
            PngByteQRCode qRCode = new PngByteQRCode(qrCodeData);
            int pixels = Convert.ToInt32(1.6 / 2 / 21 * 0.393700787 * App.dpi);
            byte[] qrCodeBytes = qRCode.GetGraphic(pixels);
            QRCodeImage.Source = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
        }

        public void GoToCarrinhoPage(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushModalAsync(new NavigationPage(new CarrinhoPage()));
            });
        }
    }
}