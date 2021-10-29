using System;
using System.Collections.Generic;
using System.Text;

namespace MobileMarket.Model
{
    public class Medicao
    {
        public int Codigo { get; set; }
        public DateTime Horario { get; set; }
        public double PotenciaTotal { get; set; }
        public double PotenciaReativa { get; set; }
        public double FatorPotencia { get; set; }
        public double Corrente { get; set; }
        public double Tensao { get; set; }
        public double Frequencia { get; set; }
        public int CodigoPonto { get; set; }

        public Medicao(
            int codigo, 
            DateTime horario, 
            double potencia_total,
            double potencia_reativa,
            double fator_potencia,
            double corrente,
            double tensao,
            double frequencia,
            int codigoPonto)
        {
            Codigo = codigo;
            Horario = horario;
            PotenciaTotal = potencia_total;
            PotenciaReativa = potencia_reativa;
            FatorPotencia = fator_potencia;
            Corrente = corrente;
            Tensao = tensao;
            Frequencia = frequencia;
            CodigoPonto = codigoPonto;
        }
    }
}
