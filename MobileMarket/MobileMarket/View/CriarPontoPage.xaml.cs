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
    public partial class CriarPontoPage : ContentPage
    {
        private PontosPage pontosPage = null;

        public CriarPontoPage(PontosPage page = null)
        {
            InitializeComponent();
            pontosPage = page;
            GradiantStyles.SetContentPageGradiant(this);
        }

        private void FinishRegisterButtonClicked(object sender, EventArgs e)
        {
            if(!IsAllFieldsOK())
                return;
            if(HTTPRequest.PostRegisterPonto(this,GetPontoInfo()))
            {
                if(pontosPage != null)
                {
                    pontosPage.UpdateLista();
                }
                Navigation.PopAsync();
            }
        }

        private Ponto GetPontoInfo()
        {
            Ponto ponto = new Ponto();
            ponto.Nome = entry_nome.Text;
            ponto.Descricao = editor_descricao.Text;
            ponto.CodigoUsuario = Convert.ToInt32(ClienteInfo.ID);
            return ponto;
        }

        private bool IsAllFieldsOK()
        {
            if(!AssertNoEmptyEntry())
                return false;
            return true;
        }

        private bool AssertNoEmptyEntry()
        {
            if (Helper.AssertEmptyEntry(entry_nome))
            {
                DisplayAlert("Campo Vazio", "Preencha o campo de nome.", "OK");
                return false;
            }
            return true;
        }
    }
}