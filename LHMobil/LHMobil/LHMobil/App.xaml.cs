using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LocalHaberlesmeSinifKutuphanesi.Istemci;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace LHMobil
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            //IstemciSinifi.BaglantiKes();
            //System.Diagnostics.Process.GetCurrentProcess().Kill();
            
        }

        protected override void OnResume()
        {
        }
        
    }
}
