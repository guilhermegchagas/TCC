using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XFGloss;

namespace MobileMarket.View
{
    public static class GradiantStyles
    {
        public static void SetContentPageGradiant(ContentPage page)
        {
			var bkgrndGradient = new Gradient()
			{
				Rotation = 150,
				Steps = new GradientStepCollection()
				{
					new GradientStep(Color.White, 0),
					new GradientStep(Color.FromHex("#e6ffe6"), 1)
				}
			};
			ContentPageGloss.SetBackgroundGradient(page,bkgrndGradient);
		}
	}
}
