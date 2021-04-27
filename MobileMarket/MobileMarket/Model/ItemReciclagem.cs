using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MobileMarket.Model
{
    public class ItemReciclagem
    {
        public string nome_produto { get; set; }
        public double vl_item_reciclagem { get; set; }
        private int _quantidade;
        public int quantidade
        {
            get {return _quantidade; }
            set 
            { 
                _quantidade = value; 
                total = value * vl_item_reciclagem; 
            }
        }
        public double total { get; set; }
    }
}
