using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MobileMarket.Model
{
    public class CarrinhoReciclagem
    {
        public List<ItemReciclagem> listaItems = new List<ItemReciclagem>();

        public void AddItem(ItemReciclagem item)
        {
            ItemReciclagem itemExistente = listaItems.Where(i => i.nome_produto == item.nome_produto).FirstOrDefault();
            if(itemExistente != null)
            {
                itemExistente.quantidade += item.quantidade;
            }
            else
            {
                listaItems.Add(item);
            }   
        }

        public IEnumerable<ItemReciclagem> GetItems()
        {
            return listaItems;
        }

        public double GetSum()
        {
            return listaItems.Sum(item => item.vl_item_reciclagem);
        }
    }
}
