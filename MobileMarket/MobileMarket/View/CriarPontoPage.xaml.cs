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
        private Ponto ponto = null;
        private bool isUpdatePage = false;

        public CriarPontoPage(PontosPage page = null, Ponto _ponto = null)
        {
            InitializeComponent();
            pontosPage = page;
            ponto = _ponto;
            GradiantStyles.SetContentPageGradiant(this);
            if(ponto != null)
            {
                SetUpdatePage();
            }
            else
            {
                SetCreatePage();
            }
        }

        private void SetCreatePage()
        {
            isUpdatePage = false;
            botaoFinalizar.Text = "Cadastrar Ponto";
        }

        private void SetUpdatePage()
        {
            isUpdatePage = true;
            botaoFinalizar.Text = "Atualizar Ponto";
            entry_nome.Text = ponto.Nome;
            editor_descricao.Text = ponto.Descricao;
        }

        private void FinishRegisterButtonClicked(object sender, EventArgs e)
        {
            if(!IsAllFieldsOK())
                return;
            if(isUpdatePage)
            {
                if (HTTPRequest.PutUpdatePonto(this, GetPontoInfo()))
                {
                    if (pontosPage != null)
                    {
                        pontosPage.UpdateLista();
                    }
                    Navigation.PopAsync();
                }
            }
            else
            {
                if (HTTPRequest.PostRegisterPonto(this, GetPontoInfo()))
                {
                    if (pontosPage != null)
                    {
                        pontosPage.UpdateLista();
                    }
                    Navigation.PopAsync();
                }
            }
        }

        private Ponto GetPontoInfo()
        {
            if (isUpdatePage)
            {
                Ponto ponto = this.ponto;
                ponto.Nome = entry_nome.Text;
                ponto.Descricao = editor_descricao.Text;
                return ponto;
            }
            else
            {
                Ponto ponto = new Ponto();
                ponto.Nome = entry_nome.Text;
                ponto.Descricao = editor_descricao.Text;
                ponto.CodigoUsuario = Convert.ToInt32(ClienteInfo.ID);
                return ponto;
            }
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