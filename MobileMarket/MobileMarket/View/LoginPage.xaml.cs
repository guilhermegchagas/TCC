using MobileMarket.Model;
using MobileMarket.ViewController;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileMarket.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            GradiantStyles.SetContentPageGradiant(this);
        }

        private void LoginButtonClicked(object sender, EventArgs e)
        {
            if (!AssertNoEmptyEntry())
                return;

            string email = entry_email.Text.Trim();
            string senha = entry_senha.Text.Trim();

            LoginTokenResult accessToken = HTTPRequest.GetLoginToken(email, senha);
            if (accessToken != null)
            {
                if(accessToken.Error == null)
                {
                    ClienteInfo.Email = email;
                    ClienteInfo.Token = accessToken.AccessToken;
                    if(HTTPRequest.UpdateClientInfo())
                    {
                        App.Current.MainPage = new IndexPage();
                    }
                    else
                    {
                        ClienteInfo.ClearInfo();
                        HTTPRequest.DisplayConnectionError(this);
                    }
                }
                else
                {
                    DisplayAlert(accessToken.Error, accessToken.ErrorDescription, "OK");
                }
            }
            else
            {
                HTTPRequest.DisplayConnectionError(this);
            }
        }

        private void RegisterButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterPage());
        }

        private bool AssertNoEmptyEntry()
        {
            if (Helper.AssertEmptyEntry(entry_email))
            {
                DisplayAlert("Campo Vazio", "Preencha o campo de email.", "OK");
                return false;
            }
            if (Helper.AssertEmptyEntry(entry_senha))
            {
                DisplayAlert("Campo Vazio", "Preencha o campo de senha.", "OK");
                return false;
            }
            return true;
        }
    }
}