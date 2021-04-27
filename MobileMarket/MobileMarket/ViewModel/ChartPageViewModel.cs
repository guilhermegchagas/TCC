using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MobileMarket.ViewModel
{
    public class ChartPageViewModel : BaseViewModel
    {
		public ObservableCollection<MonthDemand> Demands { get; set; }

		public ChartPageViewModel()
		{
			Demands = new ObservableCollection<MonthDemand>();
			Demands.Add(new MonthDemand("Jan", 42, 27));
			Demands.Add(new MonthDemand("Feb", 44, 28));
			Demands.Add(new MonthDemand("Mar", 53, 35));
			Demands.Add(new MonthDemand("Apr", 64, 44));
			Demands.Add(new MonthDemand("May", 75, 54));
			Demands.Add(new MonthDemand("Jun", 83, 63));
			Demands.Add(new MonthDemand("Jul", 87, 68));
			Demands.Add(new MonthDemand("Aug", 84, 66));
			Demands.Add(new MonthDemand("Sep", 78, 59));
			Demands.Add(new MonthDemand("Oct", 67, 48));
			Demands.Add(new MonthDemand("Nov", 55, 38));
			Demands.Add(new MonthDemand("Dec", 45, 29));

		}
	}

	public class MonthDemand
	{
		public string Demand { get; set; }
		public double Year2010 { get; set; }
		public double Year2011 { get; set; }
		public MonthDemand(string demand, double year2010, double year2011)
		{
			this.Demand = demand;
			this.Year2010 = year2010;
			this.Year2011 = year2011;
		}
	}
}
