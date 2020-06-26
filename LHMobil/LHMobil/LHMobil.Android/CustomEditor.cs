using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using LHMobil.Droid;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Editor), typeof(CustomEditor))]
namespace LHMobil.Droid
{
    class CustomEditor : EditorRenderer
    {
        public CustomEditor(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if(Control != null)
            {
                Control.InputType = 0;
            }
        }
    }
}