using MobileMarket.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MobileMarket.ViewModel
{
    public class ChartPageViewModel : BaseViewModel
    {
		public Ponto ponto { get; set; }
		public ObservableCollection<Medicao> Medicoes { get; set; }

        private ObservableCollection<object> _dataInicio;
        public ObservableCollection<object> DataInicio
        {
            get { return _dataInicio; }
            set 
            { 
                _dataInicio = value;
                OnPropertyChanged(nameof(DataInicio));
            }
        }

        private ObservableCollection<object> _dataFim;
        public ObservableCollection<object> DataFim
        {
            get { return _dataFim; }
            set
            {
                _dataFim = value;
                OnPropertyChanged(nameof(DataFim));
            }
        }

        public ChartPageViewModel()
		{
            setInitialDateTime();
			Medicoes = new ObservableCollection<Medicao>();
			for(int i = 1; i <= 10; i++)
            {
				Medicoes.Add(new Medicao(i, DateTime.Now.AddHours(i), 10*i, 37));
			}
			for (int i = 1; i <= 10; i++)
			{
				Medicoes.Add(new Medicao(i, DateTime.Now.AddDays(i), 10 * i, 37));
			}
		}

		private void setInitialDateTime()
        {
            ObservableCollection<object> todaycollection = new ObservableCollection<object>();

            DateTime daytime = DateTime.Now;
            daytime.ToString("dd/mm/yyy HH:mm:ss");

            //Current date is stated as today 
            todaycollection.Add("Today");

            //Current hour is selected if hour is less than 13 else it is subtracted by 12 to maintain 12hour format
            if (daytime.Hour < 13)
            {
                todaycollection.Add(DateTime.Now.Hour.ToString());
            }
            else
            {
                todaycollection.Add((DateTime.Now.Hour - 12).ToString());
            }

            //Current minute is selected
            todaycollection.Add(DateTime.Now.Minute.ToString());

            //Format is selected as AM if hour is less than 12 else PM is selected
            if (daytime.Hour < 12)
            {
                todaycollection.Add("AM");
            }
            else
            {
                todaycollection.Add("PM");
            }

            //Update the current date and time
            DataInicio = todaycollection;
            DataFim = todaycollection;
        }
	}
}
