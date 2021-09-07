using System;
using System.Collections.Generic;
using System.Text;

namespace MobileMarket.Model
{
    public static class ClienteInfo
    {
        public static string ID { get; set; }
        public static string Nome { get; set; }
        public static string Email { get; set; }
        public static string Senha { get; set; }

        public static string Token { get; set; }

        public static void ClearInfo()
        {
            ID = string.Empty;
            Nome = string.Empty;
            Email = string.Empty;
            Senha = string.Empty;
            Token = string.Empty;
        }
    }
}
