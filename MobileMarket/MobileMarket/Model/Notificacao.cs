using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MobileMarket.Model
{
    public enum TipoNotificacao
    {
        [Description("Alerta Ativo")]
        AlertaAtivo,
        [Description("Alerta Inativo")]
        AlertaInativo
    }
    public class Notificacao
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public TipoNotificacao Tipo { get; set; }
        public DateTime Horario { get; set; }
        public int CodigoPonto { get; set; }
    }
}
