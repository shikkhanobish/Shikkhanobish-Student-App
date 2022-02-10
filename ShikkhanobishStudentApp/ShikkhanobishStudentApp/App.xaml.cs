using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ShikkhanobishStudentApp.View;
using Xamarin.Essentials;
using System.Threading.Tasks;
using ShikkhanobishStudentApp.Model;
using Flurl.Http;
using FormsControls.Base;
using Plugin.SharedTransitions;

namespace ShikkhanobishStudentApp
{

    public partial class App : Application
    {
        int chapterID;
        NetworkAccess current = Connectivity.NetworkAccess;
        public App()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);
            chapterID = 1004;
            MainPage = new SharedTransitionNavigationPage(new LiveSuport());
        }
        
        
        protected override void OnStart()
        {
            StaticPageToPassData.OnStart();
        }

        protected override void OnSleep()
        {
            StaticPageToPassData.OnPause();
        }

        protected override void OnResume()
        {
            StaticPageToPassData.OnStart();
        }

    }
}
