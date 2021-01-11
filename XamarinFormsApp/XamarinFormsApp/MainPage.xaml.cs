using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinFormsApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void ContentPageView_Clicked(object sender, EventArgs e)
        {
#if true
            // Allow hitting Back to get back to here.
            await Navigation.PushAsync(new CustomScanPage());
#else
            // Replace this page so we can't hit Back to get back to here.
            Navigation.InsertPageBefore(new CustomScanPage(), this);
            await Navigation.PopAsync();
#endif
        }
    }
}
