using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileMarket.View
{
    public static class Helper
    {
        public static bool AssertEmptyEntry(Entry entry)
        {
            if (string.IsNullOrWhiteSpace(entry.Text))
            {
                return true;
            }
            return false;
        }

        public static bool AssertEmptyPicker(Picker picker)
        {
            if (picker.SelectedItem == null)
            {
                return true;
            }
            return false;
        }
    }
}
