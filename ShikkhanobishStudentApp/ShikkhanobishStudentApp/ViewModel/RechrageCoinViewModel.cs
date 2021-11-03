using Flurl.Http;
using Microsoft.AspNetCore.SignalR.Client;
using ShikkhanobishStudentApp.Model;
using ShikkhanobishStudentApp.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace ShikkhanobishStudentApp.ViewModel
{
    public class RechrageCoinViewModel: BaseViewModel, INotifyPropertyChanged
    {
        StudentPaymentHistory thispayment = new StudentPaymentHistory();
        int rechargeCoinAMountInt, rechargeTTakaAmountInt;
        List<Voucher> allVoucher = new List<Voucher>();
        int prStudentBuyingAMount = 0;
        public Voucher thisUsedVoucher { get; set; }
        bool isPremiumRechurge;
        HubConnection _connection = null;
        string url = "https://shikkhanobishRealTimeAPi.shikkhanobish.com/ShikkhanobishHub";
        public RechrageCoinViewModel()
        {
            avaiableCoin = StaticPageToPassData.thisStudentInfo.coin + "";
            freeMinText = StaticPageToPassData.thisStudentInfo.freemin + "";
            showAddCoinColor = Color.FromHex("#23D885");
            showVoucherColor = Color.FromHex("#F7F7F7");
            showAddCoinTxtColor = Color.White;
            showVoucherTxtColor = Color.Black;
            showOffervisibility = false;
            showAddCoinvisibility = true;
            rechargeCoinBackVisibility = false;          
            rechargeButtonVisibility = false;
            paymentGifGrid = false;
            GetVoucher();
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
       
        }
        public async Task GetPromotImage()
        {
            var allPromoImage = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/GetPromotionalImage".GetJsonAsync<List<PromotionalImage>>();
            offerList = allPromoImage;
        }
        public async Task GetVoucher()
        {
            allVoucher = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getVoucher".GetJsonAsync<List<Voucher>>();
            var prm = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getPremiumStudentWithID".PostUrlEncodedAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID })
.ReceiveJson<PremiumStudent>();
            prStudentBuyingAMount = prm.buyingAmount;
            prmbuyingamount = prm.buyingAmount + " Taka";
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
        private void PerformshowVouchers()
        {
            showVoucherColor = Color.FromHex("#2392D8");
            showAddCoinColor = Color.FromHex("#F7F7F7");
            showAddCoinTxtColor = Color.Black;
            showVoucherTxtColor = Color.White;
            showOffervisibility = true;
            showAddCoinvisibility = false;
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
        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        private async Task PerformpremiumStudentBtn()
        {
            isPremiumRechurge = true;
           
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

            
        }
        private void PerformpopOUTpaymentGif()
        {
            paymentGifGrid = false;
            SucPaymentText = "";
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
        #region Binding
        private string avaiableCoin1;

        public string avaiableCoin { get => avaiableCoin1; set => SetProperty(ref avaiableCoin1, value); }
        private bool premiumStudentVisibility1;

        public bool premiumStudentVisibility { get => premiumStudentVisibility1; set => SetProperty(ref premiumStudentVisibility1, value); }
        private string freeMinText1;

        public string freeMinText { get => freeMinText1; set => SetProperty(ref freeMinText1, value); }
        private Color showAddCoinColor1;
        private List<PromotionalImage> offerList1 { get; set; }
        public List<PromotionalImage> offerList { get { return offerList1; } set { offerList1 = value; OnPropertyChanged(); } }
        private string freeminInaddCoinScreen1;

        public string freeminInaddCoinScreen { get => freeminInaddCoinScreen1; set => SetProperty(ref freeminInaddCoinScreen1, value); }
        public Color showAddCoinColor { get => showAddCoinColor1; set => SetProperty(ref showAddCoinColor1, value); }

        private Color showVoucherColor1;
        private bool showOffervisibility1;

        public bool showOffervisibility { get => showOffervisibility1; set => SetProperty(ref showOffervisibility1, value); }
        public Color showVoucherColor { get => showVoucherColor1; set => SetProperty(ref showVoucherColor1, value); }

        private Color showVoucherTxtColor1;

        public Color showVoucherTxtColor { get => showVoucherTxtColor1; set => SetProperty(ref showVoucherTxtColor1, value); }

        private Color showAddCoinTxtColor1;
        private bool makepremiumEnabled1;

        public bool makepremiumEnabled { get => makepremiumEnabled1; set => SetProperty(ref makepremiumEnabled1, value); }
        public Color showAddCoinTxtColor { get => showAddCoinTxtColor1; set => SetProperty(ref showAddCoinTxtColor1, value); }
        private Command showVouchers1;
        private bool showAddCoinvisibility1;
        private string offertxt11;

        public string offertxt1 { get => offertxt11; set => SetProperty(ref offertxt11, value); }
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
        private string offertxt21;

        public string offertxt2 { get => offertxt21; set => SetProperty(ref offertxt21, value); }

        private string offertxt31;

        public string offertxt3 { get => offertxt31; set => SetProperty(ref offertxt31, value); }

        private string offertxt41;

        public string offertxt4 { get => offertxt41; set => SetProperty(ref offertxt41, value); }

        private string offertxt51;

        public string offertxt5 { get => offertxt51; set => SetProperty(ref offertxt51, value); }
        public bool showAddCoinvisibility { get => showAddCoinvisibility1; set => SetProperty(ref showAddCoinvisibility1, value); }
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

        public string rechargeAmount { get => rechargeAmount1; set { rechargeAmount1 = value; if (rechargeAmount != null && rechargeAmount != "" && IsDigitsOnly(rechargeAmount)) { if (int.Parse(rechargeAmount) < 10) { rechargeButtonVisibility = false; rechargeCoinBackVisibility = false; totalAmount = ""; totalRechargeCoin = ""; addedCoinamount = ""; freeminInaddCoinScreen = ""; } else { rechargeButtonVisibility = true; CalCulateReachrgeCost(); rechargeCoinBackVisibility = true; } } else { rechargeButtonVisibility = false; totalAmount = ""; totalRechargeCoin = ""; addedCoinamount = ""; freeminInaddCoinScreen = ""; } SetProperty(ref rechargeAmount1, value); } }

        private bool rechargeButton1;

        public bool rechargeButton { get => rechargeButton1; set => SetProperty(ref rechargeButton1, value); }

        private string rechargeerrorTxt;

        public string RechargeerrorTxt { get => rechargeerrorTxt; set { rechargeerrorTxt = value; SetProperty(ref rechargeerrorTxt, value); } }

        private bool rechargeButtonVisibility1;

        public bool rechargeButtonVisibility { get => rechargeButtonVisibility1; set => SetProperty(ref rechargeButtonVisibility1, value); }

        private string totalRechargeCoin1;

        public string totalRechargeCoin { get => totalRechargeCoin1; set => SetProperty(ref totalRechargeCoin1, value); }

        private string totalAmount1;

        public string totalAmount { get => totalAmount1; set => SetProperty(ref totalAmount1, value); }

        private string addedCoinamount1;
        private bool rechargeCoinBackVisibility1;

        public bool rechargeCoinBackVisibility { get => rechargeCoinBackVisibility1; set => SetProperty(ref rechargeCoinBackVisibility1, value); }
        public string addedCoinamount { get => addedCoinamount1; set => SetProperty(ref addedCoinamount1, value); }
        private string prmbuyingamount1;

        public string prmbuyingamount { get => prmbuyingamount1; set => SetProperty(ref prmbuyingamount1, value); }
        private bool regMsgVisiblity1;
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
        private bool paymentGifGrid1;

        public bool paymentGifGrid { get => paymentGifGrid1; set => SetProperty(ref paymentGifGrid1, value); }
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
        private string sucPaymentText;

        public string SucPaymentText { get => sucPaymentText; set => SetProperty(ref sucPaymentText, value); }

    }
    #endregion
}
