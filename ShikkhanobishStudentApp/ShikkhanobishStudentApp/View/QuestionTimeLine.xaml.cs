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
    public partial class QuestionTimeLine : ContentPage
    {
        public QuestionTimeLine()
        {
            InitializeComponent();
            tagFrid.IsVisible = false;
            List<string> gg = new List<string>();
            gg.Add("Bal");
            gg.Add("sal");
            schoolCheckBoxGroup.Choices = gg;
            collegeBoxGroup.Choices = gg;
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void MaterialButton_Clicked(object sender, EventArgs e)
        {
            tagFrid.IsVisible = true;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            tagFrid.IsVisible = false;
        }

        private void MaterialButton_Clicked_1(object sender, EventArgs e)
        {
            tagFrid.IsVisible = false;
        }
    }
}