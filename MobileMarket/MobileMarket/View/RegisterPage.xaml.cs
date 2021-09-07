using MobileMarket.Model;
using MobileMarket.ViewController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileMarket.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            GradiantStyles.SetContentPageGradiant(this);
        }

        private void FinishRegisterButtonClicked(object sender, EventArgs e)
        {
            if(!IsAllFieldsOK())
                return;
            FillClienteInfo();
            if(HTTPRequest.PostRegisterClient(this))
            {
                Navigation.PopToRootAsync();
            }
        }

        private void FillClienteInfo()
        {
            ClienteInfo.Nome = entry_nome.Text.Trim();
            ClienteInfo.Email = entry_email.Text.Trim();
            ClienteInfo.Senha = entry_senha.Text.Trim();
        }

        private bool IsAllFieldsOK()
        {
            if(!AssertNoEmptyEntry())
                return false;
            if (!AssertPasswordMatch())
                return false;
            return true;
        }

        private bool AssertPasswordMatch()
        {
            if(entry_senha.Text == entry_confirmar_senha.Text)
            {
                return true;
            }
            else
            {
                DisplayAlert("Confirmação Inválida", "Verifique se o campo de senha e de confirmação estão corretos.", "OK");
                return false;
            }
        }

        private bool AssertNoEmptyEntry()
        {
            if (Helper.AssertEmptyEntry(entry_nome))
            {
                DisplayAlert("Campo Vazio", "Preencha o campo de nome.", "OK");
                return false;
            }
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
            if (Helper.AssertEmptyEntry(entry_confirmar_senha))
            {
                DisplayAlert("Campo Vazio", "Preencha o campo de confirmação de senha.", "OK");
                return false;
            }
            return true;
        }
    }
}