using MobileMarket.Model;
using MobileMarket.ViewController;
using Syncfusion.SfDataGrid.XForms;
using Syncfusion.SfDataGrid.XForms.Exporting;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Xamarin.Forms;
using System.Linq;

namespace MobileMarket.ViewModel
{
    public interface ISave
    {
        string Save(string filename, string contentType, MemoryStream stream);
    }

    public class ChartPageViewModel : BaseViewModel
    {
        private bool AutoSetOk = false;

        private int _numeroPontos = 100;

        private bool _usarMedicaoComprimida = true;
        public bool UsarMedicaoComprimida 
        {
            get { return _usarMedicaoComprimida; }
            set
            {
                if(_usarMedicaoComprimida != value)
                {
                    _usarMedicaoComprimida = value;
                    SetMedicaoDisplay();
                    OnPropertyChanged(nameof(UsarMedicaoComprimida));
                }
            }
        }

        public string HeaderPT { get; set; } = "Potência" + Environment.NewLine + "Total(W)";
        public string HeaderPR { get; set; } = "Potência" + Environment.NewLine + "Reativa" + Environment.NewLine + "(VAR)";
        public string HeaderFP { get; set; } = "Fator" + Environment.NewLine + "de" + Environment.NewLine + "Potência";
        public string HeaderCorrente { get; set; } = "Corrente" + Environment.NewLine + "(A)";
        public string HeaderTensao { get; set; } = "Tensão" + Environment.NewLine + "(V)";
        public string HeaderFreq { get; set; } = "Frequência" + Environment.NewLine + "(Hz)";


        private Ponto _ponto = null;
		public Ponto Ponto
        {
            get { return _ponto; }
            set
            {
                _ponto = value;
                PrecoKWH = value.PrecoKWH;
                OnPropertyChanged(nameof(Ponto));
            }
        }

        public SfDataGrid dataGrid { get; set; }

        private ObservableCollection<Medicao> _medicoes = new ObservableCollection<Medicao> { };
        public ObservableCollection<Medicao> Medicoes
        {
            get { return _medicoes; }
            set
            {
                if(value == null)
                {
                    _medicoes = new ObservableCollection<Medicao> { };
                }
                else if (_medicoes != value)
                {
                    _medicoes = value;
                    CalcularMedias();
                    ComprimirMedicao();
                    SetMedicaoDisplay();
                }
                OnPropertyChanged(nameof(Medicoes));
            }
        }

        private ObservableCollection<Medicao> _medicoesCompressed = new ObservableCollection<Medicao> { };
        public ObservableCollection<Medicao> MedicoesCompressed
        {
            get { return _medicoesCompressed; }
            set
            {
                if (value == null)
                {
                    _medicoesCompressed = new ObservableCollection<Medicao> { };
                }
                else if (_medicoesCompressed != value)
                {
                    _medicoesCompressed = value;
                    SetMedicaoDisplay();
                }
                OnPropertyChanged(nameof(MedicoesCompressed));
            }
        }

        private ObservableCollection<Medicao> _medicoesDisplay = new ObservableCollection<Medicao> { };
        public ObservableCollection<Medicao> MedicoesDisplay
        {
            get { return _medicoesDisplay; }
            set
            {
                if (value == null)
                {
                    _medicoesDisplay = new ObservableCollection<Medicao> { };
                }
                else if(_medicoesDisplay != value)
                {
                    _medicoesDisplay = value;
                }
                OnPropertyChanged(nameof(MedicoesDisplay));
            }
        }

        private void SetMedicaoDisplay()
        {
            if (UsarMedicaoComprimida)
            {
                MedicoesDisplay = MedicoesCompressed;
            }
            else
            {
                MedicoesDisplay = Medicoes;
            }
        }

        private void ComprimirMedicao()
        {
            ObservableCollection<Medicao> medicoesCompressed = new ObservableCollection<Medicao>();
            long totalTimespan = (DataFim - DataInicio).Ticks;
            for (int i = 0; i < _numeroPontos; i++)
            {
                long startSpan = totalTimespan * (i) / 100;
                long endSpan = totalTimespan * (i+1) / 100;
                DateTime startDate = DataInicio.AddTicks(startSpan);
                DateTime endDate = DataInicio.AddTicks(endSpan);
                List<Medicao> medicoesSetDate = Medicoes.Where(medicao => medicao.Horario >= startDate && medicao.Horario < endDate).ToList();
                double mediaPotenciaTotal = medicoesSetDate.Count > 0 ? medicoesSetDate.Average(medicao => medicao.PotenciaTotal) : 0;
                double mediaPotenciaReativa = medicoesSetDate.Count > 0 ? medicoesSetDate.Average(medicao => medicao.PotenciaReativa) : 0;
                double mediaFatorPotencia = medicoesSetDate.Count > 0 ? medicoesSetDate.Average(medicao => medicao.FatorPotencia) : 0;
                double mediaCorrente = medicoesSetDate.Count > 0 ? medicoesSetDate.Average(medicao => medicao.Corrente) : 0;
                double mediaTensao = medicoesSetDate.Count > 0 ? medicoesSetDate.Average(medicao => medicao.Tensao) : 0;
                double mediaFrequencia = medicoesSetDate.Count > 0 ? medicoesSetDate.Average(medicao => medicao.Frequencia) : 0;
                Medicao newMedicao = new Medicao(i, startDate, mediaPotenciaTotal, mediaPotenciaReativa, mediaFatorPotencia, mediaCorrente, mediaTensao, mediaFrequencia, Ponto.Codigo);
                medicoesCompressed.Add(newMedicao);
            }
            MedicoesCompressed = medicoesCompressed;
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
                CalcularPrecoFinal();
                OnPropertyChanged(nameof(DataInicio));
                if(AutoSetOk)
                    UpdateMedicoes(true);
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
                CalcularPrecoFinal();
                OnPropertyChanged(nameof(DataFim));
                if (AutoSetOk)
                    UpdateMedicoes(true);
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

        #region Estatisticas
        private double _mediaPotenciaTotal = 0;
        public double MediaPotenciaTotal
        {
            get { return _mediaPotenciaTotal; }
            set
            {
                _mediaPotenciaTotal = value;
                CalcularPrecoFinal();
                OnPropertyChanged(nameof(MediaPotenciaTotal));
            }
        }

        private double _mediaPotenciaReativa = 0;
        public double MediaPotenciaReativa
        {
            get { return _mediaPotenciaReativa; }
            set
            {
                _mediaPotenciaReativa = value;
                OnPropertyChanged(nameof(MediaPotenciaReativa));
            }
        }

        private double _mediaFatorPotencia = 0;
        public double MediaFatorPotencia
        {
            get { return _mediaFatorPotencia; }
            set
            {
                _mediaFatorPotencia = value;
                OnPropertyChanged(nameof(MediaFatorPotencia));
            }
        }

        private double _mediaCorrente = 0;
        public double MediaCorrente
        {
            get { return _mediaCorrente; }
            set
            {
                _mediaCorrente = value;
                OnPropertyChanged(nameof(MediaCorrente));
            }
        }

        private double _mediaTensao = 0;
        public double MediaTensao
        {
            get { return _mediaTensao; }
            set
            {
                _mediaTensao = value;
                OnPropertyChanged(nameof(MediaTensao));
            }
        }

        private double _mediaFrequencia = 0;
        public double MediaFrequencia
        {
            get { return _mediaFrequencia; }
            set
            {
                _mediaFrequencia = value;
                OnPropertyChanged(nameof(MediaFrequencia));
            }
        }

        private double _precoKWH = 0;
        public double PrecoKWH
        {
            get { return _precoKWH; }
            set
            {
                if(_precoKWH != value)
                {
                    _precoKWH = value;
                    CalcularPrecoFinal();
                    OnPropertyChanged(nameof(PrecoKWH));
                }
            }
        }

        private double _precoFinal = 0;
        public double PrecoFinal
        {
            get { return _precoFinal; }
            set
            {
                _precoFinal = value;
                OnPropertyChanged(nameof(PrecoFinal));
            }
        }

        private void CalcularMedias()
        {
            if(Medicoes != null && Medicoes.Count > 0)
            {
                MediaPotenciaTotal = Medicoes.Average(medicao => medicao.PotenciaTotal);
                MediaPotenciaReativa = Medicoes.Average(medicao => medicao.PotenciaReativa);
                MediaFatorPotencia = Medicoes.Average(medicao => medicao.FatorPotencia);
                MediaCorrente = Medicoes.Average(medicao => medicao.Corrente);
                MediaTensao = Medicoes.Average(medicao => medicao.Tensao);
                MediaFrequencia = Medicoes.Average(medicao => medicao.Frequencia);
            }
        }

        private void CalcularPrecoFinal()
        {
            double numeroHoras = (DataFim - DataInicio).TotalHours;
            PrecoFinal = MediaPotenciaTotal/1000 * numeroHoras * PrecoKWH;
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
                    UpdateMedicoes(false);
                });
                return true;
            });
        }

        private void UpdateMedicoes(bool resetPosition)
        {
            dataGrid.CanMaintainScrollPosition = !resetPosition;
            Medicoes = HTTPRequest.GetMedicoes(Ponto.Codigo, DataInicio, DataFim);
        }

        public void AutoSet()
        {
            Medicoes = HTTPRequest.GetMedicoes(Ponto.Codigo, null, null);
            if(Medicoes != null && Medicoes.Count > 0)
            {
                DataInicioCollection = DateTimeToCollection(Medicoes[0].Horario);
                DataFimCollection = DateTimeToCollection(Medicoes[Medicoes.Count - 1].Horario);
                DataInicio = Medicoes[0].Horario;
                DataFim = Medicoes[Medicoes.Count - 1].Horario;
            }
            else
            {
                DataInicioCollection = DateTimeToCollection(DateTime.Today);
                DataFimCollection = DateTimeToCollection(DateTime.Now);
                DataInicio = DateTime.Today;
                DataFim = DateTime.Now;
            }
        }

        public string ExportarMedicoes()
        {
            DataGridExcelExportingController excelExport = new DataGridExcelExportingController();
            var excelEngine = excelExport.ExportToExcel(this.dataGrid);
            var workbook = excelEngine.Excel.Workbooks[0];
            MemoryStream stream = new MemoryStream();
            workbook.SaveAs(stream);
            workbook.Close();
            excelEngine.Dispose();

            return DependencyService.Get<ISave>().Save("Medicoes.xlsx", "application/msexcel", stream);
        }
    }
}
