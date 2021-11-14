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
    public partial class QuestionTimeLine : ContentPage
    {
        public QuestionTimeLine()
        {
            InitializeComponent();
            tagFrid.IsVisible = false;
            List<string> gg = new List<string>();
            gg.Add("Bal");
            gg.Add("sal");
            gg.Add("Bal");
            gg.Add("sal");
            gg.Add("Bal");
            gg.Add("sal");
            gg.Add("Bal");
            gg.Add("sal");
            gg.Add("Bal");
            gg.Add("sal");
            gg.Add("Bal");
            gg.Add("sal");
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
            if (result == 0)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

        }

        private void MaterialCard_Clicked(object sender, EventArgs e)
        {
            tagFrid.IsVisible = true;

        }
    }
}