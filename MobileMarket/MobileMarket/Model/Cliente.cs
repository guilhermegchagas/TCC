using System;
using System.Collections.Generic;
using System.Text;

namespace MobileMarket.Model
{
    public class Cliente
    {
        public string CPF { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public double Creditos { get; set; }
    }
}
