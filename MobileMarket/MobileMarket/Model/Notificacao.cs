using System;
using System.Collections.Generic;
using System.Text;

namespace MobileMarket.Model
{
    public class Notificacao
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Tipo { get; set; }
        public DateTime Horario { get; set; }
        public int CodigoPonto { get; set; }
    }
}
