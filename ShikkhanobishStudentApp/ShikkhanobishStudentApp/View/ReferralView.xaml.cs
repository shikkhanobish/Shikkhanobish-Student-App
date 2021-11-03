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
    public partial class ReferralView : ContentPage
    {
        public Student thisStudent = new Student();
        List<ReferralTable> allrefarerral = new List<ReferralTable>();
        ReferralTable thisref = new ReferralTable();
        List<Student> allStudent = new List<Student>();
        public ReferralView()
        {
            InitializeComponent();
            GetAllStudent();
            NavigationPage.SetHasNavigationBar(this, false);
            //SetALlStudent();
        }
        public async Task SetALlStudent() {
            allStudent = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getStudent".GetJsonAsync<List<Student>>();
            for(int i = 0; i < allStudent.Count; i++)
            {
                Random random = new Random();
                int length = 7;
                const string chars = "AB01CDEFG356HIJKLMN4OPQRS78TUVWXYZ29";
                string id = new string(Enumerable.Repeat(chars, length)
                    .Select(s => s[random.Next(s.Length)]).ToArray());

                var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setReferralTable".PostUrlEncodedAsync(new { referralID = id, studentID = allStudent[i].studentID }).ReceiveJson<Response>();
            }
        }

        public async Task GetAllStudent()
        {
            submitbtn.IsEnabled = false;
            allStudent = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getStudent".GetJsonAsync<List<Student>>();
            allrefarerral = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getRefferalTable".GetJsonAsync<List<ReferralTable>>();
            bool isGive = false;
            for (int i = 0; i < allrefarerral.Count; i++)
            {
                if(allrefarerral[i].studentID == StaticPageToPassData.thisStudentInfo.studentID)
                {
                    thisref = allrefarerral[i];
                    rfrCode.Text = allrefarerral[i].referralID;
                    if(allrefarerral[i].referredStudentID == 0)
                    {
                        used.Text = "N/A";                       
                    }
                    else
                    {
                       
                        for(int j = 0; j < allStudent.Count; j++)
                        {
                            if(allStudent[j].studentID == allrefarerral[i].referredStudentID)
                            {
                                used.Text = "" + allStudent[j].name;
                            }
                        }
                        
                    }
                }
                
                if (allrefarerral[i].referredStudentID == StaticPageToPassData.thisStudentInfo.studentID)
                {
                    isGive = true;                 
                    for (int j = 0; j < allStudent.Count; j++)
                    {
                        if (allStudent[j].studentID == allrefarerral[i].studentID)
                        {
                            give.Text = "" + allStudent[j].name;
                        }
                    }
                }

                
            }
            if (!isGive)
            {
                give.Text = "N/A";
                submitbtn.IsEnabled = true;
            }
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            ShowPopUp();
        }
        public async Task ShowPopUp()
        {
            await MaterialDialog.Instance.AlertAsync(message: "রেফার কর তোমার বন্ধুকে। তুমি এবং তোমার বন্ধু জিতে নাও ৫ মিনিট ফ্রি টিউশন! তোমার বন্ধুকে শিক্ষানবিশ একাউন্টে রেফার অপশনে গিয়ে উপরের নাম্বারটি \"Enter Referral Code\" এ পেস্ট করতে বল এবং দুইজনেই জিতে নাও ৫ মিনিট করে ফ্রি টিউশন!");
        }
        private void MaterialButton_Clicked(object sender, EventArgs e)
        {
            CheckRefferID();
        }
        public async Task CheckRefferID()
        {
            bool ok = true;
            int index = 0;
            if (codetxt.Text != null && codetxt.Text != "")
            {
                for (int i = 0; i < allrefarerral.Count; i++)
                {
                    if (codetxt.Text == allrefarerral[i].referralID.ToString())
                    {
                        if (allrefarerral[i].studentID == StaticPageToPassData.thisStudentInfo.studentID || allrefarerral[i].referredStudentID == StaticPageToPassData.thisStudentInfo.studentID)
                        {
                            ok =  false;
                            index = 1;                           
                        }
                        break;
                    }
                    if(i == allrefarerral.Count - 1)
                    {
                        index = 2;
                        ok = false;
                    }
                }
            }
            if (ok)
            {
                int stID = 0;
                for(int i =0; i < allrefarerral.Count; i++)
                {
                    if(codetxt.Text == allrefarerral[i].referralID.ToString())
                    {
                        stID = allrefarerral[i].studentID;
                        var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/registerReferral".PostUrlEncodedAsync(new { referredStudentID = StaticPageToPassData.thisStudentInfo.studentID, referralID = codetxt.Text, studentID = stID }).ReceiveJson<Response>();
                    }
                }
              
                for(int i = 0; i < allStudent.Count; i++)
                {
                   if(allStudent[i].studentID == stID)
                   {
                       await MaterialDialog.Instance.AlertAsync(message: "Congratulation! You got 5 min free tuition minute from " + allStudent[i].name);
                   }
                }
            }
            else
            {
                if(index == 1)
                {
                    await MaterialDialog.Instance.AlertAsync(message: "You can not use this referral code!");
                }
                else
                {
                    await MaterialDialog.Instance.AlertAsync(message: "Code doesn't match!");
                }
                
            }
        }
    }
}