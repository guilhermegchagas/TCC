using MobileMarket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobileMarket.ViewModel
{
    public class CriarAlarmPageViewModel : BaseViewModel
    {
        public List<TipoMedicao> TiposMedicao { get; set; } = Enum.GetValues(typeof(TipoMedicao)).Cast<TipoMedicao>().ToList();
        public List<TipoCondicao> TiposCondicao { get; set; } = Enum.GetValues(typeof(TipoCondicao)).Cast<TipoCondicao>().ToList();

        private TipoMedicao _tipoMedicaoSelecionada;
        public TipoMedicao TipoMedicaoSelecionada
        {
            get { return _tipoMedicaoSelecionada; }
            set
            {
                _tipoMedicaoSelecionada = value;
                OnPropertyChanged(nameof(TipoMedicaoSelecionada));
            }
        }

        private TipoCondicao _tipoCondicaoSelecionada;
        public TipoCondicao TipoCondicaoSelecionada
        {
            get { return _tipoCondicaoSelecionada; }
            set
            {
                _tipoCondicaoSelecionada = value;
                OnPropertyChanged(nameof(TipoCondicaoSelecionada));
            }
        }
    }
}
