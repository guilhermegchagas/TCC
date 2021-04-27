using System;
using System.Collections.Generic;
using System.Text;

namespace MobileMarket.Model
{
    public static class ClienteInfo
    {
        public static string CPF { get; set; }
        public static string Nome { get; set; }
        public static string Sobrenome { get; set; }
        public static string Email { get; set; }
        public static string Senha { get; set; }
        public static double Creditos { get; set; }

        public static string Token { get; set; }

        public static string ShortToken { get; set; }

        public static void ClearInfo()
        {
            CPF = string.Empty;
            Nome = string.Empty;
            Sobrenome = string.Empty;
            Email = string.Empty;
            Senha = string.Empty;
            Creditos = 0;
            Token = string.Empty;
            ShortToken = string.Empty;
        }
    }
}
