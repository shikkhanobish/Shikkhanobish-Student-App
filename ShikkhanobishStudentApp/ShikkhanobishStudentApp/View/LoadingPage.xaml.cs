using Flurl.Http;
using Newtonsoft.Json;
using ShikkhanobishStudentApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace ShikkhanobishStudentApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
            getInfo();
        }
        public async Task getInfo()
        {
            var currentAppVersion = VersionTracking.CurrentBuild;
            prgs.Progress = .1;
            var currentRealVersion = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getAppVersion".GetJsonAsync<AppVersion>();

            if (int.Parse(currentAppVersion) < currentRealVersion.studentAtVersion)
            {
                prgs.Progress = 1;
                using (await MaterialDialog.Instance.LoadingDialogAsync(message: "New Version Is Available! Please download latest version to use Shikkhanobish Student App..."))
                {
                    await Task.Delay(3000);
                    await Browser.OpenAsync("https://play.google.com/store/apps/details?id=com.shikkhanobish.shikkhanobishstudentapp");
                }
            }
            else
            {
                prgs.Progress = .3;
                var pn = await SecureStorage.GetAsync("phonenumber");
                var pass = await SecureStorage.GetAsync("passowrd");
                if (pn == null && pass == null)
                {
                    try
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                    }
                    catch (Exception ex)
                    {

                    }
                    //await Task.Delay(1000);
                    
                }
                else
                {
                    
                    StaticPageToPassData.thisstPass = pass;
                    StaticPageToPassData.thisStPh = pn;
                    StaticPageToPassData.isFromLogin = false;
                    bool isPending = false;
                    prgs.Progress = .5;
                    StaticPageToPassData.thisStudentInfo = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/LoginStudent".PostUrlEncodedAsync(new { phonenumber = StaticPageToPassData.thisStPh, password = StaticPageToPassData.thisstPass })
                  .ReceiveJson<Student>();
                    var pendingRatting = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getPendingRatting".GetJsonAsync<List<pendingRatting>>();
                   
                    string tuitionID = "";
                    for(int i  =0; i < pendingRatting.Count; i++)
                    {
                        if(pendingRatting[i].studentID == StaticPageToPassData.thisStudentInfo.studentID)
                        {
                            isPending = true;
                            tuitionID = pendingRatting[i].tuitionID;
                        }
                    }
                    prgs.Progress = .8;
                    if (!isPending)
                    {
                        //await Task.Delay(2000);
                        // await Application.Current.MainPage.Navigation.PushAsync(new TakeTuitionView(false));
                        //await Application.Current.MainPage.Navigation.PushAsync(new RootPage());
                        var thischoice = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getClassChoiceWithID".PostUrlEncodedAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID })
                 .ReceiveJson<ClassChoice>();
                        if(thischoice.studentID == 0)
                        {
                            await Application.Current.MainPage.Navigation.PushAsync(new ChooseInsAndClass());
                        }
                        else
                        {
                            await Application.Current.MainPage.Navigation.PushAsync(new RootPage());
                        }
                      
                    }
                    else
                    {
                        if(StaticPageToPassData.thisStudentInfo.studentID != 0)
                        {
                            using (await MaterialDialog.Instance.LoadingDialogAsync(message: "You didn't rate last time you took tuition. Going ratting page..."))
                            {
                                Task.Delay(4000);
                                StaticPageToPassData.lastTuitionHistoryID = tuitionID;
                                await Application.Current.MainPage.Navigation.PushModalAsync(new RattingPageView());
                            }
                        }
                           
                        
                    }
                    prgs.Progress = 1;
                    StaticPageToPassData.MakeActiveInServer();
                }

            }
            
            

        }
    }
}