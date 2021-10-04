using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace MobileMarket.ViewModel
{
    public class DateTimePickerViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<object> _selectedtime;
        public ObservableCollection<object> SelectedTime
        {
            get { return _selectedtime; }
            set { _selectedtime = value; RaisePropertyChanged("SelectedTime"); }
        }

        public DateTimePickerViewModel()
        {
            

        }

        void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
