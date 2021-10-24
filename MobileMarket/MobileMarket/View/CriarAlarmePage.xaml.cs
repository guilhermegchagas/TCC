using MobileMarket.Model;
using MobileMarket.ViewController;
using MobileMarket.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileMarket.View
{
    public partial class CriarAlarmePage : ContentPage
    {
        private ChartPage alarmesPage = null;
        private Alarme alarme = null;
        private bool isUpdatePage = false;
        public CriarAlarmPageViewModel ViewModel
        {
            get { return (CriarAlarmPageViewModel)BindingContext; }
        }

        public CriarAlarmePage(ChartPage page = null, Alarme _alarme = null)
        {
            InitializeComponent();
            alarmesPage = page;
            alarme = _alarme;
            GradiantStyles.SetContentPageGradiant(this);
            if(alarme != null)
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
            botaoFinalizar.Text = "Cadastrar Alarme";
        }

        private void SetUpdatePage()
        {
            isUpdatePage = true;
            botaoFinalizar.Text = "Atualizar Alarme";
            entry_nome.Text = alarme.Nome;
            editor_descricao.Text = alarme.Descricao;
            ViewModel.TipoMedicaoSelecionada = alarme.TipoMedicao;
            ViewModel.TipoCondicaoSelecionada = alarme.TipoCondicao;
            entry_valor.Text = alarme.ValorCondicao.ToString();
        }

        private void FinishRegisterButtonClicked(object sender, EventArgs e)
        {
            if(!IsAllFieldsOK())
                return;
            if(isUpdatePage)
            {
                if (HTTPRequest.PutUpdateAlarme(this, GetAlarmeInfo()))
                {
                    if (alarmesPage != null)
                    {
                        alarmesPage.UpdateListaAlarmes();
                    }
                    Navigation.PopAsync();
                }
            }
            else
            {
                if (HTTPRequest.PostRegisterAlarme(this, GetAlarmeInfo()))
                {
                    if (alarmesPage != null)
                    {
                        alarmesPage.UpdateListaAlarmes();
                    }
                    Navigation.PopAsync();
                }
            }
        }

        private Alarme GetAlarmeInfo()
        {
            if (isUpdatePage)
            {
                Alarme alarme = this.alarme;
                alarme.Nome = entry_nome.Text;
                alarme.Descricao = editor_descricao.Text;
                alarme.TipoMedicao = ViewModel.TipoMedicaoSelecionada;
                alarme.TipoCondicao = ViewModel.TipoCondicaoSelecionada;
                alarme.ValorCondicao = Convert.ToDouble(entry_valor.Text);
                return alarme;
            }
            else
            {
                Alarme alarme = new Alarme();
                alarme.Nome = entry_nome.Text;
                alarme.Descricao = editor_descricao.Text;
                alarme.TipoMedicao = ViewModel.TipoMedicaoSelecionada;
                alarme.TipoCondicao = ViewModel.TipoCondicaoSelecionada;
                alarme.ValorCondicao = Convert.ToDouble(entry_valor.Text);
                alarme.CodigoPonto = Convert.ToInt32(alarmesPage.ViewModel.ponto.Codigo);
                return alarme;
            }
        }

        private bool IsAllFieldsOK()
        {
            if(!AssertNoEmptyEntry())
                return false;
            if (!AssertValorCondicaoDoubleValue())
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
            if (Helper.AssertEmptyPicker(picker_tipo_medicao))
            {
                DisplayAlert("Campo Vazio", "Selecione o tipo de medição.", "OK");
                return false;
            }
            if (Helper.AssertEmptyPicker(picker_tipo_condicao))
            {
                DisplayAlert("Campo Vazio", "Selecione o tipo de condição.", "OK");
                return false;
            }
            if (Helper.AssertEmptyEntry(entry_valor))
            {
                DisplayAlert("Campo Vazio", "Preencha o campo de valor da condição.", "OK");
                return false;
            }
            return true;
        }

        private bool AssertValorCondicaoDoubleValue()
        {
            try
            {
                double value = Convert.ToDouble(entry_valor.Text);
                return true;
            }
            catch
            {
                DisplayAlert("Valor incorreto", "O valor deve conter apenas números.", "OK");
                return false;
            }
        }
    }
}