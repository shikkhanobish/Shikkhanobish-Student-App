using Flurl.Http;
using Microsoft.AspNetCore.SignalR.Client;
using Plugin.LocalNotification;
using ShikkhanobishStudentApp.Model;
using ShikkhanobishStudentApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Vonage;
using XF.Material.Forms.UI.Dialogs;

namespace ShikkhanobishStudentApp.ViewModel
{
    public class TakeTuitionViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private string isNewUpdate;

        //public ServerConnection serverconnection { get; set; }
        private ObservableCollection<ClassInfo> AllclsList = new ObservableCollection<ClassInfo>();

        private ObservableCollection<UniversityName> AllUNameList = new ObservableCollection<UniversityName>();
        private ObservableCollection<Subject> AllsubList = new ObservableCollection<Subject>();
        private ObservableCollection<Degree> AlldegreeList = new ObservableCollection<Degree>();
        private ObservableCollection<Chapter> AllchpList = new ObservableCollection<Chapter>();
        private ObservableCollection<Course> AllCrsList = new ObservableCollection<Course>();
        private List<favouriteTeacher> thisfavteacher = new List<favouriteTeacher>();
        private RealTimeApiMethods realtimeapi = new RealTimeApiMethods();
        int SelectedSubjectID;
        private bool isPremiumRechurge;
        private CostClass Allcost = new CostClass();
        private int thisSearcherSubId;
        private Teacher thisSelectedFavPopUpTeacher = new Teacher();
        private HubConnection _connection = null;
        private Teacher SelectedTeacher = new Teacher();
        private string url = "https://shikkhanobishRealTimeAPi.shikkhanobish.com/ShikkhanobishHub";
        private int rechargeCoinAMountInt, rechargeTTakaAmountInt;
        private StudentPaymentHistory thispayment = new StudentPaymentHistory();
        public Voucher thisUsedVoucher { get; set; }
        private int teacherisSelected;
        private List<Voucher> allVoucher = new List<Voucher>();
        private bool teacherGaveResponse = true;
        public bool isCancledTuition;
        int selectedChapterID;

        #region Methods

        public TakeTuitionViewModel(bool fromReg)
        {
            homeFirst(fromReg);
        }

        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        private void PerformshowAddCoin()
        {
            showAddCoinColor = Color.FromHex("#23D885");
            showVoucherColor = Color.FromHex("#F7F7F7");
            showAddCoinTxtColor = Color.White;
            showVoucherTxtColor = Color.Black;
            showOffervisibility = false;
            showAddCoinvisibility = true;
        }

        private void PerformshowVouchers()
        {
            showVoucherColor = Color.FromHex("#2392D8");
            showAddCoinColor = Color.FromHex("#F7F7F7");
            showAddCoinTxtColor = Color.Black;
            showVoucherTxtColor = Color.White;
            showOffervisibility = true;
            showAddCoinvisibility = false;
        }

        public async Task homeFirst(bool fromReg)
        {
            isTuitionLive = false;
            isAnimationStart = false;
            PerformshowAddCoin();
            isCancledTuition = false;
            rechargeCoinBackVisibility = false;
            isLoading = false;
            isNewUpdate = "";
            proMsgBtnIsVisible = false;
            regMsgVisiblity = false;
            paymentGifGrid = false;
            SucPaymentText = "";
            prmStudentTextVisibility = false;
            acceptTeacherVisibility = false;
            selectedTeacherConnectingVisibility = false;
            prmStudentTextVisibility = false;
            hireteacherEnabled = false;
            groupChoiseVisibility = false;
            popUpVisibility = false;
            SelectedInsName = "Not Selected";
            SelectedClassName = "Not Selected";
            SelectedSubjectName = "Not Selected";
            SelectedChapterName = "Not Selected";

            seletedCountTextVisibility = false;
            CLseletedCountTextVisibility = false;
            SubseletedCountTextVisibility = false;
            ChpseletedCountTextVisibility = false;

            activebtn = true;
            chooseTeacherEnabled = true;
            permincostVisibility = true;
            remainColopr = "Black";

            remainword = "Remain 300 Words";

            remainColopr = "#E5E5E5";
            secTitle = "Class";
            thirdTitle = "Subject";
            forthTitle = "Chapter";
            firstListBtnVisibility = true;
            secondListBtnVisibility = false;
            thirdListBtnVisibility = false;
            forthBtnVisbility = false;
            resultprgs = .1;
            resultvisi = true;
            rechargeButtonVisibility = false;

            using (var dialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Please Wait..."))
            {
                await ConnectToRealTimeApiServer().ConfigureAwait(false);
                await GetAllCost().ConfigureAwait(false);
                await GetVoucher().ConfigureAwait(false);
                await GetProMsg(fromReg).ConfigureAwait(false);
                await getALlFavTeacher().ConfigureAwait(false);
                await GetALlReport();
                avaiableCoin = StaticPageToPassData.thisStudentInfo.coin + "";
                freeMinText = StaticPageToPassData.thisStudentInfo.freemin + "";
                await GetPromotImage();
                await GetThisStChoice();
                await GetLogList();
                isLoading = false;
            }
            randonpopupTeacherbtnColor = Color.FromHex("#ECECEC");
            if (fromReg)
            {
                await MaterialDialog.Instance.AlertAsync(message: "শিক্ষানবিশে তোমাকে অভিন্দন! শিক্ষানবিশ এমন একটি প্লাটফর্ম যেখানে তুমি যখন ইচ্ছা এবং যে সাবজেক্ট ইচ্ছা সে সাবজেক্ট টিচারদের কাছ থেকে শিখে নিতে পারবে। জাস্ট তোমার প্রশ্নটি লিখ, প্রশ্নটি কোন সাবজেক্টের এবং চাপ্টারের তা সিলেক্ট কর এবং হায়ার টিচার বাটনে ক্লিক করে এখনি শুরু কর পড়াশোনা মোবাইলেই!");
            }
            isAnimationStart = true;
        }

        #region Methods
        public async Task GetLogList()
        {
            var lList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTuiTionLogNeW".GetJsonAsync<List<TuiTionLog>>();
            List<TuiTionLog> thisTuition = new List<TuiTionLog>();
            foreach (var tuition in lList)
            {
                if (tuition.studentID == StaticPageToPassData.thisStudentInfo.studentID)
                {
                    isTuitionLive = true;
                    thisTuition.Add(tuition);
                }
            }
            livetuitionList = thisTuition;
        }
        private void PerformliveTuitionViewCmd()
        {
            Application.Current.MainPage.Navigation.PushAsync(new LiveTuitionList());
        }
        public async Task GetThisStChoice()
        {
            var thischoice = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getClassChoiceWithID".PostUrlEncodedAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID })
                 .ReceiveJson<ClassChoice>();
            await InsListPopulate();
            await ClassListPopulate();
            for (int i = 0; i < backUpFipName.Count; i++)
            {
                if (backUpFipName[i].institutionID == thischoice.institutionID)
                {
                    popupList thispop = new popupList();
                    thispop.name = backUpFipName[i].name;
                    thispop.ListIndex = 1;
                    thispop.totalRequest = backUpFipName[i].tuitionRequest;
                    await SelectInsInContructor(thispop);
                    break;
                }
            }
            for (int i = 0; i < AllclsList.Count; i++)
            {
                if (AllclsList[i].classID == thischoice.classID)
                {
                    popupList thispop = new popupList();
                    thispop.name = AllclsList[i].name;
                    thispop.ListIndex = AllclsList[i].indexNo;
                    thispop.totalRequest = AllclsList[i].tuitionRequest;
                    await SelectClasInConstructor(thispop);
                    break;
                }
            }
        }

        public async Task GetALlReport()
        {
            var regRes = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getStudentReportWithStudentID"
              .PostUrlEncodedAsync(new
              {
                  studentID = StaticPageToPassData.thisStudentInfo.studentID
              })
              .ReceiveJson<List<StudentReport>>();
            reportCount = "" + regRes.Count;
        }

        public async Task PerformshowSuggestion()
        {
            await MaterialDialog.Instance.AlertAsync(message: "এই সেকশনে আপনি কি শিখতে চাচ্ছেন তা নিয়ে কিছু লিখবেন অথবা আপনার প্রশ্ন থাকলে তা সরাসরি করবেন। যেমনঃ \"আমি পদার্থ বিজ্ঞানের টর্ক টপিকটা ভাল করে বুঝতে চাই।\" অথবা \"ব্যবসায় উদ্যোগের ব্যবসায় পরিকল্পনা চাপ্টারটি বুঝতে চাই\"",
                                    title: "কি লিখবেন এইখানে?");
        }

        public async Task GetAllCost()
        {
            Allcost = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/GetCost".GetJsonAsync<CostClass>();
        }

        public async Task GetVoucher()
        {
            allVoucher = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getVoucher".GetJsonAsync<List<Voucher>>();
            for (int i = 0; i < allVoucher.Count; i++)
            {
                if (i == 0)
                {
                    if (allVoucher[i].type == 0)
                    {
                        offertxt1 = "Recharge " + allVoucher[i].amountTaka + " coin with " + allVoucher[i].amountTaka + " taka and get " + allVoucher[i].getAmount + " coin free!";
                    }
                    if (allVoucher[i].type == 1)
                    {
                        offertxt1 = "Recharge " + allVoucher[i].amountTaka + " coin with " + allVoucher[i].amountTaka + " taka and get " + allVoucher[i].getAmount + " minutes free!";
                    }
                }
                if (i == 1)
                {
                    if (allVoucher[i].type == 0)
                    {
                        offertxt2 = "Recharge " + allVoucher[i].amountTaka + " coin with " + allVoucher[i].amountTaka + " taka and get " + allVoucher[i].getAmount + " coin free!";
                    }
                    if (allVoucher[i].type == 1)
                    {
                        offertxt2 = "Recharge " + allVoucher[i].amountTaka + " coin with " + allVoucher[i].amountTaka + " taka and get " + allVoucher[i].getAmount + " minutes free!";
                    }
                }
                if (i == 2)
                {
                    if (allVoucher[i].type == 0)
                    {
                        offertxt3 = "Recharge " + allVoucher[i].amountTaka + " coin with " + allVoucher[i].amountTaka + " taka and get " + allVoucher[i].getAmount + " coin free!";
                    }
                    if (allVoucher[i].type == 1)
                    {
                        offertxt3 = "Recharge " + allVoucher[i].amountTaka + " coin with " + allVoucher[i].amountTaka + " taka and get " + allVoucher[i].getAmount + " minutes free!";
                    }
                }
                if (i == 3)
                {
                    if (allVoucher[i].type == 0)
                    {
                        offertxt4 = "Recharge " + allVoucher[i].amountTaka + " coin with " + allVoucher[i].amountTaka + " taka and get " + allVoucher[i].getAmount + " coin free!";
                    }
                    if (allVoucher[i].type == 1)
                    {
                        offertxt4 = "Recharge " + allVoucher[i].amountTaka + " coin with " + allVoucher[i].amountTaka + " taka and get " + allVoucher[i].getAmount + " minutes free!";
                    }
                }
                if (i == 4)
                {
                    if (allVoucher[i].type == 0)
                    {
                        offertxt5 = "Recharge " + allVoucher[i].amountTaka + " coin with " + allVoucher[i].amountTaka + " taka and get " + allVoucher[i].getAmount + " coin free!";
                    }
                    if (allVoucher[i].type == 1)
                    {
                        offertxt5 = "Recharge " + allVoucher[i].amountTaka + " coin with " + allVoucher[i].amountTaka + " taka and get " + allVoucher[i].getAmount + " minutes free!";
                    }
                }
            }
        }

        private void PerformpopOutRegMsgVisiblility()
        {
            if (isNewUpdate == "")
            {
                regMsgVisiblity = false;
            }
            else
            {
                url = isNewUpdate;
                Browser.OpenAsync(url, BrowserLaunchMode.External);
            }
        }

        private void PerformpopUPRegMsgVisiblility()
        {
            regMsgVisiblity = true;
        }

        public async Task GetProMsg(bool fromReg)
        {
            var promsg = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/GetPromotionalMassage".GetJsonAsync<List<PromotionalMassage>>();
            for (int i = 0; i < promsg.Count; i++)
            {
                if (promsg[i].userType == "student")
                {
                    if (fromReg)
                    {
                        isNewUpdate = "";
                        PerformpopUPRegMsgVisiblility();
                        promsgImgSrc = promsg[i].imageSrc;
                        proMsgText = promsg[i].text;
                        break;
                    }
                    if (promsg[i].msgType == 2)
                    {
                        isNewUpdate = "";
                        PerformpopUPRegMsgVisiblility();
                        promsgImgSrc = promsg[i].imageSrc;
                        proMsgText = promsg[i].text;
                        break;
                    }
                    if (promsg[i].msgType == 3)
                    {
                        isNewUpdate = promsg[i].playstoreAppLink;
                        PerformpopUPRegMsgVisiblility();
                        promsgImgSrc = promsg[i].imageSrc;
                        proMsgText = promsg[i].text;
                        break;
                    }
                }
            }
        }

        public async Task GetPromotImage()
        {
            var allPromoImage = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/GetPromotionalImage".GetJsonAsync<List<PromotionalImage>>();
            offerList = allPromoImage;
        }

        public async Task CalCulateReachrgeCost()
        {
            for (int i = 0; i < allVoucher.Count; i++)
            {
                if (int.Parse(rechargeAmount) == allVoucher[i].amountTaka)
                {
                    if (allVoucher[i].type == 0)
                    {
                        totalRechargeCoin = rechargeAmount;
                        addedCoinamount = " + " + allVoucher[i].getAmount.ToString();
                        thisUsedVoucher = allVoucher[i];
                        rechargeCoinAMountInt = allVoucher[i].getAmount + int.Parse(rechargeAmount);
                        rechargeTTakaAmountInt = int.Parse(rechargeAmount);
                        totalAmount = rechargeAmount + " Taka";
                        freeminInaddCoinScreen = 0 + "";
                        thispayment.addedMin = 0;
                        break;
                    }
                    if (allVoucher[i].type == 1)
                    {
                        totalRechargeCoin = rechargeAmount;
                        addedCoinamount = "";
                        thisUsedVoucher = allVoucher[i];
                        rechargeCoinAMountInt = int.Parse(rechargeAmount);
                        rechargeTTakaAmountInt = int.Parse(rechargeAmount);
                        totalAmount = rechargeAmount + " Taka";
                        freeminInaddCoinScreen = allVoucher[i].getAmount + "";
                        thispayment.addedMin = allVoucher[i].getAmount;
                        break;
                    }
                }
                else
                {
                    totalRechargeCoin = rechargeAmount;
                    addedCoinamount = "";
                    thisUsedVoucher = new Voucher();
                    rechargeCoinAMountInt = int.Parse(rechargeAmount);
                    rechargeTTakaAmountInt = int.Parse(rechargeAmount);
                    totalAmount = rechargeAmount + " Taka";
                    freeminInaddCoinScreen = 0 + "";
                }
            }
        }

        public bool checkInternet()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task PerformlogoutAsync()
        {
            SecureStorage.RemoveAll();
            _connection.StopAsync();
            if (StaticPageToPassData.isFromLogin)
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            else
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new LoginPage());
                var existingPages = Application.Current.MainPage.Navigation.ModalStack.ToList();
                foreach (var page in existingPages)
                {
                    Application.Current.MainPage.Navigation.RemovePage(page);
                }
            }
        }

        public void CheckEverythign()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (detailTxt != null)
                {
                    if (SelectedInsName != "Not Selected" & SelectedClassName != "Not Selected" & SelectedSubjectName != "Not Selected" & SelectedChapterName != "Not Selected" && detailTxt.Length < 300 && detailTxt != "" && detailTxt != null)
                    {
                        if (StaticPageToPassData.thisStudentInfo.coin < 3 && StaticPageToPassData.thisStudentInfo.freemin == 0)
                        {
                            perminCostText = "Insufficient Balance!";
                        }
                        else
                        {
                            if (StaticPageToPassData.thisStudentInfo.freemin > 0)
                            {
                                perminCostText = "Cost: Free!";
                            }
                            else
                            {
                                if (selectedInsName == "School")
                                {
                                    perminCostText = Allcost.SchoolCost + " coin/min";
                                }
                                if (SelectedInsName == "College")
                                {
                                    perminCostText = Allcost.CollegeCost + " coin/min";
                                }
                            }
                        }
                    }
                }
            });
        }

        public void countWord()
        {
            if (detailTxt.Length > 300)
            {
                remainword = "Extra " + (300 - detailTxt.Length) * (-1) + " Words";
                remainColopr = "Red";
            }
            else
            {
                remainword = "Remain " + (300 - detailTxt.Length) + " Words";
                remainColopr = "Black";
                CheckEverythign();
            }
        }

        private async Task PerformrechargeCoin()
        {
            string thisAMount = rechargeAmount;
            totalRechargeCoin = "";
            addedCoinamount = "";
            totalAmount = "";
            freeminInaddCoinScreen = "";
            rechargeAmount = "";

            rechargeCoinBackVisibility = false;
            rechargeButtonVisibility = false;
            isPremiumRechurge = false;
            isLoading = true;
            await Task.Delay(1000);
            RechargeerrorTxt = "";
            try
            {
                thispayment.studentID = StaticPageToPassData.thisStudentInfo.studentID;
                thispayment.amountTaka = rechargeTTakaAmountInt;
                if (thisUsedVoucher.voucherID != 0)
                {
                    thispayment.isVoucherUsed = 1;
                    thispayment.voucherID = thisUsedVoucher.voucherID;
                    thispayment.amountCoin = rechargeCoinAMountInt;
                }
                else
                {
                    thispayment.isVoucherUsed = 0;
                    thispayment.voucherID = 0;
                    thispayment.amountCoin = rechargeCoinAMountInt;
                }
                thispayment.date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                if (thisUsedVoucher.name != null)
                {
                    thispayment.name = thisUsedVoucher.name;
                }
                else
                {
                    thispayment.name = "N/A";
                }

                var redirectURL = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/RequestPayment".PostUrlEncodedAsync(new
                {
                    name = StaticPageToPassData.thisStudentInfo.name,
                    amount = int.Parse(thisAMount),
                    studentID = StaticPageToPassData.thisStudentInfo.studentID,
                    phonenumber = StaticPageToPassData.thisStudentInfo.phonenumber,
                })
   .ReceiveJson<string>();
                thispayment.type = 0;
                await Application.Current.MainPage.Navigation.PushModalAsync(new PaymentView(redirectURL));
            }
            catch (Exception ex)
            {
                RechargeerrorTxt = ex.Message;
            }
            isLoading = false;
        }

        private async Task PerformpremiumStudentBtn()
        {
            isPremiumRechurge = true;
            isLoading = true;
            await Task.Delay(1000);
            thispayment.studentID = StaticPageToPassData.thisStudentInfo.studentID;
            thispayment.amountTaka = prStudentBuyingAMount;
            thispayment.isVoucherUsed = 0;
            thispayment.voucherID = 0;
            thispayment.amountCoin = 0;
            thispayment.addedMin = 0;
            thispayment.date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            thispayment.name = "N/A";

            var redirectURL = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/RequestPayment".PostUrlEncodedAsync(new
            {
                name = StaticPageToPassData.thisStudentInfo.name,
                amount = prStudentBuyingAMount,
                studentID = StaticPageToPassData.thisStudentInfo.studentID,
                phonenumber = StaticPageToPassData.thisStudentInfo.phonenumber,
            })
            .ReceiveJson<string>();
            thispayment.type = 1;
            await Application.Current.MainPage.Navigation.PushModalAsync(new PaymentView(redirectURL));

            isLoading = false;
        }

        #endregion Methods

        #region Favteacher PopUp

        public ICommand ChooseTeacherPopUp =>
             new Command(async () =>
             {
                 using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Please Wait..."))
                 {
                    
                     if (detailTxt != null)
                     {
                         if (detailTxt.Length > 300 && detailTxt == "" && detailTxt == null)
                         {
                             await MaterialDialog.Instance.AlertAsync(message: "Write your question in the box");
                         }
                         else if (SelectedInsName == "Not Selected" & SelectedClassName == "Not Selected" & SelectedSubjectName == "Not Selected" & SelectedChapterName == "Not Selected")
                         {
                             await MaterialDialog.Instance.AlertAsync(message: "Please, select up Institution, Class, Subject, Chapter and write your question in the box");
                         }
                         else
                         {
                             var TeacherCourseList = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getAllCousrList".PostUrlEncodedAsync(new { }).ReceiveJson<List<CousrList>>();
                             var allTeacher = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getAllTeacher".PostUrlEncodedAsync(new { })
                                         .ReceiveJson<List<Teacher>>();
                             int numOFActiveTeacher = 0;
                             for (int i = 0; i < allTeacher.Count; i++)
                             {
                                 if (allTeacher[i].activeStatus == 1)
                                 {
                                     for (int j = 0; j < TeacherCourseList.Count; j++)
                                     {
                                         if (allTeacher[i].teacherID == TeacherCourseList[j].teacherID)
                                         {
                                             if (TeacherCourseList[j].sub1 == thisSearcherSubId)
                                             {
                                                 numOFActiveTeacher++;
                                             }
                                             else if (TeacherCourseList[j].sub1 == thisSearcherSubId)
                                             {
                                                 numOFActiveTeacher++;
                                             }
                                             else if (TeacherCourseList[j].sub2 == thisSearcherSubId)
                                             {
                                                 numOFActiveTeacher++;
                                             }
                                             else if (TeacherCourseList[j].sub3 == thisSearcherSubId)
                                             {
                                                 numOFActiveTeacher++;
                                             }
                                             else if (TeacherCourseList[j].sub4 == thisSearcherSubId)
                                             {
                                                 numOFActiveTeacher++;
                                             }
                                             else if (TeacherCourseList[j].sub5 == thisSearcherSubId)
                                             {
                                                 numOFActiveTeacher++;
                                             }
                                             else if (TeacherCourseList[j].sub6 == thisSearcherSubId)
                                             {
                                                 numOFActiveTeacher++;
                                             }
                                             else if (TeacherCourseList[j].sub7 == thisSearcherSubId)
                                             {
                                                 numOFActiveTeacher++;
                                             }
                                             else if (TeacherCourseList[j].sub8 == thisSearcherSubId)
                                             {
                                                 numOFActiveTeacher++;
                                             }
                                         }
                                     }
                                 }
                             }
                             totalActiveTeacher = "Active Teacher In This Subject: " + +numOFActiveTeacher;
                             chooseTeacherVisibility = true;
                             hireteacherPopupVisibility = true;
                             hireteacherEnabled = false;
                             CheckFavTeacherAvailable();
                         }
                     }
                     else
                     {
                         await MaterialDialog.Instance.AlertAsync(message: "Write your question in the box");
                     }
                 }
             });

        public ICommand acceptTeacherTuition =>
            new Command<int>(async (i) =>
            {
                if (i == 1)
                {
                    if (!CrossVonage.Current.TryStartSession())
                    {
                        return;
                    }
                    var actions = new string[] { "Audio Call", "Video Call" };
                    var result = await MaterialDialog.Instance.SelectActionAsync(title: "Do you want to use?[You can change it in call]",
                                                                     actions: actions);
                    if (result == 0)
                    {
                        string sendResponse = "https://shikkhanobishrealtimeapi.shikkhanobish.com/api/ShikkhanobishSignalR/studentTuitionResponse?&teacherID=" + teacherisSelected + "&studentID=" + StaticPageToPassData.thisStudentInfo.studentID + "&studentTuitionResponse=" + true + "&hidestVideo=" + true;
                        await realtimeapi.ExecuteRealTimeApi(sendResponse);
                    }
                    else
                    {
                        string sendResponse = "https://shikkhanobishrealtimeapi.shikkhanobish.com/api/ShikkhanobishSignalR/studentTuitionResponse?&teacherID=" + teacherisSelected + "&studentID=" + StaticPageToPassData.thisStudentInfo.studentID + "&studentTuitionResponse=" + true + "&hidestVideo=" + false;
                        await realtimeapi.ExecuteRealTimeApi(sendResponse);
                    }
                    using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Connecting Video Call..."))
                    {
                        hireteacherPopupVisibility = false;

                        PerMinPassModel perminPass = new PerMinPassModel();
                        perminPass.studentID = StaticPageToPassData.thisStudentInfo.studentID;
                        perminPass.teacherID = teacherisSelected;
                        perminPass.time = 0;
                        perminPass.sessionID = thisSesionID;
                        perminPass.firstChoiceID = firstChoiceID + "";
                        perminPass.secondChoiceID = secondChoiceID + "";
                        perminPass.thirdChoiceID = thirdChoiceID + "";
                        perminPass.forthChoiceID = forthChoiceID + "";
                        perminPass.firstChoiceName = SelectedInsName;
                        perminPass.secondChoiceName = SelectedClassName;
                        perminPass.thirdChoiceName = SelectedSubjectName;
                        perminPass.forthChoiceName = selectedChapterName;
                        StaticPageToPassData.perMinCall = perminPass;
                        var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setPendingRatting".PostUrlEncodedAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID, tuitionID = thisSesionID })
                          .ReceiveJson<Response>();
                        Application.Current.MainPage.Navigation.PushModalAsync(new VideoCallPage());
                    }
                }
                else if (i == 2)
                {
                    isCancledTuition = true;
                    teacherisSelected = 1;
                    teacherGaveResponse = true;
                    teacheracceptTimer = "";
                    selectedTeacherConnectingVisibility = false;
                    chooseTeacherVisibility = true;
                    acceptTeacherVisibility = false;
                }
            });

        public async Task CheckFavTeacherAvailable()
        {
            int k = 0;
            while (hireteacherPopupVisibility)
            {
                List<favouriteTeacher> purefav = new List<favouriteTeacher>();
                List<favouriteTeacher> normalfav = new List<favouriteTeacher>();

                normalfav = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getFavouriteTeacherwithStudentID".PostUrlEncodedAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID })
            .ReceiveJson<List<favouriteTeacher>>();
                List<TeacherActivityStatus> pureactive = await CheckPureActive();
                for (int i = 0; i < normalfav.Count; i++)
                {
                    for (int j = 0; j < pureactive.Count; j++)
                    {
                        if (normalfav[i].teacherID == pureactive[j].teacherID)
                        {
                            purefav.Add(normalfav[i]);
                        }
                    }
                }
                //popupfavteacheritemSource.Clear();
                popupfavteacheritemSource = normalfav;
                for (int i = 0; i < popupfavteacheritemSource.Count; i++)
                {
                    popupfavteacheritemSource[i].activeStatus = "Offline";
                    popupfavteacheritemSource[i].activeColor = "Gray";
                    popupfavteacheritemSource[i].teacherRatting = Math.Round(popupfavteacheritemSource[i].teacherRatting, 2);
                    if (thisSelectedFavPopUpTeacher.teacherID == popupfavteacheritemSource[i].teacherID)
                    {
                        bool isOnline = false;
                        for (int j = 0; j < purefav.Count; j++)
                        {
                            if (thisSelectedFavPopUpTeacher.teacherID == purefav[j].teacherID)
                            {
                                popupfavteacheritemSource[i].popupfavSelectedbackground = "#CFBFFF";
                                isOnline = true;
                            }
                        }
                        if (!isOnline)
                        {
                            thisSelectedFavPopUpTeacher.teacherID = 0;
                            await Task.Delay(1000);
                            if (chooseTeacherVisibility)
                            {
                                await MaterialDialog.Instance.AlertAsync(message: popupfavteacheritemSource[i].teacherName + " is not online! Please select another teacher");
                            }
                        }
                    }
                }
                for (int i = 0; i < popupfavteacheritemSource.Count; i++)
                {
                    for (int j = 0; j < pureactive.Count; j++)
                    {
                        if (popupfavteacheritemSource[i].teacherID == pureactive[j].teacherID)
                        {
                            popupfavteacheritemSource[i].activeStatus = "Online";
                            popupfavteacheritemSource[i].activeColor = "Green";
                        }
                    }
                }
                if (popupfavteacheritemSource.Count == 0)
                {
                    choosefavteacherlbl = false;
                    nofavteacherlbl = true;
                }
                else
                {
                    choosefavteacherlbl = true;
                    nofavteacherlbl = false;
                }
            }
        }

        private void PerformgroupChoice(string index)
        {
            int indexno = int.Parse(index);
            if (indexno == 1)
            {
                if (scChoice == Color.GreenYellow)
                {
                    scChoice = Color.Transparent;
                    ObservableCollection<Subject> popupclsList = new ObservableCollection<Subject>();
                    for (int i = 0; i < AllsubList.Count; i++)
                    {
                        if (AllsubList[i].classID == selectedGlobalCls.classID)
                        {
                            popupclsList.Add(AllsubList[i]);
                        }
                    }
                    groupChoiseVisibility = true;
                    ObservableCollection<popupList> convertedList = new ObservableCollection<popupList>();
                    convertedList = ConvertSubTOPupUpList(popupclsList);
                    resultvisi = false;
                    popupList = convertedList;
                }
                else
                {
                    scChoice = Color.GreenYellow;
                    cmChoice = Color.Transparent;
                    arChoice = Color.Transparent;

                    ObservableCollection<Subject> popupclsList = new ObservableCollection<Subject>();
                    for (int i = 0; i < AllsubList.Count; i++)
                    {
                        if (AllsubList[i].classID == selectedGlobalCls.classID && AllsubList[i].groupName == "Science")
                        {
                            popupclsList.Add(AllsubList[i]);
                        }
                    }
                    groupChoiseVisibility = true;
                    ObservableCollection<popupList> convertedList = new ObservableCollection<popupList>();
                    convertedList = ConvertSubTOPupUpList(popupclsList);
                    resultvisi = false;
                    popupList = convertedList;
                }
            }
            if (indexno == 2)
            {
                if (cmChoice == Color.GreenYellow)
                {
                    cmChoice = Color.Transparent;
                    ObservableCollection<Subject> popupclsList = new ObservableCollection<Subject>();
                    for (int i = 0; i < AllsubList.Count; i++)
                    {
                        if (AllsubList[i].classID == selectedGlobalCls.classID)
                        {
                            popupclsList.Add(AllsubList[i]);
                        }
                    }
                    groupChoiseVisibility = true;
                    ObservableCollection<popupList> convertedList = new ObservableCollection<popupList>();
                    convertedList = ConvertSubTOPupUpList(popupclsList);
                    resultvisi = false;
                    popupList = convertedList;
                }
                else
                {
                    cmChoice = Color.GreenYellow;
                    arChoice = Color.Transparent;
                    scChoice = Color.Transparent;
                    ObservableCollection<Subject> popupclsList = new ObservableCollection<Subject>();
                    for (int i = 0; i < AllsubList.Count; i++)
                    {
                        if (AllsubList[i].classID == selectedGlobalCls.classID && AllsubList[i].groupName == "Commerce")
                        {
                            popupclsList.Add(AllsubList[i]);
                        }
                    }
                    groupChoiseVisibility = true;
                    ObservableCollection<popupList> convertedList = new ObservableCollection<popupList>();
                    convertedList = ConvertSubTOPupUpList(popupclsList);
                    resultvisi = false;
                    popupList = convertedList;
                }
            }
            if (indexno == 3)
            {
                if (arChoice == Color.GreenYellow)
                {
                    arChoice = Color.Transparent;
                    ObservableCollection<Subject> popupclsList = new ObservableCollection<Subject>();
                    for (int i = 0; i < AllsubList.Count; i++)
                    {
                        if (AllsubList[i].classID == selectedGlobalCls.classID)
                        {
                            popupclsList.Add(AllsubList[i]);
                        }
                    }
                    groupChoiseVisibility = true;
                    ObservableCollection<popupList> convertedList = new ObservableCollection<popupList>();
                    convertedList = ConvertSubTOPupUpList(popupclsList);
                    resultvisi = false;
                    popupList = convertedList;
                }
                else
                {
                    arChoice = Color.GreenYellow;
                    scChoice = Color.Transparent;
                    cmChoice = Color.Transparent;
                    ObservableCollection<Subject> popupclsList = new ObservableCollection<Subject>();
                    for (int i = 0; i < AllsubList.Count; i++)
                    {
                        if (AllsubList[i].classID == selectedGlobalCls.classID && AllsubList[i].groupName == "Arts")
                        {
                            popupclsList.Add(AllsubList[i]);
                        }
                    }
                    groupChoiseVisibility = true;
                    ObservableCollection<popupList> convertedList = new ObservableCollection<popupList>();
                    convertedList = ConvertSubTOPupUpList(popupclsList);
                    resultvisi = false;
                    popupList = convertedList;
                }
            }
        }

        private void PerformClosePopUpHireTeacher()
        {
            hireteacherPopupVisibility = false;
        }

        private async Task PerformchooserndTeachercmd()
        {
            if (randonpopupTeacherbtnColor == Color.FromHex("#5098E87F"))
            {
                randonpopupTeacherbtnColor = Color.FromHex("#ECECEC");
                hireteacherEnabled = false;
            }
            else
            {
                randonpopupTeacherbtnColor = Color.FromHex("#5098E87F");
                hireteacherEnabled = true;
                thisSelectedFavPopUpTeacher.teacherID = 0;
            }
        }

        public async Task<List<TeacherActivityStatus>> CheckPureActive()
        {
            var rightNowActiveTeacher = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getTeacherActivityStatus".GetJsonAsync<List<TeacherActivityStatus>>();
            await Task.Delay(1000);
            var AfterOneSecActiveTeacher = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getTeacherActivityStatus".GetJsonAsync<List<TeacherActivityStatus>>();

            List<TeacherActivityStatus> pureActive = new List<TeacherActivityStatus>();

            for (int i = 0; i < AfterOneSecActiveTeacher.Count; i++)
            {
                for (int j = 0; j < rightNowActiveTeacher.Count; j++)
                {
                    if (AfterOneSecActiveTeacher[i].teacherID == rightNowActiveTeacher[j].teacherID)
                    {
                        pureActive.Add(AfterOneSecActiveTeacher[i]);
                        break;
                    }
                }
            }
            return pureActive;
        }

        private async Task PerformhireTeacherBtnCmdAsync()
        {
            LTDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            LTSubject = SelectedSubjectName;
            isTuitionLive = true;
            isscTeacherInfoVisible = false;
            teacherGaveResponse = false;
            chooseTeacherVisibility = false;
            hireteacherPopupVisibility = false;
            selectedTeacherConnectingVisibility = true;
            connectingTeachertxt = "Searching Teacher...";
            Teacher thisTeacher = new Teacher();
            teacherisSelected = 0;
            bool isFavTeacher = true;

            string tID = StaticPageToPassData.GenarateIDString(40);
            var rsdes = "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setTuiTionLog".PostUrlEncodedAsync(new
            {
                studentName = StaticPageToPassData.thisStudentInfo.name,
                subjectname = SelectedSubjectName,
                tuitionLogID = tID,
                description = detailTxt,
                date = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"),
                subjectID = SelectedSubjectID,
                studentID = StaticPageToPassData.thisStudentInfo.studentID,
                chapterID = selectedChapterID,
                chapterName = SelectedChapterName

            }).ReceiveJson<Response>();
            var lList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTuiTionLogNeW".GetJsonAsync<List<TuiTionLog>>();
            List<TuiTionLog> thisList = new List<TuiTionLog>();
            foreach(var tuition in lList)
            {
                if(tuition.studentID == StaticPageToPassData.thisStudentInfo.studentID)
                {
                    thisList.Add(tuition);
                   
                }
            }
            livetuitionList = thisList;
            string sendTeachernotification = "https://shikkhanobishrealtimeapi.shikkhanobish.com/api/ShikkhanobishSignalR/realTimetuitionNotiofication?&tuitionid=" + tID;
            await realtimeapi.ExecuteRealTimeApi(sendTeachernotification);
            // //select Teacher
            // while (teacherisSelected == 0)
            // {
            //     if (thisSelectedFavPopUpTeacher.teacherID == 0)
            //     {
            //         SelectedTeacher = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/HireTeacherAsync".PostUrlEncodedAsync(new { subID = thisSearcherSubId })
            //.ReceiveJson<Teacher>();
            //         teacherisSelected = SelectedTeacher.teacherID;
            //         thisTeacher = SelectedTeacher;
            //         isFavTeacher = false;
            //     }
            //     else
            //     {
            //         teacherisSelected = thisSelectedFavPopUpTeacher.teacherID;
            //         thisTeacher = thisSelectedFavPopUpTeacher;
            //         isFavTeacher = true;
            //     }
            // }
            // isSearchGifVisible = false;
            // isscTeacherInfoVisible = true;
            // connectingTeachertxt = "Teacher Found! Waiting for response...";
            // //Call teacher
            // int thisCost = thisTuitionCostCal();
            // Teacher thisscTeacherInfo = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getTeacherWithID".PostUrlEncodedAsync(new { teacherID = teacherisSelected }).ReceiveJson<Teacher>();
            // scteacherName = thisscTeacherInfo.name;
            // scteacherTotalTuition = thisscTeacherInfo.totalTuition + "";
            // float toalRating = 0;

            // float totalSum = thisscTeacherInfo.fiveStar * 5 + thisscTeacherInfo.fourStar * 4 + thisscTeacherInfo.threeStar * 3 + thisscTeacherInfo.twoStar * 2 + thisscTeacherInfo.oneStar;

            // toalRating = totalSum / (thisscTeacherInfo.fiveStar + thisscTeacherInfo.fourStar + thisscTeacherInfo.threeStar + thisscTeacherInfo.twoStar + thisscTeacherInfo.oneStar);
            // scteacherRatting = toalRating + "";
            // string uriToCAllTeacher = "https://shikkhanobishrealtimeapi.shikkhanobish.com/api/ShikkhanobishSignalR/CallSelectedTeacher?&teacherID=" + teacherisSelected + "&des=" + detailTxt + "&cls=" + SelectedClassName + "&sub=" + thisSearcherSubId + "&chapter=" + selectedChapterName + "&cost=" + thisCost + "&name=" + StaticPageToPassData.thisStudentInfo.name + "&studentID=" + StaticPageToPassData.thisStudentInfo.studentID;
            // await realtimeapi.ExecuteRealTimeApi(uriToCAllTeacher);
            // int sec = 35;

            // Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            // {
            //     if (sec == 0)
            //     {
            //         isSearchGifVisible = true;
            //         isscTeacherInfoVisible = false;
            //         if (isFavTeacher)
            //         {
            //             connectingTeachertxt = "Teacher is unable to connect. Please, choose another teacher.";
            //             selectedTeacherConnectingVisibility = false;
            //             chooseTeacherVisibility = true;
            //             Task.Delay(1000);
            //             teacherisSelected = thisSelectedFavPopUpTeacher.teacherID;
            //             return false;
            //         }
            //         else
            //         {
            //             if (!isCancledTuition)
            //             {
            //                 CallTeacherAgain();
            //             }

            //             return false;
            //         }
            //     }
            //     sec -= 1;
            //     return true;
            // });
        }

        public async Task CallTeacherAgain()
        {
            int thisCost;
            string uriToCAllTeacher;
            teacherisSelected = 0;
            //select Teacher
            while (teacherisSelected == 0)
            {
                if (thisSelectedFavPopUpTeacher.teacherID == 0)
                {
                    SelectedTeacher = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/HireTeacherAsync".PostUrlEncodedAsync(new { subID = thisSearcherSubId })
                    .ReceiveJson<Teacher>();
                    teacherisSelected = SelectedTeacher.teacherID;
                }
                else
                {
                    teacherisSelected = thisSelectedFavPopUpTeacher.teacherID;
                }
            }

            if (selectedInsName == "School")
            {
                thisCost = Allcost.SchoolCost;
            }
            if (SelectedInsName == "College")
            {
                thisCost = Allcost.CollegeCost;
            }
            connectingTeachertxt = "Teacher Found! Waiting for response...";
            uriToCAllTeacher = "https://shikkhanobishrealtimeapi.shikkhanobish.com/api/ShikkhanobishSignalR/CallSelectedTeacher?&teacherID=" + teacherisSelected + "&des=" + detailTxt + "&cls=" + SelectedClassName + "&sub=" + thisSearcherSubId + "&chapter=" + selectedChapterName + "&cost=" + thisTuitionCostCal() + "&name=" + StaticPageToPassData.thisStudentInfo.name + "&studentID=" + StaticPageToPassData.thisStudentInfo.studentID;
            await realtimeapi.ExecuteRealTimeApi(uriToCAllTeacher);
        }

        public int thisTuitionCostCal()
        {
            int thisCost = 0;
            if (selectedInsName == "School")
            {
                thisCost = Allcost.SchoolCost;
            }
            if (SelectedInsName == "College")
            {
                thisCost = Allcost.CollegeCost;
            }
            return thisCost;
        }

        private async Task PerformcancleTeacherSearch()
        {
            var actions = new string[] { "Yes", "No" };
            var result = await MaterialDialog.Instance.SelectActionAsync(title: "Do you want to cancle tuition request?",
                                                             actions: actions);
            if(result == 0)
            {
                isTuitionLive = false;
                var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/deleteTuitionLog".PostUrlEncodedAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID }).ReceiveJson<Response>();
            }
            
        }

        public async Task requestPermission()
        {
            var status = await Permissions.RequestAsync<Permissions.Camera>();
        }

        public async Task ConnectToRealTimeApiServer()
        {
            _connection = new HubConnectionBuilder()
                 .WithUrl("https://shikkhanobishRealTimeAPi.shikkhanobish.com/ShikkhanobishHub")
                 .Build();
            try
            {
                await _connection.StartAsync();
            }
            catch (Exception ex)
            {
                var ss = ex.InnerException;
            }

            _connection.Closed += async (s) =>
            {
                await _connection.StartAsync();
            };

            _connection.On<int, int, bool, int, string, string>("SelectedTeacherResponse", async (teacherID, studentID, response, apikey, sessionID, token) =>
            {
                if (teacherID == teacherisSelected && studentID == StaticPageToPassData.thisStudentInfo.studentID)
                {
                    teacherGaveResponse = true;
                    if (response == false)
                    {
                        isSearchGifVisible = true;
                        isscTeacherInfoVisible = false;
                        connectingTeachertxt = "Teacher decline your request!";
                        await Task.Delay(1000);
                        connectingTeachertxt = "Searching Teacher...";
                        int teacherisSelected;
                        if (thisSelectedFavPopUpTeacher.teacherID == 0)
                        {
                            teacherisSelected = 0;
                            //select Teacher
                            while (teacherisSelected == 0)
                            {
                                if (thisSelectedFavPopUpTeacher.teacherID == 0)
                                {
                                    await Task.Delay(2000);
                                    SelectedTeacher = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/HireTeacherAsync".PostUrlEncodedAsync(new { subID = thisSearcherSubId })
                           .ReceiveJson<Teacher>();
                                    teacherisSelected = SelectedTeacher.teacherID;
                                }
                                else
                                {
                                    teacherisSelected = thisSelectedFavPopUpTeacher.teacherID;
                                }
                            }
                            int thisCost = 0;
                            if (selectedInsName == "School")
                            {
                                thisCost = Allcost.SchoolCost;
                            }
                            if (SelectedInsName == "College")
                            {
                                thisCost = Allcost.CollegeCost;
                            }
                            connectingTeachertxt = "Teacher Found! Waiting for response...";
                            string uriToCAllTeacher = "https://shikkhanobishrealtimeapi.shikkhanobish.com/api/ShikkhanobishSignalR/CallSelectedTeacher?&teacherID=" + teacherisSelected + "&des=" + detailTxt + "&cls=" + SelectedClassName + "&sub=" + thisSearcherSubId + "&chapter=" + selectedChapterName + "&cost=" + thisTuitionCostCal() + "&name=" + StaticPageToPassData.thisStudentInfo.name + "&studentID=" + StaticPageToPassData.thisStudentInfo.studentID;
                            await realtimeapi.ExecuteRealTimeApi(uriToCAllTeacher);
                        }
                        else
                        {
                            connectingTeachertxt = "Teacher is unable to teach you. Please, choose another teacher.";
                            teacherisSelected = 0;
                            teacheracceptTimer = "";
                            chooseTeacherVisibility = true;
                            acceptTeacherVisibility = false;
                            await Task.Delay(1000);
                            selectedTeacherConnectingVisibility = false;
                            chooseTeacherVisibility = true;
                            teacherisSelected = thisSelectedFavPopUpTeacher.teacherID;
                        }
                    }
                    else
                    {
                        var thisTeacher = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getTeacherWithID".PostUrlEncodedAsync(new { teacherID = teacherID })
                           .ReceiveJson<Teacher>();
                        connectingTeachertxt = thisTeacher.name + " has been connected. Please accept call to start tuition...";
                        thisSesionID = sessionID;
                        acceptTeacherVisibility = true;
                        CrossVonage.Current.ApiKey = apikey + "";
                        CrossVonage.Current.SessionId = sessionID;
                        CrossVonage.Current.UserToken = token;
                        int sec = 30;
                        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                        {
                            if (sec == 0)
                            {
                                connectingTeachertxt = "You didnt took the tuition in time.";
                                selectedTeacherConnectingVisibility = false;
                                chooseTeacherVisibility = true;
                                teacherisSelected = thisSelectedFavPopUpTeacher.teacherID;
                                return selectedTeacherConnectingVisibility;
                                teacheracceptTimer = "";
                            };
                            teacheracceptTimer = "0 : " + sec;
                            sec -= 1;
                            return selectedTeacherConnectingVisibility;
                        });
                    }
                }
            });
            _connection.On<string, int>("acceptTuitionNotification", async (tuitionid, studentID) =>
            {
                Notification noti = new Notification();
                await noti.Show("Someone accepted your tuition request");

            });
            _connection.On<int, bool, string, string, string, string, string, string>("StudentPaymentStatus", async (studentID, successFullPayment, amount, response, paymentID, trxID, cardID, cardType) =>
             {
                 if (studentID == StaticPageToPassData.thisStudentInfo.studentID)
                 {
                     if (successFullPayment)
                     {
                         string pm = "";
                         for (int i = 0; i < amount.Length; i++)
                         {
                             if (amount[i] == '.')
                             {
                                 break;
                             }
                             pm = pm + amount[i];
                         }
                         int payamount = int.Parse(pm);
                         if (!isPremiumRechurge)
                         {
                             var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setStudentPaymentHistory".PostUrlEncodedAsync(new
                             {
                                 studentID = StaticPageToPassData.thisStudentInfo.studentID,
                                 paymentID = paymentID,
                                 date = thispayment.date,
                                 trxID = trxID,
                                 amountTaka = payamount,
                                 amountCoin = thispayment.amountCoin,
                                 medium = cardType,
                                 name = thispayment.name,
                                 isVoucherUsed = thispayment.isVoucherUsed,
                                 voucherID = thispayment.voucherID,
                                 cardID = cardID
                             })
                             .ReceiveJson<Response>();
                             if (thispayment.type == 0)
                             {
                                 var regRes = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/updateStudent"
                                 .PostUrlEncodedAsync(new
                                 {
                                     studentID = thispayment.studentID,
                                     phonenumber = StaticPageToPassData.thisStudentInfo.phonenumber,
                                     password = StaticPageToPassData.thisStudentInfo.password,
                                     totalSpent = StaticPageToPassData.thisStudentInfo.totalSpent + payamount,
                                     totalTuitionTime = StaticPageToPassData.thisStudentInfo.totalTuitionTime,
                                     coin = StaticPageToPassData.thisStudentInfo.coin + thispayment.amountCoin,
                                     freemin = StaticPageToPassData.thisStudentInfo.freemin + thispayment.addedMin,
                                     city = StaticPageToPassData.thisStudentInfo.city,
                                     name = StaticPageToPassData.thisStudentInfo.name,
                                     institutionName = "none"
                                 })
                                  .ReceiveJson<Response>();
                             }
                             else if (thispayment.type == 1)
                             {
                                 var regRes = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/makePremiumStudent"
                                 .PostUrlEncodedAsync(new
                                 {
                                     studentID = thispayment.studentID,
                                 })
                                  .ReceiveJson<Response>();
                             }

                             paymentGifGrid = true;
                             SucPaymentText = "You have successfully added " + amount + " coin in your account. Thank you for staying with us.";
                             await StaticPageToPassData.GetStudent();
                             avaiableCoin = StaticPageToPassData.thisStudentInfo.coin + "";
                             freeMinText = StaticPageToPassData.thisStudentInfo.freemin + "";
                         }
                         else
                         {
                             var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setStudentPaymentHistory".PostUrlEncodedAsync(new
                             {
                                 studentID = StaticPageToPassData.thisStudentInfo.studentID,
                                 paymentID = paymentID,
                                 date = thispayment.date,
                                 trxID = trxID,
                                 amountTaka = payamount,
                                 amountCoin = 0,
                                 medium = cardType,
                                 name = thispayment.name,
                                 isVoucherUsed = thispayment.isVoucherUsed,
                                 voucherID = thispayment.voucherID,
                                 cardID = cardID
                             })
                             .ReceiveJson<Response>();

                             var respr = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/makePremiumStudent"
                                .PostUrlEncodedAsync(new
                                {
                                    studentID = thispayment.studentID,
                                })
                                 .ReceiveJson<Response>();
                             paymentGifGrid = true;
                             SucPaymentText = "Congratulation! You have bucome Shikkhanobish Premium Student. Thank you for staying with us.";
                             await StaticPageToPassData.GetStudent();
                             avaiableCoin = StaticPageToPassData.thisStudentInfo.coin + "";
                             freeMinText = StaticPageToPassData.thisStudentInfo.freemin + "";
                             getALlFavTeacher();
                         }
                     }
                     else
                     {
                         await MaterialDialog.Instance.AlertAsync(message: response,
                                     title: "There is problem");
                     }
                 }
             });
        }

       

        private void PerformpopOUTpaymentGif()
        {
            paymentGifGrid = false;
            SucPaymentText = "";
        }

        #endregion Favteacher PopUp

        #region populate list

        public async Task InsListPopulate()
        {
            backUpFipName = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getInstitution".GetJsonAsync<ObservableCollection<Institution>>();
        }

        public async Task ClassListPopulate()
        {
            AllclsList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getClassInfo".GetJsonAsync<ObservableCollection<ClassInfo>>();
            secondListBtnVisibility = true;
        }

        public async Task UNameListPopulate()
        {
            AllUNameList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getUniversityName".GetJsonAsync<ObservableCollection<UniversityName>>();
            secondListBtnVisibility = true;
        }

        public async Task DegreeListPopulate()
        {
            AlldegreeList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getDegree".GetJsonAsync<ObservableCollection<Degree>>();
        }

        public async Task SubjectListPopulate()
        {
            AllsubList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getSubject".GetJsonAsync<ObservableCollection<Subject>>();
        }

        public async Task ChapterListPopulate()
        {
            AllchpList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getChapter".GetJsonAsync<ObservableCollection<Chapter>>();
        }

        public async Task CourseListPopulate()
        {
            AllCrsList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getCourse".GetJsonAsync<ObservableCollection<Course>>();
        }

        #endregion populate list

        #region Popup in take tuition

        //Click in select btn

        public ICommand selectInsCommand =>
            new Command<string>(async (index) =>
            {
                groupChoiseVisibility = false;
                popupList.Clear();
                popUpVisibility = true;
                if (int.Parse(index) == 0)
                {
                    if (backUpFipName == null)
                    {
                        resultvisi = true;
                    }
                    searchName = "Select Institution";
                    SearchableVisibility = false;
                    await InsListPopulate();
                    ObservableCollection<popupList> convertedList = new ObservableCollection<popupList>();
                    convertedList = ConvertInsTOPupUpList(backUpFipName);
                    resultvisi = false;
                    popupList = convertedList;
                }
                //selecting list for 2nd popup
                else if (int.Parse(index) == 1)
                {
                    if (selectedGlobalIns.institutionID == 101)
                    {
                        resultvisi = true;
                        searchName = "Select Class";
                        ObservableCollection<ClassInfo> popupclsList = new ObservableCollection<ClassInfo>();
                        for (int i = 0; i < AllclsList.Count; i++)
                        {
                            if (AllclsList[i].institutionID == selectedGlobalIns.institutionID)
                            {
                                popupclsList.Add(AllclsList[i]);
                            }
                        }
                        ObservableCollection<popupList> convertedList = new ObservableCollection<popupList>();
                        convertedList = ConvertClsTOPupUpList(popupclsList);
                        resultvisi = false;
                        popupList = convertedList;
                    }
                    else if (selectedGlobalIns.institutionID == 102)
                    {
                        resultvisi = true;
                        searchName = "Select Class";
                        ObservableCollection<ClassInfo> popupclsList = new ObservableCollection<ClassInfo>();
                        for (int i = 0; i < AllclsList.Count; i++)
                        {
                            if (AllclsList[i].institutionID == selectedGlobalIns.institutionID)
                            {
                                popupclsList.Add(AllclsList[i]);
                            }
                        }
                        ObservableCollection<popupList> convertedList = new ObservableCollection<popupList>();
                        resultvisi = false;
                        convertedList = ConvertClsTOPupUpList(popupclsList);

                        popupList = convertedList;
                    }
                    else if (selectedGlobalIns.institutionID == 103)
                    {
                        resultvisi = true;
                        searchName = "Select University";
                        ObservableCollection<popupList> convertedList = new ObservableCollection<popupList>();
                        convertedList = ConvertUniNameTOPupUpList(AllUNameList);
                        resultvisi = false;
                        popupList = convertedList;
                    }
                    SearchableVisibility = false;
                }
                //selecting list for 3rd popup
                else if (int.Parse(index) == 2)
                {
                    if (thirdTitle == "Subject")
                    {
                        arChoice = Color.Transparent;
                        scChoice = Color.Transparent;
                        cmChoice = Color.Transparent;
                        resultvisi = true;
                        searchName = "Select Subject";
                        ObservableCollection<Subject> popupclsList = new ObservableCollection<Subject>();
                        for (int i = 0; i < AllsubList.Count; i++)
                        {
                            if (AllsubList[i].classID == selectedGlobalCls.classID)
                            {
                                popupclsList.Add(AllsubList[i]);
                            }
                        }
                        if (popupclsList[0].classID == 102)
                        {
                            groupChoiseVisibility = true;
                        }
                        else
                        {
                            groupChoiseVisibility = false;
                        }
                        List<Subject> SortedList = new List<Subject>();
                        SortedList = popupclsList.OrderBy(x => x.subjectID).ToList();
                        SortedList.Reverse();
                        ObservableCollection<Subject> newsublist = new ObservableCollection<Subject>(SortedList);
                        ObservableCollection<popupList> convertedList = new ObservableCollection<popupList>();
                        convertedList = ConvertSubTOPupUpList(newsublist);
                        resultvisi = false;
                        popupList = convertedList;
                    }
                    else if (thirdTitle == "Degree")
                    {
                        resultvisi = true;
                        searchName = "Select Degree";
                        ObservableCollection<Degree> popupclsList = new ObservableCollection<Degree>();
                        for (int i = 0; i < AlldegreeList.Count; i++)
                        {
                            if (AlldegreeList[i].uniNameID == selectedGlobalUniName.uniNameID)
                            {
                                popupclsList.Add(AlldegreeList[i]);
                            }
                        }
                        ObservableCollection<popupList> convertedList = new ObservableCollection<popupList>();
                        convertedList = ConvertDegreeTOPupUpList(popupclsList);
                        resultvisi = false;
                        popupList = convertedList;
                    }
                }
                //selecting list for 4thrd popup
                else if (int.Parse(index) == 3)
                {
                    if (forthTitle == "Chapter")
                    {
                        resultvisi = true;
                        searchName = "Select Chapter";
                        ObservableCollection<Chapter> popupclsList = new ObservableCollection<Chapter>();
                        for (int i = 0; i < AllchpList.Count; i++)
                        {
                            if (AllchpList[i].classID == selectedGlobalCls.classID && AllchpList[i].subjectID == selectedGlobalSub.subjectID)
                            {
                                popupclsList.Add(AllchpList[i]);
                            }
                        }
                        List<Chapter> SortedList = new List<Chapter>();
                        SortedList = popupclsList.OrderBy(x => x.chapterID).ToList();
                        ObservableCollection<Chapter> newsublist = new ObservableCollection<Chapter>(SortedList);
                        ObservableCollection<popupList> convertedList = new ObservableCollection<popupList>();
                        convertedList = ConvertChapterTOPupUpList(newsublist);
                        resultvisi = false;
                        popupList = convertedList;
                    }
                    else if (forthTitle == "Course")
                    {
                        resultvisi = true;
                        searchName = "Select Course";
                        ObservableCollection<Course> popupclsList = new ObservableCollection<Course>();
                        for (int i = 0; i < AllCrsList.Count; i++)
                        {
                            if (AllCrsList[i].uniNameID == selectedGlobalUniName.uniNameID && AllCrsList[i].degreeID == selectedGlobalDgr.degreeID)
                            {
                                popupclsList.Add(AllCrsList[i]);
                            }
                        }
                        ObservableCollection<popupList> convertedList = new ObservableCollection<popupList>();
                        convertedList = ConvertCourseTOPupUpList(popupclsList);
                        resultvisi = false;
                        popupList = convertedList;
                    }
                }
            });

        public async Task SelectClasInConstructor(popupList thisList)
        {
            ClassInfo selectedList = new ClassInfo();
            for (int i = 0; i < AllclsList.Count; i++)
            {
                if (AllclsList[i].name == thisList.name)
                {
                    selectedList = AllclsList[i];
                    secondChoiceID = AllclsList[i].classID + "";
                }
            }

            selectedGlobalCls = selectedList;
            SelectedClassName = selectedList.name;
            CLTRequest = selectedList.tuitionRequest;
            CLavgratting = selectedList.avgRatting;
            await SubjectListPopulate();
            thirdListBtnVisibility = true;
        }

        public async Task SelectInsInContructor(popupList thisList)
        {
            groupChoiseVisibility = false;
            if (thisList.ListIndex == 1)
            {
                if (selectedGlobalCls != null || selectedGlobalUniName != null)
                {
                    resetList(1);
                }
                seletedCountTextVisibility = true;
                Institution selectedIns = new Institution();
                for (int i = 0; i < backUpFipName.Count; i++)
                {
                    if (backUpFipName[i].name == thisList.name)
                    {
                        selectedIns = backUpFipName[i];
                        firstChoiceID = backUpFipName[i].institutionID + "";
                    }
                }
                selectedGlobalIns = selectedIns;
                SelectedInsName = selectedIns.name;
                TRequest = selectedIns.tuitionRequest;
                avgratting = selectedIns.avgRatting;
                if (thisList.name == "College" || thisList.name == "School")
                {
                    secTitle = "Class";
                    thirdTitle = "Subject";
                    forthTitle = "Chapter";
                    await ClassListPopulate();
                }
                else
                {
                    secTitle = "University";
                    thirdTitle = "Degree";
                    forthTitle = "Course";
                    await UNameListPopulate();
                }
            }
        }

        //click in selected item
        public ICommand SelectedItem =>
             new Command<popupList>(async (thisList) =>
             {
                 groupChoiseVisibility = false;
                 popWaitVisiblity = true;
                 if (thisList.ListIndex == 1)
                 {
                     if (selectedGlobalCls != null || selectedGlobalUniName != null)
                     {
                         resetList(1);
                     }
                     seletedCountTextVisibility = true;
                     Institution selectedIns = new Institution();
                     for (int i = 0; i < backUpFipName.Count; i++)
                     {
                         if (backUpFipName[i].name == thisList.name)
                         {
                             selectedIns = backUpFipName[i];
                             firstChoiceID = backUpFipName[i].institutionID + "";
                         }
                     }
                     selectedGlobalIns = selectedIns;
                     SelectedInsName = selectedIns.name;
                     TRequest = selectedIns.tuitionRequest;
                     avgratting = selectedIns.avgRatting;
                     if (thisList.name == "College" || thisList.name == "School")
                     {
                         secTitle = "Class";
                         thirdTitle = "Subject";
                         forthTitle = "Chapter";
                         await ClassListPopulate();
                     }
                     else
                     {
                         secTitle = "University";
                         thirdTitle = "Degree";
                         forthTitle = "Course";
                         await UNameListPopulate();
                     }
                 }
                 else if (thisList.ListIndex == 2 || thisList.ListIndex == 3)
                 {
                     if (selectedGlobalSub != null || selectedGlobalDgr != null)
                     {
                         resetList(2);
                     }
                     CLseletedCountTextVisibility = true;
                     if (thisList.ListIndex == 2)
                     {
                         ClassInfo selectedList = new ClassInfo();
                         for (int i = 0; i < AllclsList.Count; i++)
                         {
                             if (AllclsList[i].name == thisList.name)
                             {
                                 selectedList = AllclsList[i];
                                 secondChoiceID = AllclsList[i].classID + "";
                             }
                         }

                         selectedGlobalCls = selectedList;
                         SelectedClassName = selectedList.name;
                         CLTRequest = selectedList.tuitionRequest;
                         CLavgratting = selectedList.avgRatting;
                         await SubjectListPopulate();
                         thirdListBtnVisibility = true;
                     }
                     else
                     {
                         UniversityName selectedList = new UniversityName();
                         for (int i = 0; i < AllUNameList.Count; i++)
                         {
                             if (AllUNameList[i].name == thisList.name)
                             {
                                 selectedList = AllUNameList[i];
                                 secondChoiceID = AllUNameList[i].uniNameID + "";
                             }
                         }
                         selectedGlobalUniName = selectedList;
                         SelectedClassName = selectedList.name;
                         CLTRequest = selectedList.tuitionRequest;
                         CLavgratting = selectedList.avgRatting;
                         await DegreeListPopulate();
                         thirdListBtnVisibility = true;
                     }
                 }
                 else if (thisList.ListIndex == 4)
                 {
                     if (selectedGlobalCrs != null || selectedGlobalChp != null)
                     {
                         resetList(3);
                     }
                     SubseletedCountTextVisibility = true;
                     Subject selectedList = new Subject();
                     for (int i = 0; i < AllsubList.Count; i++)
                     {
                         if (AllsubList[i].name == thisList.name && AllsubList[i].classID == selectedGlobalCls.classID)
                         {
                             selectedList = AllsubList[i];
                             thisSearcherSubId = AllsubList[i].subjectID;
                             thirdChoiceID = AllsubList[i].subjectID + "";
                         }
                     }

                     selectedGlobalSub = selectedList;
                     SelectedSubjectName = selectedList.name;
                     SelectedSubjectID = selectedList.subjectID;
                     SubTRequest = selectedList.tuitionRequest;
                     Subavgratting = selectedList.avgRatting;
                     forthBtnVisbility = true;
                     await ChapterListPopulate();
                     firstListBtnVisibility = true;
                 }
                 else if (thisList.ListIndex == 5)
                 {
                     if (selectedGlobalCrs != null || selectedGlobalChp != null)
                     {
                         resetList(3);
                     }
                     SubseletedCountTextVisibility = true;
                     Degree selectedList = new Degree();
                     for (int i = 0; i < AlldegreeList.Count; i++)
                     {
                         if (AlldegreeList[i].name == thisList.name)
                         {
                             selectedList = AlldegreeList[i];
                             thirdChoiceID = AlldegreeList[i].degreeID + "";
                         }
                     }
                     selectedGlobalDgr = selectedList;
                     SelectedSubjectName = selectedList.name;
                     SubTRequest = selectedList.tuitionRequest;
                     Subavgratting = selectedList.avgRatting;
                     forthBtnVisbility = true;
                     await CourseListPopulate();
                     firstListBtnVisibility = true;
                 }
                 else if (thisList.ListIndex == 6)
                 {
                     ChpseletedCountTextVisibility = true;
                     Chapter selectedList = new Chapter();
                     for (int i = 0; i < AllchpList.Count; i++)
                     {
                         if (AllchpList[i].name == thisList.name)
                         {
                             selectedList = AllchpList[i];
                             forthChoiceID = AllchpList[i].chapterID + "";
                         }
                     }
                     selectedGlobalChp = selectedList;
                     selectedChapterID = selectedList.chapterID;
                     SelectedChapterName = selectedList.name;
                     ChpTRequest = selectedList.tuitionRequest;
                     Chpavgratting = selectedList.avgRatting;
                 }
                 else if (thisList.ListIndex == 7)
                 {
                     ChpseletedCountTextVisibility = true;
                     Course selectedList = new Course();
                     for (int i = 0; i < AllCrsList.Count; i++)
                     {
                         if (AllCrsList[i].name == thisList.name)
                         {
                             selectedList = AllCrsList[i];
                             forthChoiceID = AllCrsList[i].courseID + "";
                         }
                     }
                     selectedGlobalCrs = selectedList;

                     SelectedChapterName = selectedList.name;
                     ChpTRequest = selectedList.tuitionRequest;
                     Chpavgratting = selectedList.avgRatting;
                 }

                 CheckEverythign();
                 popWaitVisiblity = false;
                 popUpVisibility = false;
                 searchText = "";
             });

        private void PerformClosePopUp()
        {
            popUpVisibility = false;
        }

        #endregion Popup in take tuition

        #region favourite Teacher

        private void favGrid()
        {
        }

        private int prStudentBuyingAMount = 0;

        public async Task getALlFavTeacher()
        {
            var prm = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getPremiumStudentWithID".PostUrlEncodedAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID })
  .ReceiveJson<PremiumStudent>();
            prStudentBuyingAMount = prm.buyingAmount;
            prmbuyingamount = prm.buyingAmount + " Taka";
            if (prm.studentID == 0)
            {
                makepremiumEnabled = true;
                premiumStudentVisibility = true;
                prmStudentText = "*";
                studentstatus = "Normal";
                studentstatusColor = Color.Black;
                maxnumteacher = prm.maxNumberofFavouriteTeacher;
                prmStudentTextVisibility = true;
            }
            else
            {
                makepremiumEnabled = false;
                premiumStudentVisibility = false;
                prmStudentText = "*";
                studentstatus = "Premium";
                studentstatusColor = Color.FromHex("#864AE8");
                maxnumteacher = prm.maxNumberofFavouriteTeacher;
                prmStudentTextVisibility = false;
            }

            thisfavteacher = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getFavouriteTeacherwithStudentID".PostUrlEncodedAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID })
       .ReceiveJson<List<favouriteTeacher>>();

            for (int i = 0; i < thisfavteacher.Count; i++)
            {
                thisfavteacher[i].popupfavSelectedbackground = "Transparent";
                thisfavteacher[i].teacherRatting = Math.Round(thisfavteacher[i].teacherRatting, 2);
            }
            if (thisfavteacher.Count == 0)
            {
                prmStudentTextVisibility = true;
            }
            else
            {
                prmStudentTextVisibility = false;
            }
            favteacherItemSource = thisfavteacher;
        }

        public ICommand RemoveFavTeacher
        {
            get
            {
                return new Command<favouriteTeacher>((favteacher) =>
                {
                    Remove(favteacher);
                });
            }
        }

        public ICommand SeletecFavpopupTeacher
        {
            get
            {
                return new Command<favouriteTeacher>(async (favteacher) =>
                {
                    thisSelectedFavPopUpTeacher.teacherID = favteacher.teacherID;
                    if (favteacher.activeStatus == "Online")
                    {
                        hireteacherEnabled = true;
                        randonpopupTeacherbtnColor = Color.FromHex("#ECECEC");
                    }
                });
            }
        }

        public async Task SelectFavTeacher()
        {
        }

        public async Task Remove(favouriteTeacher favteacher)
        {
            favteacherItemSource.Clear();
            var res = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/removeFavTeacherWithTeacherID".PostUrlEncodedAsync(new { teacherID = favteacher.teacherID })
     .ReceiveJson<Response>();
            favteacherItemSource = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getFavouriteTeacherwithStudentID".PostUrlEncodedAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID })
      .ReceiveJson<List<favouriteTeacher>>();
            for (int i = 0; i < favteacherItemSource.Count; i++)
            {
                favteacherItemSource[i].teacherRatting = Math.Round(favteacherItemSource[i].teacherRatting, 2);
            }
        }

        #endregion favourite Teacher

        #region Converter

        public ObservableCollection<popupList> ConvertInsTOPupUpList(ObservableCollection<Institution> insList)
        {
            ObservableCollection<popupList> popuplist = new ObservableCollection<popupList>();

            for (int i = 0; i < insList.Count; i++)
            {
                popupList popupobj = new popupList();
                popupobj.name = insList[i].name;
                popupobj.totalRequest = insList[i].tuitionRequest;
                popupobj.avgRatting = insList[i].avgRatting;
                popupobj.ListIndex = 1;
                popuplist.Add(popupobj);
            }

            return popuplist;
        }

        public ObservableCollection<popupList> ConvertClsTOPupUpList(ObservableCollection<ClassInfo> list)
        {
            ObservableCollection<popupList> popuplist = new ObservableCollection<popupList>();

            for (int i = 0; i < list.Count; i++)
            {
                popupList popupobj = new popupList();
                popupobj.name = list[i].name;

                popupobj.ListIndex = 2;
                popuplist.Add(popupobj);
            }

            return popuplist;
        }

        public ObservableCollection<popupList> ConvertUniNameTOPupUpList(ObservableCollection<UniversityName> list)
        {
            ObservableCollection<popupList> popuplist = new ObservableCollection<popupList>();

            for (int i = 0; i < list.Count; i++)
            {
                popupList popupobj = new popupList();
                popupobj.name = list[i].name;
                popupobj.totalRequest = list[i].tuitionRequest;
                popupobj.avgRatting = list[i].avgRatting;
                popupobj.ListIndex = 3;
                popuplist.Add(popupobj);
            }

            return popuplist;
        }

        public ObservableCollection<popupList> ConvertSubTOPupUpList(ObservableCollection<Subject> insList)
        {
            ObservableCollection<popupList> popuplist = new ObservableCollection<popupList>();

            for (int i = 0; i < insList.Count; i++)
            {
                popupList popupobj = new popupList();
                popupobj.name = insList[i].name;
                popupobj.totalRequest = insList[i].tuitionRequest;
                popupobj.avgRatting = insList[i].avgRatting;
                popupobj.ListIndex = 4;
                popuplist.Add(popupobj);
            }

            return popuplist;
        }

        public ObservableCollection<popupList> ConvertDegreeTOPupUpList(ObservableCollection<Degree> insList)
        {
            ObservableCollection<popupList> popuplist = new ObservableCollection<popupList>();

            for (int i = 0; i < insList.Count; i++)
            {
                popupList popupobj = new popupList();
                popupobj.name = insList[i].name;
                popupobj.totalRequest = insList[i].tuitionRequest;
                popupobj.avgRatting = insList[i].avgRatting;
                popupobj.ListIndex = 5;
                popuplist.Add(popupobj);
            }

            return popuplist;
        }

        public ObservableCollection<popupList> ConvertChapterTOPupUpList(ObservableCollection<Chapter> insList)
        {
            ObservableCollection<popupList> popuplist = new ObservableCollection<popupList>();

            for (int i = 0; i < insList.Count; i++)
            {
                popupList popupobj = new popupList();
                popupobj.name = insList[i].name;
                popupobj.totalRequest = insList[i].tuitionRequest;
                popupobj.avgRatting = insList[i].avgRatting;
                popupobj.ListIndex = 6;
                popuplist.Add(popupobj);
            }

            return popuplist;
        }

        public ObservableCollection<popupList> ConvertCourseTOPupUpList(ObservableCollection<Course> insList)
        {
            ObservableCollection<popupList> popuplist = new ObservableCollection<popupList>();

            for (int i = 0; i < insList.Count; i++)
            {
                popupList popupobj = new popupList();
                popupobj.name = insList[i].name;
                popupobj.totalRequest = insList[i].tuitionRequest;
                popupobj.avgRatting = insList[i].avgRatting;
                popupobj.ListIndex = 7;
                popuplist.Add(popupobj);
            }

            return popuplist;
        }

        #endregion Converter

        #region reset list

        public void resetSecondListlist()
        {
            selectedGlobalCls = null;
            selectedGlobalUniName = null;
            CLseletedCountTextVisibility = false;
            SelectedClassName = "Not Selected";
        }

        public void resetThisrdListlist()
        {
            selectedGlobalSub = null;
            selectedGlobalDgr = null;
            SubseletedCountTextVisibility = false;
            SelectedSubjectName = "Not Selected";
        }

        public void resetForthListlist()
        {
            selectedGlobalChp = null;
            selectedGlobalCrs = null;
            ChpseletedCountTextVisibility = false;
            SelectedChapterName = "Not Selected";
        }

        public void resetList(int i)
        {
            if (i == 1)
            {
                resetSecondListlist();
                resetThisrdListlist();
                resetForthListlist();
                thirdListBtnVisibility = false;
                forthBtnVisbility = false;
            }
            else if (i == 2)
            {
                resetThisrdListlist();
                resetForthListlist();
                forthBtnVisbility = false;
            }
            else if (i == 3)
            {
                resetForthListlist();
            }
        }

        #endregion reset list

        #endregion Methods

        #region Binding

        public Institution selectedGlobalIns { get; set; }
        public ClassInfo selectedGlobalCls { get; set; }
        public UniversityName selectedGlobalUniName { get; set; }
        public Subject selectedGlobalSub { get; set; }
        public Degree selectedGlobalDgr { get; set; }
        public Chapter selectedGlobalChp { get; set; }
        public Course selectedGlobalCrs { get; set; }
        public ObservableCollection<Institution> backUpFipName { get; set; }
        public ObservableCollection<ClassInfo> backUpScPName { get; set; }
        public ObservableCollection<popupList> backUpthrPName { get; set; }
        public ObservableCollection<popupList> backUpfrPName { get; set; }
        public ObservableCollection<popupList> thisList { get; set; }
        private bool popUpVisibility1;

        public bool popUpVisibility { get => popUpVisibility1; set => SetProperty(ref popUpVisibility1, value); }

        private ObservableCollection<popupList> _popupList = new ObservableCollection<popupList>();
        public ObservableCollection<popupList> popupList { get => _popupList; set => SetProperty(ref _popupList, value); }

        private string searchName1;

        public string searchName { get => searchName1; set => SetProperty(ref searchName1, value); }

        private bool searchableVisibility;

        public bool SearchableVisibility { get => searchableVisibility; set => SetProperty(ref searchableVisibility, value); }

        private string searchPlaceholder1;

        public string searchPlaceholder { get => searchPlaceholder1; set => SetProperty(ref searchPlaceholder1, value); }

        private string selectedInsName;

        public string SelectedInsName { get => selectedInsName; set => SetProperty(ref selectedInsName, value); }

        private string selectedClassName;

        public string SelectedClassName { get => selectedClassName; set => SetProperty(ref selectedClassName, value); }

        private string selectedSubjectName;

        public string SelectedSubjectName { get => selectedSubjectName; set => SetProperty(ref selectedSubjectName, value); }

        private string selectedChapterName;

        public string SelectedChapterName { get => selectedChapterName; set => SetProperty(ref selectedChapterName, value); }

        private string searchText1;

        public string searchText
        { get { return searchText1; } set { searchText1 = value; /*SearchControl();*/ OnPropertyChanged(); } }

        private ICommand closePopUp;

        public ICommand ClosePopUp
        {
            get
            {
                if (closePopUp == null)
                {
                    closePopUp = new Command(PerformClosePopUp);
                }

                return closePopUp;
            }
        }

        private ICommand showSuggestion1;

        public ICommand showSuggestion
        {
            get
            {
                if (showSuggestion1 == null)
                {
                    showSuggestion1 = new Command(async => PerformshowSuggestion());
                }

                return showSuggestion1;
            }
        }

        private int sCount;

        public int SCount { get => sCount; set => SetProperty(ref sCount, value); }

        private int tRequest;

        public int TRequest { get => tRequest; set => SetProperty(ref tRequest, value); }

        private double avgratting1;

        public double avgratting { get => avgratting1; set => SetProperty(ref avgratting1, value); }
        private Teacher searchedTeacher1;

        public Teacher searchedTeacher { get => searchedTeacher1; set => SetProperty(ref searchedTeacher1, value); }

        private int cLSCount;

        public int CLSCount { get => cLSCount; set => SetProperty(ref cLSCount, value); }

        private int cLTRequest;

        public int CLTRequest { get => cLTRequest; set => SetProperty(ref cLTRequest, value); }

        private float cLavgratting;

        public float CLavgratting { get => cLavgratting; set => SetProperty(ref cLavgratting, value); }

        private int subSCount;

        public int SubSCount { get => subSCount; set => SetProperty(ref subSCount, value); }

        private int subTRequest;

        public int SubTRequest { get => subTRequest; set => SetProperty(ref subTRequest, value); }

        private float subavgratting;

        public float Subavgratting { get => subavgratting; set => SetProperty(ref subavgratting, value); }

        private int chpSCount;

        public int ChpSCount { get => chpSCount; set => SetProperty(ref chpSCount, value); }

        private int chpTRequest;

        public int ChpTRequest { get => chpTRequest; set => SetProperty(ref chpTRequest, value); }

        private float chpavgratting;

        public float Chpavgratting { get => chpavgratting; set => SetProperty(ref chpavgratting, value); }

        private bool seletedCountTextVisibility1;

        public bool seletedCountTextVisibility { get => seletedCountTextVisibility1; set => SetProperty(ref seletedCountTextVisibility1, value); }

        private bool cLseletedCountTextVisibility;

        public bool CLseletedCountTextVisibility { get => cLseletedCountTextVisibility; set => SetProperty(ref cLseletedCountTextVisibility, value); }

        private bool subseletedCountTextVisibility;

        public bool SubseletedCountTextVisibility { get => subseletedCountTextVisibility; set => SetProperty(ref subseletedCountTextVisibility, value); }

        private bool chpseletedCountTextVisibility;

        public bool ChpseletedCountTextVisibility { get => chpseletedCountTextVisibility; set => SetProperty(ref chpseletedCountTextVisibility, value); }

        private bool inactivebtn;

        public bool Inactivebtn { get => inactivebtn; set => SetProperty(ref inactivebtn, value); }

        private bool activebtn1;

        public bool activebtn { get => activebtn1; set => SetProperty(ref activebtn1, value); }

        private string detailTxt1;
        public string detailTxt
        { get { return detailTxt1; } set { detailTxt1 = value; countWord(); OnPropertyChanged(); } }

        private string remainword1;
        public string remainword
        { get { return remainword1; } set { remainword1 = value; OnPropertyChanged(); } }

        private string remainColopr1;
        public string remainColopr
        { get { return remainColopr1; } set { remainColopr1 = value; OnPropertyChanged(); } }

        private bool permincostVisibility1;

        public bool permincostVisibility { get => permincostVisibility1; set => SetProperty(ref permincostVisibility1, value); }

        private string perminCostText1;

        public string perminCostText { get => perminCostText1; set => SetProperty(ref perminCostText1, value); }
        private string SecondListTitle1;

        public string SecondListTitle { get => SecondListTitle1; set => SetProperty(ref SecondListTitle1, value); }

        private List<PromotionalImage> offerList1 { get; set; }
        public List<PromotionalImage> offerList
        { get { return offerList1; } set { offerList1 = value; OnPropertyChanged(); } }

        private double tticonopacity1;

        public double tticonopacity { get => tticonopacity1; set => SetProperty(ref tticonopacity1, value); }

        private double rcopacity1;

        public double rcopacity { get => rcopacity1; set => SetProperty(ref rcopacity1, value); }

        private bool secondListBtnVisibility1;

        public bool secondListBtnVisibility { get => secondListBtnVisibility1; set => SetProperty(ref secondListBtnVisibility1, value); }

        private bool firstListBtnVisibility1;

        public bool firstListBtnVisibility { get => firstListBtnVisibility1; set => SetProperty(ref firstListBtnVisibility1, value); }

        private string secTitle1;

        public string secTitle { get => secTitle1; set => SetProperty(ref secTitle1, value); }

        private string thirdTitle1;

        public string thirdTitle { get => thirdTitle1; set => SetProperty(ref thirdTitle1, value); }

        private string forthTitle1;

        public string forthTitle { get => forthTitle1; set => SetProperty(ref forthTitle1, value); }

        private bool thirdListBtnVisibility1;

        public bool thirdListBtnVisibility { get => thirdListBtnVisibility1; set => SetProperty(ref thirdListBtnVisibility1, value); }

        private bool forthBtnVisbility1;

        public bool forthBtnVisbility { get => forthBtnVisbility1; set => SetProperty(ref forthBtnVisbility1, value); }

        private string phoneNumber;

        public string PhoneNumber { get => phoneNumber; set => SetProperty(ref phoneNumber, value); }
        private string Password1;

        public string Password { get => Password1; set => SetProperty(ref Password1, value); }

        private string errortxt1;

        public string errortxt { get => errortxt1; set => SetProperty(ref errortxt1, value); }

        private double resultprgs1;

        public double resultprgs { get => resultprgs1; set => SetProperty(ref resultprgs1, value); }

        private bool resultvisi1;

        public bool resultvisi { get => resultvisi1; set => SetProperty(ref resultvisi1, value); }

        private Command logout1;

        public ICommand logout
        {
            get
            {
                if (logout1 == null)
                {
                    logout1 = new Command(async => PerformlogoutAsync());
                }

                return logout1;
            }
        }

        private string freeMin1;

        public string freeMin { get => freeMin1; set => SetProperty(ref freeMin1, value); }

        private string avaiableCoin1;

        public string avaiableCoin { get => avaiableCoin1; set => SetProperty(ref avaiableCoin1, value); }

        private bool popWaitVisiblity1;

        public bool popWaitVisiblity { get => popWaitVisiblity1; set => SetProperty(ref popWaitVisiblity1, value); }

        private List<favouriteTeacher> favteacherItemSource1;

        public List<favouriteTeacher> favteacherItemSource { get => favteacherItemSource1; set => SetProperty(ref favteacherItemSource1, value); }

        private Command favGridCommand1;

        public ICommand favGridCommand
        {
            get
            {
                if (favGridCommand1 == null)
                {
                    favGridCommand1 = new Command(favGrid);
                }

                return favGridCommand1;
            }
        }

        private string prmStudentText1;

        public string prmStudentText { get => prmStudentText1; set => SetProperty(ref prmStudentText1, value); }

        private string studentstatus1;

        public string studentstatus { get => studentstatus1; set => SetProperty(ref studentstatus1, value); }

        private Color studentstatusColor1;

        public Color studentstatusColor { get => studentstatusColor1; set => SetProperty(ref studentstatusColor1, value); }

        private string maxnumteacher1;

        public string maxnumteacher { get => maxnumteacher1; set => SetProperty(ref maxnumteacher1, value); }

        private bool prmStudentTextVisibility1;

        public bool prmStudentTextVisibility { get => prmStudentTextVisibility1; set => SetProperty(ref prmStudentTextVisibility1, value); }

        private bool groupChoiseVisibility1;

        public bool groupChoiseVisibility { get => groupChoiseVisibility1; set => SetProperty(ref groupChoiseVisibility1, value); }

        private Color scChoice1;

        public Color scChoice { get => scChoice1; set => SetProperty(ref scChoice1, value); }

        private Color cmChoice1;

        public Color cmChoice { get => cmChoice1; set => SetProperty(ref cmChoice1, value); }

        private Color arChoice1;

        public Color arChoice { get => arChoice1; set => SetProperty(ref arChoice1, value); }

        private Command groupChoice1;

        public ICommand groupChoice
        {
            get
            {
                if (groupChoice1 == null)
                {
                    groupChoice1 = new Command<string>(PerformgroupChoice);
                }

                return groupChoice1;
            }
        }

        private bool hireteacherPopupVisibility1;

        public bool hireteacherPopupVisibility { get => hireteacherPopupVisibility1; set => SetProperty(ref hireteacherPopupVisibility1, value); }

        private Command closePopUpHireTeacher;

        public ICommand ClosePopUpHireTeacher
        {
            get
            {
                if (closePopUpHireTeacher == null)
                {
                    closePopUpHireTeacher = new Command(PerformClosePopUpHireTeacher);
                }

                return closePopUpHireTeacher;
            }
        }

        private Command hireTeacherBtnCmd1;

        public ICommand hireTeacherBtnCmd
        {
            get
            {
                if (hireTeacherBtnCmd1 == null)
                {
                    hireTeacherBtnCmd1 = new Command(async => PerformhireTeacherBtnCmdAsync());
                }

                return hireTeacherBtnCmd1;
            }
        }

        private Color randonpopupTeacherbtnColor1;

        public Color randonpopupTeacherbtnColor { get => randonpopupTeacherbtnColor1; set => SetProperty(ref randonpopupTeacherbtnColor1, value); }

        private Command chooserndTeachercmd1;

        public ICommand chooserndTeachercmd
        {
            get
            {
                if (chooserndTeachercmd1 == null)
                {
                    chooserndTeachercmd1 = new Command(async => PerformchooserndTeachercmd());
                }

                return chooserndTeachercmd1;
            }
        }

        private bool hireteacherEnabled1;

        public bool hireteacherEnabled { get => hireteacherEnabled1; set => SetProperty(ref hireteacherEnabled1, value); }

        private List<favouriteTeacher> popupfavteacheritemSource1;

        public List<favouriteTeacher> popupfavteacheritemSource { get => popupfavteacheritemSource1; set => SetProperty(ref popupfavteacheritemSource1, value); }

        private bool nofavteacherlbl1;

        public bool nofavteacherlbl { get => nofavteacherlbl1; set => SetProperty(ref nofavteacherlbl1, value); }

        private bool choosefavteacherlbl1;

        public bool choosefavteacherlbl { get => choosefavteacherlbl1; set => SetProperty(ref choosefavteacherlbl1, value); }

        private favouriteTeacher seletectedFavTeacherFrompopUp1;

        public favouriteTeacher seletectedFavTeacherFrompopUp
        { get { return seletectedFavTeacherFrompopUp1; } set { seletectedFavTeacherFrompopUp1 = value; SetProperty(ref seletectedFavTeacherFrompopUp1, value); } }

        private bool selectedTeacherConnectingVisibility1;

        public bool selectedTeacherConnectingVisibility { get => selectedTeacherConnectingVisibility1; set => SetProperty(ref selectedTeacherConnectingVisibility1, value); }

        private bool chooseTeacherVisibility1;

        public bool chooseTeacherVisibility { get => chooseTeacherVisibility1; set => SetProperty(ref chooseTeacherVisibility1, value); }

        private Command cancleTeacherSearch1;

        public ICommand cancleTeacherSearch
        {
            get
            {
                if (cancleTeacherSearch1 == null)
                {
                    cancleTeacherSearch1 = new Command(async =>  PerformcancleTeacherSearch());
                }

                return cancleTeacherSearch1;
            }
        }

        private string connectingTeachertxt1;
        private string thisSesionID;
        private string firstChoiceID;
        private string secondChoiceID;
        private string thirdChoiceID;
        private string forthChoiceID;

        public string connectingTeachertxt { get => connectingTeachertxt1; set => SetProperty(ref connectingTeachertxt1, value); }

        private Command acceptTeacherTuition1;

        //public ICommand acceptTeacherTuition
        //{
        //    get
        //    {
        //        if (acceptTeacherTuition1 == null)
        //        {
        //            acceptTeacherTuition1 = new Command(async => PerformacceptTeacherTuition());
        //        }

        //        return acceptTeacherTuition1;
        //    }
        //}

        private bool acceptTeacherVisibility1;

        public bool acceptTeacherVisibility { get => acceptTeacherVisibility1; set => SetProperty(ref acceptTeacherVisibility1, value); }

        private Command rechargeCoin1;

        public ICommand rechargeCoin
        {
            get
            {
                if (rechargeCoin1 == null)
                {
                    rechargeCoin1 = new Command(async => PerformrechargeCoin());
                }

                return rechargeCoin1;
            }
        }

        private WebViewSource paymentSource;

        public WebViewSource PaymentSource { get => paymentSource; set => SetProperty(ref paymentSource, value); }

        private string rechargeAmount1;

        public string rechargeAmount
        { get => rechargeAmount1; set { rechargeAmount1 = value; if (rechargeAmount != null && rechargeAmount != "" && IsDigitsOnly(rechargeAmount)) { if (int.Parse(rechargeAmount) < 10) { rechargeButtonVisibility = false; rechargeCoinBackVisibility = false; totalAmount = ""; totalRechargeCoin = ""; addedCoinamount = ""; freeminInaddCoinScreen = ""; } else { rechargeButtonVisibility = true; CalCulateReachrgeCost(); rechargeCoinBackVisibility = true; } } else { rechargeButtonVisibility = false; totalAmount = ""; totalRechargeCoin = ""; addedCoinamount = ""; freeminInaddCoinScreen = ""; } SetProperty(ref rechargeAmount1, value); } }

        private bool rechargeButton1;

        public bool rechargeButton { get => rechargeButton1; set => SetProperty(ref rechargeButton1, value); }

        private string rechargeerrorTxt;

        public string RechargeerrorTxt
        { get => rechargeerrorTxt; set { rechargeerrorTxt = value; SetProperty(ref rechargeerrorTxt, value); } }

        private bool rechargeButtonVisibility1;

        public bool rechargeButtonVisibility { get => rechargeButtonVisibility1; set => SetProperty(ref rechargeButtonVisibility1, value); }

        private string totalRechargeCoin1;

        public string totalRechargeCoin { get => totalRechargeCoin1; set => SetProperty(ref totalRechargeCoin1, value); }

        private string totalAmount1;

        public string totalAmount { get => totalAmount1; set => SetProperty(ref totalAmount1, value); }

        private string addedCoinamount1;

        public string addedCoinamount { get => addedCoinamount1; set => SetProperty(ref addedCoinamount1, value); }

        private bool regMsgVisiblity1;

        public bool regMsgVisiblity { get => regMsgVisiblity1; set => SetProperty(ref regMsgVisiblity1, value); }

        private ImageSource promsgImgSrc1;

        public ImageSource promsgImgSrc { get => promsgImgSrc1; set => SetProperty(ref promsgImgSrc1, value); }

        private string proMsgText1;

        public string proMsgText { get => proMsgText1; set => SetProperty(ref proMsgText1, value); }

        private Command popOutRegMsgVisiblility1;

        public ICommand popOutRegMsgVisiblility
        {
            get
            {
                if (popOutRegMsgVisiblility1 == null)
                {
                    popOutRegMsgVisiblility1 = new Command(PerformpopOutRegMsgVisiblility);
                }

                return popOutRegMsgVisiblility1;
            }
        }

        private bool paymentGifGrid1;

        public bool paymentGifGrid { get => paymentGifGrid1; set => SetProperty(ref paymentGifGrid1, value); }

        private string sucPaymentText;

        public string SucPaymentText { get => sucPaymentText; set => SetProperty(ref sucPaymentText, value); }

        private Command popOUTpaymentGif1;

        public ICommand popOUTpaymentGif
        {
            get
            {
                if (popOUTpaymentGif1 == null)
                {
                    popOUTpaymentGif1 = new Command(PerformpopOUTpaymentGif);
                }

                return popOUTpaymentGif1;
            }
        }

        private bool isLoading1;

        public bool isLoading { get => isLoading1; set => SetProperty(ref isLoading1, value); }

        private string freeminInaddCoinScreen1;

        public string freeminInaddCoinScreen { get => freeminInaddCoinScreen1; set => SetProperty(ref freeminInaddCoinScreen1, value); }

        private string freeMinText1;

        public string freeMinText { get => freeMinText1; set => SetProperty(ref freeMinText1, value); }

        private bool proMsgBtnIsVisible1;

        public bool proMsgBtnIsVisible { get => proMsgBtnIsVisible1; set => SetProperty(ref proMsgBtnIsVisible1, value); }

        private Command premiumStudentBtn1;

        public ICommand premiumStudentBtn
        {
            get
            {
                if (premiumStudentBtn1 == null)
                {
                    premiumStudentBtn1 = new Command(async => PerformpremiumStudentBtn());
                }

                return premiumStudentBtn1;
            }
        }

        private bool makepremiumEnabled1;

        public bool makepremiumEnabled { get => makepremiumEnabled1; set => SetProperty(ref makepremiumEnabled1, value); }

        private string prmbuyingamount1;

        public string prmbuyingamount { get => prmbuyingamount1; set => SetProperty(ref prmbuyingamount1, value); }

        private bool chooseTeacherEnabled1;

        public bool chooseTeacherEnabled { get => chooseTeacherEnabled1; set => SetProperty(ref chooseTeacherEnabled1, value); }

        private bool rechargeCoinBackVisibility1;

        public bool rechargeCoinBackVisibility { get => rechargeCoinBackVisibility1; set => SetProperty(ref rechargeCoinBackVisibility1, value); }

        private Command showVouchers1;

        public ICommand showVouchers
        {
            get
            {
                if (showVouchers1 == null)
                {
                    showVouchers1 = new Command(PerformshowVouchers);
                }

                return showVouchers1;
            }
        }

        private Command showAddCoin1;

        public ICommand showAddCoin
        {
            get
            {
                if (showAddCoin1 == null)
                {
                    showAddCoin1 = new Command(PerformshowAddCoin);
                }

                return showAddCoin1;
            }
        }

        private Color showAddCoinColor1;

        public Color showAddCoinColor { get => showAddCoinColor1; set => SetProperty(ref showAddCoinColor1, value); }

        private Color showVoucherColor1;

        public Color showVoucherColor { get => showVoucherColor1; set => SetProperty(ref showVoucherColor1, value); }

        private Color showVoucherTxtColor1;

        public Color showVoucherTxtColor { get => showVoucherTxtColor1; set => SetProperty(ref showVoucherTxtColor1, value); }

        private Color showAddCoinTxtColor1;

        public Color showAddCoinTxtColor { get => showAddCoinTxtColor1; set => SetProperty(ref showAddCoinTxtColor1, value); }

        private string offertxt11;

        public string offertxt1 { get => offertxt11; set => SetProperty(ref offertxt11, value); }

        private string offertxt21;

        public string offertxt2 { get => offertxt21; set => SetProperty(ref offertxt21, value); }

        private string offertxt31;

        public string offertxt3 { get => offertxt31; set => SetProperty(ref offertxt31, value); }

        private string offertxt41;

        public string offertxt4 { get => offertxt41; set => SetProperty(ref offertxt41, value); }

        private string offertxt51;

        public string offertxt5 { get => offertxt51; set => SetProperty(ref offertxt51, value); }

        private bool addVisibility1;

        public bool addVisibility { get => addVisibility1; set => SetProperty(ref addVisibility1, value); }

        private bool showOffervisibility1;

        public bool showOffervisibility { get => showOffervisibility1; set => SetProperty(ref showOffervisibility1, value); }

        private bool showAddCoinvisibility1;

        public bool showAddCoinvisibility { get => showAddCoinvisibility1; set => SetProperty(ref showAddCoinvisibility1, value); }

        private bool premiumStudentVisibility1;

        public bool premiumStudentVisibility { get => premiumStudentVisibility1; set => SetProperty(ref premiumStudentVisibility1, value); }

        private string teacheracceptTimer1;

        public string teacheracceptTimer { get => teacheracceptTimer1; set => SetProperty(ref teacheracceptTimer1, value); }

        private string totalActiveTeacher1;

        public string totalActiveTeacher { get => totalActiveTeacher1; set => SetProperty(ref totalActiveTeacher1, value); }

        private string reportCount1;

        public string reportCount { get => reportCount1; set => SetProperty(ref reportCount1, value); }

        private bool isSearchGifVisible1;

        public bool isSearchGifVisible { get => isSearchGifVisible1; set => SetProperty(ref isSearchGifVisible1, value); }

        private string scteacherName1;

        public string scteacherName { get => scteacherName1; set => SetProperty(ref scteacherName1, value); }

        private string scteacherTotalTuition1;

        public string scteacherTotalTuition { get => scteacherTotalTuition1; set => SetProperty(ref scteacherTotalTuition1, value); }

        private string scteacherRatting1;

        public string scteacherRatting { get => scteacherRatting1; set => SetProperty(ref scteacherRatting1, value); }

        private bool isscTeacherInfoVisible1;

        public bool isscTeacherInfoVisible { get => isscTeacherInfoVisible1; set => SetProperty(ref isscTeacherInfoVisible1, value); }

        private bool isAnimationStart1;

        public bool isAnimationStart { get => isAnimationStart1; set => SetProperty(ref isAnimationStart1, value); }

        private bool isTuitionLive1;

        public bool isTuitionLive { get => isTuitionLive1; set => SetProperty(ref isTuitionLive1, value); }

        private string lTDate;

        public string LTDate { get => lTDate; set => SetProperty(ref lTDate, value); }

        private string lTSubject;

        public string LTSubject { get => lTSubject; set => SetProperty(ref lTSubject, value); }

        private Command liveTuitionViewCmd1;

        public ICommand liveTuitionViewCmd
        {
            get
            {
                if (liveTuitionViewCmd1 == null)
                {
                    liveTuitionViewCmd1 = new Command(PerformliveTuitionViewCmd);
                }

                return liveTuitionViewCmd1;
            }
        }

        private List<TuiTionLog> livetuitionList1;

        public List<TuiTionLog> livetuitionList { get => livetuitionList1; set => SetProperty(ref livetuitionList1, value); }


        #endregion Binding Garbage
    }
}