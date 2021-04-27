using System;
using System.Collections.Generic;
using System.Text;

namespace MobileMarket.Model
{
    public class Cupom
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Vencimento { get; set; }
        public double CustoCreditos { get; set; }
        public string CorHex { get; set; }
    }
}
