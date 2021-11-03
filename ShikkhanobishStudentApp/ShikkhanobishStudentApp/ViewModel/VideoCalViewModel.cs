
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
using Xamarin.Forms;
using Xamarin.Forms.Vonage;
using XF.Material.Forms.UI.Dialogs;

namespace ShikkhanobishStudentApp.ViewModel
{
    public class VideoCalViewModel: BaseViewModel, INotifyPropertyChanged
    {

        #region Methods
        bool TimerContinue;
        int timerSecCounter,timerMinCounter, totalCostCount;
        bool isSafeTiemAvailable;
        bool isLastMin;
        bool isBalanceOver;
        RealTimeApiMethods realtimeapi = new RealTimeApiMethods();
        CostClass Allcost = new CostClass();

        public VideoCalViewModel()
        {
            isBalanceOver = false;
            hideStudent = true;
            hideVideotxt = "Hide Video";
            isLastMin = false;
            TimerContinue = true;
            timeColor = Color.LightSeaGreen;
            timerSecCounter = 20;
            timerMinCounter = 0;
            totalCostCount = 0;
            totaolCost = totalCostCount + "";
            isSafeTiemAvailable = true;
            CheckTeacherAlive();
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if(isSafeTiemAvailable)
                {
                    timerSecCounter--;
                    time = timerMinCounter + " : " + timerSecCounter+"";
                    if(timerSecCounter == 0 & timerMinCounter == 0 && isSafeTiemAvailable)
                    {
                        SendApiCall();
                        timeColor = Color.White;
                        isSafeTiemAvailable = false;
                    }
                }
                else
                {
                    if(timerSecCounter == 59)
                    {                      
                        timerSecCounter = -1;
                        timerMinCounter++;
                        SendApiCall();
                    }
                    timerSecCounter++;
                    time = timerMinCounter + " : " + timerSecCounter + "";
                }
                
                return TimerContinue;
            });
            
        }
        public async Task CheckTeacherAlive()
        {
            while(TimerContinue)
            {
                int k = 7;
                int isTeacheractiveInARow = 0;
                while(k > 0)
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
                    bool isteacheractive = false;
                    for (int i = 0; i < pureActive.Count; i++)
                    {
                        if (pureActive[i].teacherID == StaticPageToPassData.perMinCall.teacherID)
                        {
                            isteacheractive = true;
                        }
                    }
                    if (isteacheractive)
                    {
                        isTeacheractiveInARow++;
                    }
                    k--;
                }
                if(isTeacheractiveInARow == 0)
                {
                    using (var dialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Teacher has disconnected from video call!!Ending Video Tuition..."))
                    {                       
                        TimerContinue = false;
                        CrossVonage.Current.EndSession();
                        if (isSafeTiemAvailable)
                        {
                            var reresponses = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/deletePendingTuition".PostUrlEncodedAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID })
                        .ReceiveJson<Response>();
                            await StaticPageToPassData.GetStudent();
                            StaticPageToPassData.isFromReg = false;
                            Application.Current.MainPage.Navigation.PushModalAsync(new TakeTuitionView());
                            var existingPages = Application.Current.MainPage.Navigation.ModalStack.ToList();
                            foreach (var page in existingPages)
                            {
                                Application.Current.MainPage.Navigation.RemovePage(page);
                            }
                        }
                        else
                        {
                            Application.Current.MainPage.Navigation.PushModalAsync(new RattingPageView());
                        }
                        string cutUrlCall = "https://shikkhanobishrealtimeapi.shikkhanobish.com/api/ShikkhanobishSignalR/CutVideoCall?&teacherID=" + StaticPageToPassData.lastTeacherID + "&studentID=" + StaticPageToPassData.thisStudentInfo.studentID + "&isCut=" + true;
                        await realtimeapi.ExecuteRealTimeApi(cutUrlCall);
                    }
                }
                
            }
            
        }
        public async Task GetAllCost()
        {
            Allcost = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/GetCost".GetJsonAsync<CostClass>();
        }
        public async Task SendApiCall()
        {
            if (timerMinCounter == 0)
            {
                await GetAllCost();
            }
            int cost = 0;
            PerMinPassModel perminCall = StaticPageToPassData.perMinCall;
            int time = timerMinCounter + 1;
            if (perminCall.firstChoiceID == "101")
            {
                totalCostCount = totalCostCount + Allcost.SchoolCost;
                cost = Allcost.SchoolCost;
                totaolCost = totalCostCount + "";
            }
            if (perminCall.firstChoiceID == "102")
            {
                totalCostCount = totalCostCount + Allcost.CollegeCost;
                cost = Allcost.CollegeCost;
                totaolCost = totalCostCount + "";
            }
            StaticPageToPassData.lastTuitionHistoryID = perminCall.sessionID;
            StaticPageToPassData.lastTeacherID = perminCall.teacherID;
            bool cintinueTuition = true;
            var student = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getStudentWithID".PostUrlEncodedAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID })
.ReceiveJson<Student>();          
            if ((student.freemin == 1 && student.coin < cost) || (student.freemin == 0 && student.coin > cost && student.coin < cost*2))
            {
                var result = await MaterialDialog.Instance.ConfirmAsync(message: "You do not have enough balance to continue after 1 minuite. Call will cut autometicly after 1 minuite",
                                confirmingText: "Ok");
                string lastMinCall = "https://shikkhanobishrealtimeapi.shikkhanobish.com/api/ShikkhanobishSignalR/LastMinAlert?&teacherID=" + perminCall.teacherID + "&studentID=" + perminCall.studentID + "&isLastMin=" + true;
                await realtimeapi.ExecuteRealTimeApi(lastMinCall);
            }          
            else if (student.freemin == 0 && student.coin < cost)
            {
                using (var dialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Insufficient Balance To Continue Call..."))
                {
                    cintinueTuition = false;
                    Task.Delay(1000);
                    TimerContinue = false;
                    Application.Current.MainPage.Navigation.PushModalAsync(new RattingPageView());
                    string cutUrlCall = "https://shikkhanobishrealtimeapi.shikkhanobish.com/api/ShikkhanobishSignalR/CutVideoCall?&teacherID=" + StaticPageToPassData.lastTeacherID + "&studentID=" + StaticPageToPassData.thisStudentInfo.studentID + "&isCut=" + true;
                    await realtimeapi.ExecuteRealTimeApi(cutUrlCall);
                    CrossVonage.Current.EndSession();
                }
            }
            if (cintinueTuition)
            {
                var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/PerMinPassCall".PostUrlEncodedAsync(new
                {
                    studentID = perminCall.studentID,
                    teacherID = perminCall.teacherID,
                    time = time,
                    sessionID = perminCall.sessionID,
                    firstChoiceID = perminCall.firstChoiceID,
                    secondChoiceID = perminCall.secondChoiceID,
                    thirdChoiceID = perminCall.thirdChoiceID,
                    forthChoiceID = perminCall.forthChoiceID,
                    firstChoiceName = perminCall.firstChoiceName,
                    secondChoiceName = perminCall.secondChoiceName,
                    thirdChoiceName = perminCall.thirdChoiceName,
                    forthChoiceName = perminCall.forthChoiceName
                })
     .ReceiveJson<PerMinCallResponse>();
                string SendTimeAndCostInfo = "https://shikkhanobishrealtimeapi.shikkhanobish.com/api/ShikkhanobishSignalR/SendTimeAndCostInfo?&teacherID=" + perminCall.teacherID + "&studentID=" + perminCall.studentID + "&time=" + time + "&earned=" + res.earned;
                await realtimeapi.ExecuteRealTimeApi(SendTimeAndCostInfo);
            }
            

            //Ui in client
            
           
            
        }
        public async Task EndOrBackBtn()
        {
            string msg;
            if (isSafeTiemAvailable)
            {
                msg = "If you cut the call now you won't be charge any coin. Do you want to cuyt the call?";
            }
            else
            {
                msg = "Do you want to cut the call ? ";
            }
                
            var result = await MaterialDialog.Instance.ConfirmAsync(message: msg,
                                    confirmingText: "Yes",
                                    dismissiveText: "No");           
            if (result == true)
            {
                using (var dialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Ending Video Tuition..."))
                {
                    string cutUrlCall = "https://shikkhanobishrealtimeapi.shikkhanobish.com/api/ShikkhanobishSignalR/CutVideoCall?&teacherID=" + StaticPageToPassData.lastTeacherID + "&studentID=" + StaticPageToPassData.thisStudentInfo.studentID + "&isCut=" + true;
                    await realtimeapi.ExecuteRealTimeApi(cutUrlCall);
                    TimerContinue = false;
                    CrossVonage.Current.EndSession();
                    if (isSafeTiemAvailable)
                    {
                        var reresponses = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/deletePendingTuition".PostUrlEncodedAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID })
                    .ReceiveJson<Response>();
                        await StaticPageToPassData.GetStudent();
                        StaticPageToPassData.isFromReg = false;
                        Application.Current.MainPage.Navigation.PushModalAsync(new TakeTuitionView());
                        var existingPages = Application.Current.MainPage.Navigation.ModalStack.ToList();
                        foreach (var page in existingPages)
                        {
                            Application.Current.MainPage.Navigation.RemovePage(page);
                        }
                    }
                    else
                    {
                        Application.Current.MainPage.Navigation.PushModalAsync(new RattingPageView());
                    }
                }
                    
            }


        }
        private void PerformhideStudentCmd()
        {
            if (hideStudent)
            {
                hideStudent = false;
                hideVideotxt = "Show Video";
            }
            else
            {
                hideStudent = true;
                hideVideotxt = "Hide Video";
            }

        }
        private void PerformEndCall()
        {
            EndOrBackBtn();
        }
        public ICommand goRattingPage =>
            new Command(() =>
            {
                Application.Current.MainPage.Navigation.PushModalAsync(new RattingPageView());
            });


        #endregion

        #region Binding
        private string time1;

        public string time { get => time1; set => SetProperty(ref time1, value); }

        private string totaolCost1;

        public string totaolCost { get => totaolCost1; set => SetProperty(ref totaolCost1, value); }

        private Color timeColor1;

        public Color timeColor { get => timeColor1; set => SetProperty(ref timeColor1, value); }

        private Command endCall;

        public ICommand EndCall
        {
            get
            {
                if (endCall == null)
                {
                    endCall = new Command(PerformEndCall);
                }

                return endCall;
            }
        }

        private bool hideStudent1;

        public bool hideStudent { get => hideStudent1; set => SetProperty(ref hideStudent1, value); }

        private Command hideStudentCmd1;

        public ICommand hideStudentCmd
        {
            get
            {
                if (hideStudentCmd1 == null)
                {
                    hideStudentCmd1 = new Command(PerformhideStudentCmd);
                }

                return hideStudentCmd1;
            }
        }

        private string hideVideotxt1;

        public string hideVideotxt { get => hideVideotxt1; set => SetProperty(ref hideVideotxt1, value); }



        #endregion
    }
}
