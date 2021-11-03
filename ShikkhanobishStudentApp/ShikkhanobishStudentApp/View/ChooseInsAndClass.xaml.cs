using Flurl.Http;
using ShikkhanobishStudentApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace ShikkhanobishStudentApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseInsAndClass : ContentPage
    {
        string selectClas;
        int clsIndex;
        public ChooseInsAndClass()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            clseight.BackgroundColor = Color.Transparent;
            clsnine.BackgroundColor = Color.Transparent;
            clsten.BackgroundColor = Color.Transparent;
            clsele.BackgroundColor = Color.Transparent;
            clstelv.BackgroundColor = Color.Transparent;           
        }
        private void clseight_Clicked(object sender, EventArgs e)
        {
            clseight.BackgroundColor = Color.FromHex("#B3E5FF");
            clsnine.BackgroundColor = Color.Transparent;
            clsten.BackgroundColor = Color.Transparent;
            clsele.BackgroundColor = Color.Transparent;
            clstelv.BackgroundColor = Color.Transparent;
            selectClas = "Class 8";
            clsIndex = 1;
            ShowMsg();
        }

        private void clsnine_Clicked(object sender, EventArgs e)
        {
            clseight.BackgroundColor = Color.Transparent;
            clsnine.BackgroundColor = Color.FromHex("#B3E5FF");
            clsten.BackgroundColor = Color.Transparent;
            clsele.BackgroundColor = Color.Transparent;
            clstelv.BackgroundColor = Color.Transparent;
            selectClas = "Class 9";
            clsIndex = 2;
            ShowMsg();
        }

        private void clsten_Clicked(object sender, EventArgs e)
        {
            clseight.BackgroundColor = Color.Transparent;
            clsnine.BackgroundColor = Color.Transparent;
            clsten.BackgroundColor = Color.FromHex("#B3E5FF");
            clsele.BackgroundColor = Color.Transparent;
            clstelv.BackgroundColor = Color.Transparent;
            selectClas = "Class 10";
            clsIndex = 3;
            ShowMsg();
        }

        private void clsele_Clicked(object sender, EventArgs e)
        {
            clseight.BackgroundColor = Color.Transparent;
            clsnine.BackgroundColor = Color.Transparent;
            clsten.BackgroundColor = Color.Transparent;
            clsele.BackgroundColor = Color.FromHex("#B3E5FF");
            clstelv.BackgroundColor = Color.Transparent;
            selectClas = "Class 11";
            clsIndex = 4;
            ShowMsg();
        }

        private void clstelv_Clicked(object sender, EventArgs e)
        {
            clseight.BackgroundColor = Color.Transparent;
            clsnine.BackgroundColor = Color.Transparent;
            clsten.BackgroundColor = Color.Transparent;
            clsele.BackgroundColor = Color.Transparent;
            clstelv.BackgroundColor = Color.FromHex("#B3E5FF");
            selectClas = "Class 12";
            clsIndex = 5;
            ShowMsg();
        }
        public async Task ShowMsg()
        {
            var actions = new string[] { "Yes", "No" };
            var result = await MaterialDialog.Instance.SelectActionAsync(title: "Do you want to take tuition from "+ selectClas + ". [You can change whenever you want]." ,
                                                             actions: actions);
            if(result == 0)
            {
                using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Please Wait..."))
                {
                    var allCls = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getClassInfo".GetJsonAsync<List<ClassInfo>>();
                    int insID = 0;
                    if(clsIndex == 1 || clsIndex == 2 || clsIndex == 3)
                    {
                        insID = 101;
                    }
                    else
                    {
                        insID = 102;
                    }
                    int clsID = 0;
                    if(clsIndex == 1)
                    {
                        clsID = 103;
                    }
                    if (clsIndex == 2)
                    {
                        clsID = 101;
                    }
                    if (clsIndex == 3)
                    {
                        clsID = 101;
                    }
                    if (clsIndex == 4)
                    {
                        clsID = 102;
                    }
                    if (clsIndex == 1)
                    {
                        clsID = 102;
                    }

                    var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setClassChoice".PostUrlEncodedAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID, institutionID  = insID, classID  = clsID})
                 .ReceiveJson<Response>();
                    await Application.Current.MainPage.Navigation.PushAsync(new RootPage());                  
                }


            }
        }
    }
}