using Flurl.Http;
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
    public class LiveTuitionViewModel: BaseViewModel, INotifyPropertyChanged
    {
        List<TuiTionLog> tuitionList = new List<TuiTionLog>();
        TuiTionLog tuionLog = new TuiTionLog();


        #region Methods
        private RealTimeApiMethods realtimeapi = new RealTimeApiMethods();
        List<TuiTionLog> lList = new List<TuiTionLog>();
        TuiTionLog thisTuition = new TuiTionLog();
        Teacher thisSelectedTeacher = new Teacher();
        public LiveTuitionViewModel()
        {
            SetTuitionLog();
            getTuitionLog();
            IsnumberofTeacherShow = false;
            GetLogList();
            
        }
        public async Task GetLogList()
        {
            using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Please Wait..."))
            {
               
                isscTeacherInfoVisible = false;
                lList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTuiTionLogNeW".GetJsonAsync<List<TuiTionLog>>();
            var tList = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getAllTeacher".PostJsonAsync(new { }).ReceiveJson<List<Teacher>>();
            var trList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTuitionRequestCount".GetJsonAsync<List<TutionRequestCount>>();

            List<TuiTionLog> ntll = new List<TuiTionLog>();
            int count = 0;
            foreach (var item in lList)
            {
                if (item.studentID == StaticPageToPassData.thisStudentInfo.studentID)
                {
                    item.teacherNameList = new List<Teacher>();
                    
                    foreach (var trCount in trList)
                    {
                        if (item.tuitionLogID == trCount.tuitionID)
                        {
                                var twithID = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getTeacherWithID".PostJsonAsync(new { teacherID = trCount.teacherID }).ReceiveJson<Teacher>();
                                if (twithID.activeStatus == 0)
                                {
                                    twithID.activeString = "Offline";

                                }
                                else
                                {
                                    twithID.activeString = "Online";
                                }
                                item.teacherNameList.Add(twithID);
                                count++;
                            }
                    }

                    item.isPendingTeacherAvailable = true;

                    ntll.Add(item);
                }
            }
            liveTuitionList = ntll;
            }
        }
       
        public ICommand ViewTeacherInfo
        {
            get
            {
                return new Command<Teacher>((selectedTeacher) =>
                {
                    
                    searchedTeacher = selectedTeacher;                    
                    TeacherRiviewInfo(selectedTeacher.teacherID);
                   
                });
            }
        }
        public ICommand seeAllTeacher
        {
            get
            {
                return new Command<TuiTionLog>(async (thist) =>
                {

                    
                    for(int i =0; i < thist.teacherNameList.Count; i++)
                    {
                        if(thist.teacherNameList[i].activeStatus == 0)
                        {
                            thist.teacherNameList[i].activeString = "Offline";
                        }
                        else if (thist.teacherNameList[i].activeStatus == 1)
                        {
                            thist.teacherNameList[i].activeString = "Online";
                        }
                        else if (thist.teacherNameList[i].activeStatus == 2)
                        {
                            thist.teacherNameList[i].activeString = "On Tuition";
                        }
                    }
                    IsnumberofTeacherShow = true;
                    thisTuition = thist;
                    teacherNameList = thisTuition.teacherNameList;
                });
            }
        }
        public async Task ContinueCheckingTeacherActivity()
        {
            while (true)
            {
                if (thisTuition.tuitionLogID != null)
                {
                    var activests = await CheckPureActive();
                    for (int i = 0; i < thisTuition.teacherNameList.Count; i++)
                    {
                        for (int j = 0; j < activests.Count; j++)
                        {
                            if (activests[j].teacherID == thisTuition.teacherNameList[i].teacherID)
                            {
                                thisTuition.teacherNameList[i].activeString = "Online";
                            }
                            else
                            {
                                thisTuition.teacherNameList[i].activeString = "Offline";
                            }
                        }
                    }
                   
                }               
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

        private void PerformpopuotTeacherCount()
        {
            IsnumberofTeacherShow = false;
        }

        public async Task TeacherRiviewInfo(int tid)
        {
            var rvWithID = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTeacherReviewWithTeacherID".PostJsonAsync(new { teacherID = tid }).ReceiveJson<List<TeacherReview>>();
           
            reviewList = rvWithID;
            isscTeacherInfoVisible = true;
        }
        private void PerformpopouyTeacherInfo()
        {
            isscTeacherInfoVisible = false;
        }
        private void Performgobakc()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }
        private async Task PerformacceptTeacherTuition()
        {
            using (var dialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Please wait for teacher to response..."))
            {
                string uriToCAllTeacher = "https://shikkhanobishrealtimeapi.shikkhanobish.com/api/ShikkhanobishSignalR/CallTeacherForTuition?&teacherID=" + searchedTeacher.teacherID + "&tuitionID=" + thisTuition.tuitionLogID + "&name=" + StaticPageToPassData.thisStudentInfo.name + "&studentID=" + StaticPageToPassData.thisStudentInfo.studentID;
                await realtimeapi.ExecuteRealTimeApi(uriToCAllTeacher);
                await Application.Current.MainPage.Navigation.PushAsync(new CallingPage(thisTuition.tuitionLogID));
            }
                
        }
        #endregion
        public async Task SetTuitionLog()
        {
            var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setTuitionLog".PostUrlEncodedAsync(new {
                studentName=tuionLog.studentName,
                subjectname=tuionLog.subjectName,
                tuitionLogID= tuionLog.tuitionLogID,
                description= tuionLog.description,
                date=tuionLog.date,
                subjectID=tuionLog.subjectID,
                studentID= tuionLog.studentID,
                tuitionLogStatus = tuionLog.tuitionLogStatus,
                pendingTeacherID = tuionLog.pendingTeacherID,
                chapterName = tuionLog.chapterName,
                chapterID = tuionLog.chapterID,
                isTextOrVideo= tuionLog.isTextOrVideo,
                img1=tuionLog.img1,
                img2=tuionLog.img2,
                img3= tuionLog.img3,
                img4= tuionLog.img4
            }).ReceiveJson<TuiTionLog>();

        }
        public async Task getTuitionLog()
        {
            tuitionList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTuitionLog".GetJsonAsync<List<TuiTionLog>>();
        }
        #region Bindings
        private List<TuiTionLog> liveTuitionList1;

        public List<TuiTionLog> liveTuitionList { get => liveTuitionList1; set => SetProperty(ref liveTuitionList1, value); }

        private Command gobakc1;

        public ICommand gobakc
        {
            get
            {
                if (gobakc1 == null)
                {
                    gobakc1 = new Command(Performgobakc);
                }

                return gobakc1;
            }
        }
        private string rqsTeacherCount1;

        public string rqsTeacherCount { get => rqsTeacherCount1; set => SetProperty(ref rqsTeacherCount1, value); }

        private Teacher thisClickedTeacher1;

        public Teacher thisClickedTeacher { get => thisClickedTeacher1; set => SetProperty(ref thisClickedTeacher1, value); }

        private bool isscTeacherInfoVisible1;

        public bool isscTeacherInfoVisible { get => isscTeacherInfoVisible1; set => SetProperty(ref isscTeacherInfoVisible1, value); }

        private Teacher searchedTeacher1;

        public Teacher searchedTeacher { get => searchedTeacher1; set => SetProperty(ref searchedTeacher1, value); }

        

        private Command popouyTeacherInfo1;

        public ICommand popouyTeacherInfo
        {
            get
            {
                if (popouyTeacherInfo1 == null)
                {
                    popouyTeacherInfo1 = new Command(PerformpopouyTeacherInfo);
                }

                return popouyTeacherInfo1;
            }
        }

        private List<TeacherReview> reviewList1;

        public List<TeacherReview> reviewList { get => reviewList1; set => SetProperty(ref reviewList1, value); }
        private List<Teacher> teacherNameList1;

        public List<Teacher> teacherNameList { get => teacherNameList1; set => SetProperty(ref teacherNameList1, value); }
        private bool IsnumberofTeacherShow1;

        public bool IsnumberofTeacherShow { get => IsnumberofTeacherShow1; set => SetProperty(ref IsnumberofTeacherShow1, value); }

        private Command acceptTeacherTuition1;

        public ICommand acceptTeacherTuition
        {
            get
            {
                if (acceptTeacherTuition1 == null)
                {
                    acceptTeacherTuition1 = new Command( async => PerformacceptTeacherTuition());
                }

                return acceptTeacherTuition1;
            }
        }
        private Command popuotTeacherCount1;
        public ICommand popuotTeacherCount
        {
            get
            {
                if (popuotTeacherCount1 == null)
                {
                    popuotTeacherCount1 = new Command(PerformpopuotTeacherCount);
                }

                return popuotTeacherCount1;
            }
        }



        #endregion
    }
}
