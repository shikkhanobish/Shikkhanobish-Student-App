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
         


            //List<int> b= new List<int>();
            //b.Add(0);
            //b.Add(0);
            //b.Add(0);
            //b.Add(0);
            //b.Add(0);
            //b.Add(0);
            //b.Add(0);
            //b.Add(0);
            //b.Add(0);
            //b.Add(0);
            //b.Add(0);
            //b.Add(0);
            //b.Add(0);
            //b.Add(0);
            //b.Add(0);
            //b.Add(0);

            //demoNotifi.ItemsSource = b;

            //Label title = new Label();
            //title.Text = "Notification";
            //title.FontSize = 18;
            //title.FontAttributes = FontAttributes.Bold;
            //title.HorizontalOptions = LayoutOptions.Center;
            //title.VerticalOptions = LayoutOptions.Center;

            //mainGrid.Children.Add(title);
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}