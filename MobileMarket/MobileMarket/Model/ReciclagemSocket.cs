using MobileMarket.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;
using Xamarin.Forms;

namespace MobileMarket.Model
{
    public class ReciclagemSocket
    {
        public static ReciclagemSocket reciclagemSocket = new ReciclagemSocket();

        public WebSocket ws = null;
        public CarrinhoReciclagem CarrinhoReciclagem = new CarrinhoReciclagem();
        public QRConnectPage qrPage;
        public CarrinhoPage carrinhoPage;

        public void ConectarWebSocket()
        {
            if(ws == null)
            {
                ws = new WebSocket("ws://192.168.15.10:4649/ReciclagemSocket?CPF=" + ClienteInfo.CPF + "&ShortToken=" + ClienteInfo.ShortToken);
                //ws = new WebSocket("ws://reciclado-001-site1.etempurl.com:500/ReciclagemSocket?CPF=" + ClienteInfo.CPF + "&ShortToken=" + ClienteInfo.ShortToken);
                ws.OnMessage += (sender, e) => LerMensagem(sender, e);
                ws.Connect();
            }
        }

        public void LerMensagem(object sender, MessageEventArgs e)
        {
            switch(e.Data)
            {
                case "Start":
                    qrPage.GoToCarrinhoPage(sender, e);
                    break;
                case "FinalizarOK":
                    carrinhoPage.FinalizarOK();
                    break;
                case "FinalizarFailed":
                    carrinhoPage.FinalizarFailed();
                    break;
                default:
                    try
                    {
                        ItemReciclagem item = JsonConvert.DeserializeObject<ItemReciclagem>(e.Data);
                        item.quantidade = 1;
                        CarrinhoReciclagem.AddItem(item);
                        carrinhoPage.AtualizarCarrinho();
                    }
                    catch{}
                    break;
            }
        }

        public void FinalizarColeta()
        {
            ws.Send("Finalizar");
        }
    }
}
