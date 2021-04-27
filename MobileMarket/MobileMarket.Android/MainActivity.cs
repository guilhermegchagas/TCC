using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Forms;
using CarouselView.FormsPlugin.Droid;

namespace MobileMarket.Droid
{
    [Activity(Label = "MobileMarket", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Forms.SetFlags("CarouselView_Experimental");
            Forms.Init(this, savedInstanceState);
            CarouselViewRenderer.Init();

            LoadApplication(new App());
            XFGloss.Droid.Library.Init(this, savedInstanceState);

            #region Screen Height & Width  
            var pixels = Resources.DisplayMetrics.WidthPixels;
            var scale = Resources.DisplayMetrics.Density;
            var dps = (double)((pixels - 0.5f) / scale);
            var ScreenWidth = (int)dps;
            App.screenWidthPixels = pixels;
            App.screenWidthUnits = ScreenWidth;
            pixels = Resources.DisplayMetrics.HeightPixels;
            dps = (double)((pixels - 0.5f) / scale);
            var ScreenHeight = (int)dps;
            App.screenHeightPixels = pixels;
            App.screenHeightUnits = ScreenHeight;
            App.dpi = (double)Resources.DisplayMetrics.DensityDpi;
            #endregion
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public override void OnBackPressed()
        {
            if (!CHD.HWBackButtonManager.Instance.NotifyHWBackButtonPressed())
            {
                return;
            }

            base.OnBackPressed();
        }
    }
}