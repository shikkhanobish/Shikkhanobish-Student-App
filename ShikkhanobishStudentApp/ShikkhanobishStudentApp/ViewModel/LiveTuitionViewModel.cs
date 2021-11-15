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

                    //rqsTeacherCount1 = count.ToString();
                    item.isPendingTeacherAvailable = true;

                    //if (item.pendingTeacherID != 0)
                    //{
                        
                    //}
                    //else
                    //{
                    //    item.teacherName = "--";
                    //    item.isPendingTeacherAvailable = false;
                    //}

                    //Add Teacher
                    ntll.Add(item);
                }
            }
            rqsTeacherCount = "( " +  count.ToString()  +" )";
            liveTuitionList = ntll;

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
        
        #endregion
    }
}
