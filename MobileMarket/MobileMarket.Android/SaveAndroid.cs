using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using MobileMarket.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(MobileMarket.Droid.SaveAndroid))]
namespace MobileMarket.Droid
{
    public class SaveAndroid : ISave
    {
        [Obsolete]
        public string Save(string filename, string contentType, MemoryStream stream)
        {
            string exception = string.Empty;
            string root = null;
            root = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath;
            Java.IO.File myDir = new Java.IO.File(root + "/Syncfusion");
            myDir.Mkdir();
            Java.IO.File file = new Java.IO.File(myDir, filename);
            if (file.Exists()) file.Delete();
            try
            {
                FileOutputStream outs = new FileOutputStream(file);
                outs.Write(stream.ToArray());

                outs.Flush();
                outs.Close();
            }
            catch (Exception e)
            {
                exception = e.ToString();
            }
            if (file.Exists() && contentType != "application/html")
            {
                return file.AbsolutePath;
            }
            return "";
        }
    }
}