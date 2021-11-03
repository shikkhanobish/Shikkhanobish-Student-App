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
    public partial class ReportView : ContentPage
    {
        public ReportView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);          
            getAllReport();
        }
        public async Task getAllReport()
        {
            var regRes = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getStudentReportWithStudentID"
              .PostUrlEncodedAsync(new
              {
                  studentID = StaticPageToPassData.thisStudentInfo.studentID
                  
              })
              .ReceiveJson<List<StudentReport>>();
            rtitm.ItemsSource = regRes;
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}