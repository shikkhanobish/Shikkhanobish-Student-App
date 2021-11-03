using Flurl.Http;
using ShikkhanobishStudentApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShikkhanobishStudentApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResgisterView : ContentPage
    {
        public ResgisterView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        int click = 0;
        private void Button_Clicked(object sender, EventArgs e)
        {
            StaticPageToPassData.RegisteredPhonenNumber = fi.Text + sec.Text + th.Text + fr.Text + fiv.Text + si.Text + sev.Text + ei.Text + ni.Text;
        }
        
        private void Button_Clicked_1(object sender, EventArgs e)
        {
            //fi.Text = ""; sec.Text = ""; th.Text = ""; fr.Text = ""; fiv.Text = ""; si.Text = ""; sev.Text = ""; ei.Text = ""; ni.Text = ""; 
            first.Text = ""; second.Text = ""; third.Text = ""; forth.Text = ""; fifth.Text = "";
        }
    }
}