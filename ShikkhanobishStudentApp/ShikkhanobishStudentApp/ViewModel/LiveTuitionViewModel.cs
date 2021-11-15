using Flurl.Http;
using ShikkhanobishStudentApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ShikkhanobishStudentApp.ViewModel
{
    public class LiveTuitionViewModel: BaseViewModel, INotifyPropertyChanged
    {
        #region Methods
        public LiveTuitionViewModel()
        {
            GetLogList();
        }
        public async Task GetLogList()
        {
            isscTeacherInfoVisible = false;
            var lList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTuiTionLogNeW".GetJsonAsync<List<TuiTionLog>>();
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
                            
                            var twithID = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getTeacherWithID".PostJsonAsync(new { teacherID = trCount.teacherID}).ReceiveJson<Teacher>();
                            
                            item.teacherNameList.Add(twithID);
                            count++;
                        }
                    }

                    item.isPendingTeacherAvailable = true;

                    ntll.Add(item);
                }
            }
            rqsTeacherCount = "( " +  count.ToString()  +" )";
            liveTuitionList = ntll;

        }
        public ICommand ViewTeacherInfo
        {
            get
            {
                return new Command<Teacher>((selectedTeacher) =>
                {
                    isscTeacherInfoVisible = true;
                    searchedTeacher = selectedTeacher;
                    TeacherRiviewInfo(selectedTeacher.teacherID);
                });
            }
        }

        public async Task TeacherRiviewInfo(int tid)
        {
            var rvWithID = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTeacherReviewWithTeacherID".PostJsonAsync(new { teacherID = tid }).ReceiveJson<TeacherReview>();
            //reviewList = rvWithID;

        }
        private void PerformpopouyTeacherInfo()
        {
            isscTeacherInfoVisible = false;
        }
        private void Performgobakc()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }
        #endregion


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


        #endregion
    }
}
