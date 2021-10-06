using MobileMarket.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace MobileMarket.ViewModel
{
    public class ChartPageViewModel : BaseViewModel
    {
		public Ponto ponto { get; set; }
		public ObservableCollection<Medicao> Medicoes { get; set; }

        private string _medicaoSelecionada = "PotenciaTotal";
        public string MedicaoSelecionada
        {
            get { return _medicaoSelecionada; }
            set
            {
                _medicaoSelecionada = value;
                OnPropertyChanged(nameof(MedicaoSelecionada));
            }
        }

        #region Date Control
        private ObservableCollection<object> _dataInicioCollection;
        public ObservableCollection<object> DataInicioCollection
        {
            get { return _dataInicioCollection; }
            set 
            {
                _dataInicioCollection = value;
                OnPropertyChanged(nameof(DataInicioCollection));
                DataInicio = CollectionToDateTime(value);
            }
        }

        private DateTime _dataInicio;
        public DateTime DataInicio
        {
            get { return _dataInicio; }
            set
            {
                _dataInicio = value;
                OnPropertyChanged(nameof(DataInicio));
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
                DataFim = CollectionToDateTime(value);
            }
        }

        private DateTime _dataFim;
        public DateTime DataFim
        {
            get { return _dataFim; }
            set
            {
                _dataFim = value;
                OnPropertyChanged(nameof(DataFim));
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
            Medicoes = new ObservableCollection<Medicao>();
			for(int i = 1; i <= 10; i++)
            {
				Medicoes.Add(new Medicao(i, DateTime.Now.AddHours(i), 10 * i , 20 * i, 30 * i, 40 * i, 50* i, 60 * i, 70 * i, 37));
			}
			for (int i = 1; i <= 10; i++)
			{
				Medicoes.Add(new Medicao(i, DateTime.Now.AddDays(i), 10 * i, 20 * i, 30 * i, 40 * i, 50 * i, 60 * i, 70 * i, 37));
            }
        }
    }
}
