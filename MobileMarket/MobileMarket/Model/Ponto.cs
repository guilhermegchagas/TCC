using System;
using System.Collections.Generic;
using System.Text;

namespace MobileMarket.Model
{
    public class Ponto
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public double PrecoKWH { get; set; }
        public int CodigoUsuario { get; set; }
        public bool IsDescricaoVisible
        { 
            get
            {
                return !string.IsNullOrEmpty(Descricao);
            }
        }
        public string TitlePonto
        {
            get
            {
                return "Ponto: " + Nome;
            }
        }
    }
}
