using System;
using System.Collections.Generic;
using System.Text;

namespace MobileMarket.Model
{
    public class Medicao
    {
        public int Codigo { get; set; }
        public DateTime Horario { get; set; }
        public double Potencia { get; set; }
        public int CodigoPonto { get; set; }

        public Medicao(int codigo, DateTime horario, double potencia, int codigoPonto)
        {
            Codigo = codigo;
            Horario = horario;
            Potencia = potencia;
            CodigoPonto = codigoPonto;
        }
    }
}
