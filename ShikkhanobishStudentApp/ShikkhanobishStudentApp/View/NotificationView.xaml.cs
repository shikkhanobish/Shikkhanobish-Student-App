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
    public partial class NotificationView : ContentPage
    {
        public NotificationView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            List<int> a = new List<int>();
            a.Add(0);
            a.Add(0);
            a.Add(0);
            a.Add(0);
            a.Add(0);
            a.Add(0);
            a.Add(0);

            rtitm.ItemsSource = a;
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}