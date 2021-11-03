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
    public partial class RattingPageView : ContentPage
    {
        public RattingPageView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        public async Task EndOrBackBtn()
        {

            var result = await MaterialDialog.Instance.ConfirmAsync(message: "Please rate teacher first",
                                  confirmingText: "OK"
                                  );         
        }

        protected override bool OnBackButtonPressed()
        {
            EndOrBackBtn();
            return true;
        }
        public async Task FinalRate()
        {
            using (var dialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Please Wait..."))
            {
                var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/FinalizeTuitionHistory".PostUrlEncodedAsync(new { tuitionID = StaticPageToPassData.lastTuitionHistoryID , ratting = StaticPageToPassData.lastRate, teacherID = StaticPageToPassData.lastTeacherID })
         .ReceiveJson<Response>();
                var existingPages = Navigation.NavigationStack.ToList();
                foreach (var page in existingPages)
                {
                    Navigation.RemovePage(page);
                }
                var reresponses = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/deletePendingTuition".PostUrlEncodedAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID })
                  .ReceiveJson<Response>();
                await StaticPageToPassData.GetStudent();
                StaticPageToPassData.isFromReg = false;
                await Application.Current.MainPage.Navigation.PushModalAsync(new TakeTuitionView());
                await dialog.DismissAsync();
            }
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            FinalRate();
        }

        private void AddFavTeacherButton_Clicked(object sender, EventArgs e)
        {
            AddFavTeacher();
        }
        public async Task AddFavTeacher()
        {
            using (var dialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Adding Favourite Teacher..."))
            {
                var res = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/addasFavouriteTeacher".PostUrlEncodedAsync(new { teacherID = StaticPageToPassData.lastTeacherID, studentID = StaticPageToPassData.thisStudentInfo.studentID })
         .ReceiveJson<Response>();
                favbyn.IsVisible = false;
                nofavlbl.IsVisible = true;
                await dialog.DismissAsync();
            }
        }

        private void ReportButton_Clicked_1(object sender, EventArgs e)
        {
            ReportTeacher();
        }
        public async Task ReportTeacher()
        {
            reportGrid.IsVisible = false;
            if(StaticPageToPassData.reportDes == null || StaticPageToPassData.reportDes == "") {
                StaticPageToPassData.reportDes = "N/A";
            }
            using (var dialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Thank you for your feedback. We are adding this issue in our server and will take action according to Shikkhanobish Terms And Condition..."))
            {
                var res = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/setReport".PostUrlEncodedAsync(new
                {
                    reportID = StaticPageToPassData.GenarateNewID(),
                    studentID = StaticPageToPassData.thisStudentInfo.studentID,
                    teacherID = StaticPageToPassData.lastTeacherID,
                    reportIndex = StaticPageToPassData.reportIndex,
                    description = StaticPageToPassData.reportDes,
                })
         .ReceiveJson<Response>();
                await dialog.DismissAsync();
            }
        }
    }
}