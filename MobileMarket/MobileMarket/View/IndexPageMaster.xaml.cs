using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileMarket.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IndexPageMaster : ContentPage
    {
        public IndexPageMaster()
        {
            InitializeComponent();
        }

        private void ChangePage(object sender, SelectedItemChangedEventArgs e)
        {
            ListViewItem item = (ListViewItem)(e.SelectedItem);
            switch(item.Title)
            {
                case "Home":
                    ((MasterDetailPage)Parent).Detail = new NavigationPage(new IndexPageDetail());
                    ((MasterDetailPage)Parent).IsPresented = false;
                    break;
                case "Iniciar Coleta":
                    break;
                case "Resgatar Cupons":
                    break;
                case "Meus Cupons":
                    ((MasterDetailPage)Parent).Detail = new NavigationPage(new PontosPage());
                    ((MasterDetailPage)Parent).IsPresented = false;
                    break;
            }
        }
    }

    public class ListViewItem
    { 
        public string Title { get; set; }
    }
}