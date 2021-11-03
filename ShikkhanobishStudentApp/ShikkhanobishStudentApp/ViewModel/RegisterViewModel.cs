using Flurl.Http;
using ShikkhanobishStudentApp.Model;
using ShikkhanobishStudentApp.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace ShikkhanobishStudentApp.ViewModel
{
    public class RegisterViewModel : BaseViewModel, INotifyPropertyChanged
    {
        int btnNav;
        public string studentPhonenumber;
        int sec;
        List<Student> allStudent { get; set; }
        public RegisterViewModel()
        {
            sec = 60;
            waitVisibility = false;
            btnNav = 1;
            btnTxt = "Send Otp";
            varificationvisibility = true;
            numberVisibility = true;
            titleTxt = "Enter Your Phone Number";
            egTxt = "e.g: 01xxxxxxxxx";
            sendAgainVisibility = false;
            infoVisibility = false;
            registerbtvEnabled = true;
            otpVisibility = false;
            sndAgainEnabled = false;
        }
        public ICommand btnCommand =>
             new Command<string>( async(index) =>
             {

                 if(int.Parse(index) == 0)
                 {
                     btnNav = 1;
                     btnTxt = "Send Otp";
                     varificationvisibility = true;
                     numberVisibility = true;
                     titleTxt = "Enter Your Phone Number";
                     egTxt = "e.g: 01xxxxxxxxx";
                     sendAgainVisibility = false;
                     infoVisibility = false;
                     registerbtvEnabled = true;
                     otpVisibility = false;
                     otpfiText = "";
                     otpsecText = "";
                     otpthText = "";
                     otpfrText = "";
                     otpfivText = "";
                 }
                 else
                 {
                     if (btnNav == 1)
                     {
                         using (var dialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Checking Phonenumber..."))
                         {
                             studentPhonenumber = "01"+StaticPageToPassData.RegisteredPhonenNumber;
                             await CheckPhonenumber();
                             if (uniquePhonenumber == true)
                             {
                                 dialog.MessageText = "Sending OTP To Your Number...";
                                 await sendsms();
                                 sndAgainEnabled = false;
                                 btnNav++;
                                 titleTxt = "Enter 5 Digit OTP ";
                                 numberVisibility = false;
                                 egTxt = "";
                                 btnTxt = "Varify";
                                 sendAgainColor = Color.Gray;
                                 sendAgainVisibility = true;
                                 varificationvisibility = true;
                                 otpVisibility = true;
                                 entryText = "";
                                 Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                                 {
                                     if (sec == 0)
                                     {
                                         sendAgainColor = Color.Green;
                                         sec = 60;
                                         sendAgainText = "Send OTP Again ";
                                         sndAgainEnabled = true;
                                         btnNav--;
                                         return false;
                                     }
                                     else
                                     {
                                         sec--;
                                         sendAgainText = "Send OTP Again In " + sec + " Seconds";
                                         return true;
                                     }


                                 });
                             }
                             else
                             {
                                 errorText = "Phone Number Already Registered";
                             }
                         }
                         
                         
                        
                     }
                     else if (btnNav == 2)
                     {
                         string otp = StaticPageToPassData.otpcode;
                         if (otpfiText != null && otpsecText != null && otpthText != null && otpfrText != null && otpfivText != null)
                         {
                             if(otpfiText == otp[0].ToString() && otpsecText == otp[1].ToString() && otpthText == otp[2].ToString() && otpfrText == otp[3].ToString() && otpfivText == otp[4].ToString() )
                             {                              
                                 btnNav++;
                                 errorText = "";
                                 otpVisibility = false;
                                 titleTxt = "Enter Your Information";
                                 infoVisibility = true;
                                 btnTxt = "Register";
                                 sendAgainVisibility = false;
                                 
                                 
                             }
                             else
                             {
                                 errorText = "OTP doesnt match";
                             }
                             
                         }
                         else
                         {
                             errorText = "OTP can't be empty";
                         }

                     }
                     else if (btnNav == 3)
                     {
                         bool allok = true;
                         if (password != null)
                         {
                             if (!password.Any(char.IsDigit) && password.Length < 6)
                             {
                                 passimgsrc = "wrong.png";
                                 pasMargin = 9;
                                 allok = false;
                                 errorText = "Password length should be 6 and at least contain one digit";
                             }
                             else
                             {
                                 passimgsrc = "right.png";
                                 pasMargin = 10;
                             }
                         }
                         else
                         {
                             passimgsrc = "wrong.png";
                             pasMargin = 9;
                             allok = false;
                         }

                         if (conPassword != null)
                         {
                             if (conPassword != password)
                             {
                                 conpassimgsrc = "wrong.png";
                                 conpasMargin = 9;
                                 allok = false;
                                 errorText = "Password doesn't match";
                             }
                             else
                             {
                                 conpassimgsrc = "right.png";
                                 conpasMargin = 10;
                             }
                         }
                         else
                         {
                             conpassimgsrc = "wrong.png";
                             conpasMargin = 9;
                             allok = false;
                         }


                         if (name == null || name == "")
                         {
                             nameimgsrc = "wrong.png";
                             nameMargin = 9;
                             allok = false;
                             errorText = "Name shouldn't be empty";
                         }
                         else
                         {
                             nameimgsrc = "right.png";
                             nameMargin = 10;
                         }

                         if (city == null || city == "")
                         {
                             cityimgsrc = "wrong.png";
                             cityMargin = 9;
                             allok = false;
                             errorText = "City shouldn't be empty";
                         }
                         else
                         {
                             cityimgsrc = "right.png";
                             cityMargin = 10;
                         }

                         if (allok == true)
                         {
                             registerbtvEnabled = true;
                             await RegisterStudnet();
                             if(regRes.Massage == "Succesfull!")
                             {
                                 await SecureStorage.SetAsync("phonenumber", studentPhonenumber);
                                 await SecureStorage.SetAsync("passowrd", password);
                                 StaticPageToPassData.isFromReg = true;
                                 await Application.Current.MainPage.Navigation.PushAsync(new ChooseInsAndClass());
                             }
                             else
                             {
                                 errorText = regRes.Massage;
                             }
                             
                         }

                     }
                 }
                 
             });

        private void PerformsendAgainBtn()
        {
            sendsms();
            sndAgainEnabled = false;
            sendAgainColor = Color.Gray;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (sec == 0)
                {
                    sendAgainColor = Color.Green;
                    sec = 60;
                    sendAgainText = "Send OTP Again ";
                    sndAgainEnabled = true;
                    return false;
                }
                else
                {
                    sec--;
                    sendAgainText = "Send OTP Again In " + sec + " Seconds";
                    return true;
                }


            });
        }
        public async Task sendsms()
        {
            Random rd = new Random();
            string number = "+8801" + StaticPageToPassData.RegisteredPhonenNumber;
            int code = rd.Next(10000, 99999);
            string msg = "Shikkhanobish Verification Code: " + code;
            var regRes = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/SendSmsAsync"
        .PostUrlEncodedAsync(new { msg = msg, number = number })
        .ReceiveJson<SendSms>();
            StaticPageToPassData.otpcode = code.ToString();
        }

        bool uniquePhonenumber = true;
        Response regRes;
        public async Task CheckPhonenumber()
        {
            allStudent = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getStudent".GetJsonAsync<List<Student>>();
            if(allStudent.Count != 0)
            {
                for (int i = 0; i < allStudent.Count; i++)
                {
                    if (studentPhonenumber == allStudent[i].phonenumber)
                    {
                        uniquePhonenumber = false;
                        break;
                    }
                }
            }
            
        }
        public async Task RegisterStudnet()
        {
            waitVisibility = true;
            prgs = .1;
            prgsPercent = "10%";
            int thisUSerID = 0;
            
            if (allStudent.Count == 0)
            {
                thisUSerID = 10000001;
            }
            else
            {
                int maxID = allStudent[0].studentID;
                for (int i = 0; i < allStudent.Count; i++)
                {
                    if (maxID < allStudent[i].studentID)
                    {
                        maxID = allStudent[i].studentID;
                    }
                }
                thisUSerID = maxID + 1;
            }
            prgs = .3;
            prgsPercent = "30%";
          
            prgs = .8;
            prgsPercent = "80%";
            StaticPageToPassData.isFromLogin = false;
            regRes = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setStudent"
          .PostUrlEncodedAsync(new { studentID = thisUSerID, phonenumber = studentPhonenumber, password = password, totalSpent = 0, totalTuitionTime = 0, coin = 0, freemin = 10, city = city, name = name, institutionName  = "none"})
          .ReceiveJson<Response>();
           prgs = 1;
           prgsPercent = "100%";
            // StaticPageToPassData.MakeActiveInServer();
            Random random = new Random();
            int length = 7;
            const string chars = "AB01CDEFG356HIJKLMN4OPQRS78TUVWXYZ29";
            string id = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setReferralTable".PostUrlEncodedAsync(new { referralID = id, studentID = thisUSerID }).ReceiveJson<Response>();
        }
         public ICommand goBack =>
             new Command(() =>
             {
                 Application.Current.MainPage.Navigation.PopAsync();
             });


        private async Task PerformgoToTermConditionPgae()
        {
            await Browser.OpenAsync("https://www.termsandcondiitionssample.com/live.php?token=fqOkRWtl84cXwPJdjXKTKRKxHxive1lj", BrowserLaunchMode.SystemPreferred);
        }
        #region Binding 
        private bool numberVisibility1;

        public bool numberVisibility { get => numberVisibility1; set => SetProperty(ref numberVisibility1, value); }
        private bool varificationvisibility1;

        public bool varificationvisibility { get => varificationvisibility1; set => SetProperty(ref varificationvisibility1, value); }

        private string btnTxt1;

        public string btnTxt { get => btnTxt1; set => SetProperty(ref btnTxt1, value); }

        private string titleTxt1;

        public string titleTxt { get => titleTxt1; set => SetProperty(ref titleTxt1, value); }

        private string egTxt1;

        public string egTxt { get => egTxt1; set => SetProperty(ref egTxt1, value); }

        private bool sendAgainVisibility1;

        public bool sendAgainVisibility { get => sendAgainVisibility1; set => SetProperty(ref sendAgainVisibility1, value); }

        private Color sendAgainColor1;

        public Color sendAgainColor { get => sendAgainColor1; set => SetProperty(ref sendAgainColor1, value); }

        private string entryText1;

        public string entryText { get => entryText1; set => SetProperty(ref entryText1, value); }

        private bool infoVisibility1;

        public bool infoVisibility { get => infoVisibility1; set => SetProperty(ref infoVisibility1, value); }

        private string password1;

        public string password { get => password1; set => SetProperty(ref password1, value); }

        private string conPassword1;

        public string conPassword { get => conPassword1; set => SetProperty(ref conPassword1, value); }

        private string name1;

        public string name { get => name1; set => SetProperty(ref name1, value); }

        private string city1;

        public string city { get => city1; set => SetProperty(ref city1, value); }

        private string passimgsrc1;

        public string passimgsrc { get => passimgsrc1; set => SetProperty(ref passimgsrc1, value); }

        private Thickness nameMargin1;

        public Thickness nameMargin { get => nameMargin1; set => SetProperty(ref nameMargin1, value); }

        private string conpassimgsrc1;

        public string conpassimgsrc { get => conpassimgsrc1; set => SetProperty(ref conpassimgsrc1, value); }

        private Thickness pasMargin1;

        public Thickness pasMargin { get => pasMargin1; set => SetProperty(ref pasMargin1, value); }

        private string nameimgsrc1;

        public string nameimgsrc { get => nameimgsrc1; set => SetProperty(ref nameimgsrc1, value); }

        private Thickness cityMargin1;

        public Thickness cityMargin { get => cityMargin1; set => SetProperty(ref cityMargin1, value); }

        private string cityimgsrc1;

        public string cityimgsrc { get => cityimgsrc1; set => SetProperty(ref cityimgsrc1, value); }

        private Thickness conpasMargin1;

        public Thickness conpasMargin { get => conpasMargin1; set => SetProperty(ref conpasMargin1, value); }

        private bool registerbtvEnabled1;

        public bool registerbtvEnabled { get => registerbtvEnabled1; set => SetProperty(ref registerbtvEnabled1, value); }

        private bool otpVisibility1;

        public bool otpVisibility { get => otpVisibility1; set => SetProperty(ref otpVisibility1, value); }

        private string otpfiText1;

        public string otpfiText { get => otpfiText1; set => SetProperty(ref otpfiText1, value); }

        private string otpsecText1;

        public string otpsecText { get => otpsecText1; set => SetProperty(ref otpsecText1, value); }

        private string otpfrText1;

        public string otpfrText { get => otpfrText1; set => SetProperty(ref otpfrText1, value); }

        private string otpthText1;

        public string otpthText { get => otpthText1; set => SetProperty(ref otpthText1, value); }

        private string errorText1;

        public string errorText { get => errorText1; set => SetProperty(ref errorText1, value); }

        private string otpfivText1;

        public string otpfivText { get => otpfivText1; set => SetProperty(ref otpfivText1, value); }

        private bool waitVisibility1;

        public bool waitVisibility { get => waitVisibility1; set => SetProperty(ref waitVisibility1, value); }

        private double prgs1;

        public double prgs { get => prgs1; set => SetProperty(ref prgs1, value); }

        private string prgsPercent1;

        public string prgsPercent { get => prgsPercent1; set => SetProperty(ref prgsPercent1, value); }

        private bool sndAgainEnabled1;

        public bool sndAgainEnabled { get => sndAgainEnabled1; set => SetProperty(ref sndAgainEnabled1, value); }

        private string sendAgainText1;

        public string sendAgainText { get => sendAgainText1; set => SetProperty(ref sendAgainText1, value); }

        private Color sendAgainBack1;

        public Color sendAgainBack { get => sendAgainBack1; set => SetProperty(ref sendAgainBack1, value); }

        private Command sendAgainBtn1;

        public ICommand sendAgainBtn
        {
            get
            {
                if (sendAgainBtn1 == null)
                {
                    sendAgainBtn1 = new Command(PerformsendAgainBtn);
                }

                return sendAgainBtn1;
            }
        }

        private Command goToTermConditionPgae1;

        public ICommand goToTermConditionPgae
        {
            get
            {
                if (goToTermConditionPgae1 == null)
                {
                    goToTermConditionPgae1 = new Command( async => PerformgoToTermConditionPgae());
                }

                return goToTermConditionPgae1;
            }
        }

        

        #endregion

    }
}
