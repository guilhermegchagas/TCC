using System;
using System.Collections.Generic;
using System.ComponentModel;
using MobileMarket.Converter;
using System.Text;

namespace MobileMarket.Model
{
    public enum TipoMedicao
    {
        [Description("Potência Total")]
        PotenciaTotal,
        [Description("Potência Reativa")]
        PotenciaReativa,
        [Description("Fator de Potência")]
        FatorPotencia,
        [Description("Corrente")]
        Corrente,
        [Description("Tensão")]
        Tensao,
        [Description("Frequência")]
        Frequencia
    }

    public enum TipoCondicao
    {
        [Description("Maior")]
        Maior,
        [Description("Menor")]
        Menor
    }
    public class Alarme
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public TipoCondicao TipoCondicao { get; set; }
        public TipoMedicao TipoMedicao { get; set; }
        public double ValorCondicao { get; set; }
        public DateTime HorarioAtualizacao { get; set; }
        public int CodigoPonto { get; set; }
        public bool IsDescricaoVisible
        {
            get
            {
                return !string.IsNullOrEmpty(Descricao);
            }
        }
        public string DescricaoCondicao
        {
            get
            {
                string shortTipoCondicao;
                switch(TipoCondicao)
                {
                    case TipoCondicao.Maior:
                        shortTipoCondicao = ">";
                        break;
                    case TipoCondicao.Menor:
                        shortTipoCondicao = "<";
                        break;
                    default:
                        shortTipoCondicao = "";
                        break;
                }
                return Converter.EnumConverter.GetDescription(TipoMedicao) + " " + shortTipoCondicao + " " + ValorCondicao.ToString();
            }
        }
    }
}
