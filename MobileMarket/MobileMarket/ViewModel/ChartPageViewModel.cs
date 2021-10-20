using MobileMarket.Model;
using MobileMarket.ViewController;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileMarket.ViewModel
{
    public class ChartPageViewModel : BaseViewModel
    {
        private bool AutoSetOk = false;
		public Ponto ponto { get; set; }

        private ObservableCollection<Medicao> _medicoes = null;
        public ObservableCollection<Medicao> Medicoes
        {
            get { return _medicoes; }
            set
            {
                _medicoes = value;
                OnPropertyChanged(nameof(Medicoes));
            }
        }

        private string _medicaoSelecionada = "PotenciaTotal";
        public string MedicaoSelecionada
        {
            get { return _medicaoSelecionada; }
            set
            {
                _medicaoSelecionada = value;
                OnPropertyChanged(nameof(MedicaoSelecionada));
                MedicaoSelecionadaLabel = MedicaoTexto[MedicaoSelecionada];
            }
        }

        private string _medicaoSelecionadaLabel = "Potência Total";
        public string MedicaoSelecionadaLabel
        {
            get { return _medicaoSelecionadaLabel; }
            set
            {
                _medicaoSelecionadaLabel = value;
                OnPropertyChanged(nameof(MedicaoSelecionadaLabel));
            }
        }

        public Dictionary<string, string> MedicaoTexto = new Dictionary<string, string>()
        {
            {"PotenciaTotal", "Potência Total (W)"},
            {"PotenciaAtiva", "Potência Ativa (VA)"},
            {"PotenciaReativa", "Potência Reativa (VAR)"},
            {"FatorPotencia", "Fator de Potência"},
            {"Corrente", "Corrente (A)"},
            {"Tensao", "Tensão (V)"},
            {"Frequencia", "Frequência (Hz)"}
        };

        #region Date Control
        private ObservableCollection<object> _dataInicioCollection;
        public ObservableCollection<object> DataInicioCollection
        {
            get { return _dataInicioCollection; }
            set 
            {
                _dataInicioCollection = value;
                OnPropertyChanged(nameof(DataInicioCollection));
            }
        }

        public void DataInicioOKClicked()
        {
            DataInicio = CollectionToDateTime(DataInicioCollection);
        }

        private DateTime _dataInicio;
        public DateTime DataInicio
        {
            get { return _dataInicio; }
            set
            {
                _dataInicio = value;
                OnPropertyChanged(nameof(DataInicio));
                if(AutoSetOk)
                    UpdateMedicoes();
            }
        }

        private ObservableCollection<object> _dataFimCollection;
        public ObservableCollection<object> DataFimCollection
        {
            get { return _dataFimCollection; }
            set
            {
                _dataFimCollection = value;
                OnPropertyChanged(nameof(DataFimCollection));
            }
        }

        public void DataFimOKClicked()
        {
            DataFim = CollectionToDateTime(DataFimCollection);
        }

        private DateTime _dataFim;
        public DateTime DataFim
        {
            get { return _dataFim; }
            set
            {
                _dataFim = value;
                OnPropertyChanged(nameof(DataFim));
                if (AutoSetOk)
                    UpdateMedicoes();
            }
        }

        private ObservableCollection<object> DateTimeToCollection(DateTime date)
        {
            ObservableCollection<object> collection = new ObservableCollection<object>();

            collection.Add(date.Year.ToString());

            collection.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month).Substring(0, 3));

            if (date.Day < 10)
                collection.Add("0" + date.Day.ToString());
            else
                collection.Add(date.Day.ToString());

            if (date.Hour < 10)
                collection.Add("0" + date.Hour.ToString());
            else
                collection.Add(date.Hour.ToString());

            if (date.Minute < 10)
                collection.Add("0" + date.Minute.ToString());
            else
                collection.Add(date.Minute.ToString());

            return collection;
        }

        private DateTime CollectionToDateTime(ObservableCollection<object> collection)
        {
            int year = Convert.ToInt32(collection[0]);
            int month = 1;
            for (int i = 1; i <= 12; i++)
            {
                string currentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i).Substring(0, 3);
                if (collection[1].ToString() == currentMonth)
                    month = i;
            }
            int day = Convert.ToInt32(collection[2]);
            int hour = Convert.ToInt32(collection[3]);
            int minute = Convert.ToInt32(collection[4]);

            DateTime dateTime = new DateTime(
                year,
                month,
                day,
                hour,
                minute,
                0
            );

            return dateTime;
        }
        #endregion

        public ChartPageViewModel()
		{
            DataInicioCollection = DateTimeToCollection(DateTime.Now.AddDays(-1));
            DataFimCollection = DateTimeToCollection(DateTime.Now);
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                AutoSet();
                AutoSetOk = true;
                return false;
            });
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                Task.Run(() =>
                {
                    UpdateMedicoes();
                });
                return true;
            });
        }

        private void UpdateMedicoes()
        {
            Medicoes = HTTPRequest.GetMedicoes(ponto.Codigo, DataInicio, DataFim);
        }

        public void AutoSet()
        {
            Medicoes = HTTPRequest.GetMedicoes(ponto.Codigo, null, null);
            DataInicioCollection = DateTimeToCollection(Medicoes[3].Horario);
            DataFimCollection = DateTimeToCollection(Medicoes[Medicoes.Count-1].Horario);
            DataInicio = Medicoes[3].Horario;
            DataFim = Medicoes[Medicoes.Count - 1].Horario;
        }
    }
}
