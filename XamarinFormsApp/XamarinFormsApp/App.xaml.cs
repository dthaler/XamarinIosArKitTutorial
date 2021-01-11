using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinFormsApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

#if false
            // Use this if we only have one Xamarin.Forms page.
            MainPage = new MainPage();
#else
            // Use this to allow navigating between multiple Xamarin.Forms pages.
            MainPage = new NavigationPage(new MainPage());
#endif
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
