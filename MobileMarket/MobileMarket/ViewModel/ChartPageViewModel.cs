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

		public ChartPageViewModel()
		{
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
	}
}
