using Flurl.Http;
using FormsControls.Base;
using Plugin.LocalNotification;
using ShikkhanobishStudentApp.Model;
using ShikkhanobishStudentApp.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.Resources;
using XF.Material.Forms.UI.Dialogs;
using XF.Material.Forms.UI.Dialogs.Configurations;


namespace ShikkhanobishStudentApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
  
    public partial class TakeTuitionView : ContentPage
    {


        public TakeTuitionView()
        {
            InitializeComponent();
            BindingContext = new TakeTuitionViewModel(StaticPageToPassData.isFromReg);
            connectivityGrid.IsVisible = false;
          
            var current = Connectivity.NetworkAccess;
          
          
            if (current == NetworkAccess.Internet)
            {
                getAllInfo();
            }
            else
            {
              
                logoutBtn.IsEnabled = false;
                connectivityGrid.IsVisible = true;
                
                ShowSnakeBarError();
            }
           
        }
   

        public async Task getAllInfo()
        {
                NavigationPage.SetHasNavigationBar(this, false);
                var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
                var width = mainDisplayInfo.Width;
               
                Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
               

        }
        async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                logoutBtn.IsEnabled = true;
               
                connectivityGrid.IsVisible = false;
            }
            else
            {
                
                logoutBtn.IsEnabled = false;
                connectivityGrid.IsVisible = true;
                ShowSnakeBarError();
            }
            
        }
        public async Task ShowSnakeBarError()
        {
            var alertDialogConfiguration = new MaterialSnackbarConfiguration
            {
                BackgroundColor = XF.Material.Forms.Material.GetResource<Color>(MaterialConstants.Color.ERROR),
                MessageTextColor = XF.Material.Forms.Material.GetResource<Color>(MaterialConstants.Color.ON_PRIMARY).MultiplyAlpha(0.8),
                CornerRadius = 8,
                
                ScrimColor = Color.FromHex("#FFFFFF").MultiplyAlpha(0.32),
                ButtonAllCaps = false

            };

            await MaterialDialog.Instance.SnackbarAsync(message: "No Network Connection Avaiable",
                                        actionButtonText: "Got It",
                                        configuration: alertDialogConfiguration,
                                        msDuration: MaterialSnackbar.DurationIndefinite);
        }
        async private void paymentClicked(object sender, EventArgs e)
        {           
          
           
           
            
        }

        async private void homeClicked(object sender, EventArgs e)
        {

          
            
           
        }

        private void profileClicked(object sender, EventArgs e)
        {            
            Application.Current.MainPage.Navigation.PushModalAsync(new StudentProfile());
        }
        private void favouriteClicked(object sender, EventArgs e)
        {
          
          
           


        }
        public async Task ShowNotification(string msg)
        {

            await NotificationCenter.Current.Show((notification) => notification
                    .WithScheduleOptions((schedule) => schedule
                    
                    .Build())
                    .WithAndroidOptions((android) => android
                         .WithChannelId("General")
                         .WithTimeout(TimeSpan.FromSeconds(30))                      
                         .Build())
                    .WithiOSOptions((ios) => ios
                        .Build())
                    .WithReturningData("Dummy Data")
                    .WithTitle("Shikkhanobish")
                    .WithDescription(msg)
                    .WithNotificationId(100)
                    
                    .Create());;
        }
        private void Button_Clicked_3(object sender, EventArgs e)
        {
            SecureStorage.RemoveAll();
        }
       






        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new ForgotPassword());
        }

        private async void Button_Clicked_5(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                await getAllInfo();
                logoutBtn.IsEnabled = true;
                
                connectivityGrid.IsVisible = false;
                
            }
            else
            {
                logoutBtn.IsEnabled = false;
              
                connectivityGrid.IsVisible = true;
                ShowSnakeBarError();
            }
        }

       
        protected override bool OnBackButtonPressed()
        {
            EndOrBackBtn();
            return true;
        }

        private async Task EndOrBackBtn()
        {
            var actions = new string[] { "Yes", "No" };

            //Show simple dialog
            var result = await MaterialDialog.Instance.SelectActionAsync(title: "Do you want to exit this app?",
                                                             actions: actions);
            if(result == 0)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {

        }

        private void MaterialButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new ReferralView());
        }

        private void MaterialButton_Clicked_1(object sender, EventArgs e)
        {
            //refer
            Application.Current.MainPage.Navigation.PushAsync(new ReferralView());
            //Navigation.PushAsync(new CustomTransitionNavPage(new ReferralView()));
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushAsync(new NotificationView());
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushAsync(new ReportView());
        }
    }
}