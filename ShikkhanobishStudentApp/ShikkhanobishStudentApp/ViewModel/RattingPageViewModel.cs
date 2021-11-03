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
    public class RattingPageViewModel: BaseViewModel, INotifyPropertyChanged
    {
        int selectedRating;
        public RattingPageViewModel()
        {
            reportVisibility = false;
            oneStartVisibility = false;
            twoStartVisibility = false;
            threeStartVisibility = false;
            fourStartVisibility = false;
            fiveStartVisibility = false;
            rateBtnEnabled = false;
            reportSubmitEnabled = false;
            addFavteacherVisbility = true;
            nofavTeacherTextVisbility = false;
            GetAllInfo();
        }
        public async Task GetAllInfo()
        {
            var historyInfo = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTuitionHistoryWithTuitionID".PostUrlEncodedAsync(new { tuitionID = StaticPageToPassData.lastTuitionHistoryID })
      .ReceiveJson<StudentTuitionHistory>();
            var FavstudentList = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getFavouriteTeacherwithTeacherID".PostUrlEncodedAsync(new {
                teacherID = historyInfo.teacherID 
            })
     .ReceiveJson<List<favouriteTeacher>>();
            var prm = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getPremiumStudentWithID".PostUrlEncodedAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID })
 .ReceiveJson<PremiumStudent>();
            if(prm.studentID == 0)
            {
                var thisfavteacher = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getFavouriteTeacherwithStudentID".PostUrlEncodedAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID })
      .ReceiveJson<List<favouriteTeacher>>();
                if(thisfavteacher.Count == 1)
                {
                    addFavteacherVisbility = false;
                    nofavTeacherTextVisbility = true;
                    nofavTeacherTxt = "You cannot add more then 1 favourite teacher. To add unlimited favourite teacher please subcrite to our premium student feature only with " + prm.buyingAmount + " taka";
                }
                else if (thisfavteacher.Count == 0)
                {
                    addFavteacherVisbility = true;
                    nofavTeacherTextVisbility = false;
                }
            }
            for (int i =0; i < FavstudentList.Count; i++)
            {
                if(FavstudentList[i].studentID == StaticPageToPassData.thisStudentInfo.studentID)
                {
                    addFavteacherVisbility = false;
                    nofavTeacherTextVisbility = true;
                    nofavTeacherTxt = "Already added as favourite teacher.";
                    break;
                }
                if(i == FavstudentList.Count - 1)
                {
                    addFavteacherVisbility = true;
                    nofavTeacherTextVisbility = false;
                }
            }
            TeacherName = historyInfo.teacherName;
            TotalCost = historyInfo.cost+"";
            TuitionID = historyInfo.tuitionID;
            Time = historyInfo.time;
            cls = historyInfo.secondChoiceName;
            subject = historyInfo.thirdChoiceName;
            chapter = historyInfo.forthChoiceName;
            StaticPageToPassData.lastTeacherID = historyInfo.teacherID;
        }

        private void PerformStartClick(string index)
        {
            rateBtnEnabled = true;
            if (index == "1")
            {
                oneStartVisibility = true;
                twoStartVisibility = false;
                threeStartVisibility = false;
                fourStartVisibility = false;
                fiveStartVisibility = false;
                rattingName = "Unexpected!";
                selectedRating = 1;
            }
            if (index == "2")
            {
                oneStartVisibility = true;
                twoStartVisibility = true;
                threeStartVisibility = false;
                fourStartVisibility = false;
                fiveStartVisibility = false;
                rattingName = "Room For Improve!";
                selectedRating = 2;
            }
            if (index == "3")
            {
                oneStartVisibility = true;
                twoStartVisibility = true;
                threeStartVisibility = true;
                fourStartVisibility = false;
                fiveStartVisibility = false;
                rattingName = "Good Teacher!";
                selectedRating = 3;
            }
            if (index == "4")
            {
                oneStartVisibility = true;
                twoStartVisibility = true;
                threeStartVisibility = true;
                fourStartVisibility = true;
                fiveStartVisibility = false;
                rattingName = "Excelent Teacher!";
                selectedRating = 4;
            }
            if (index == "5")
            {
                oneStartVisibility = true;
                twoStartVisibility = true;
                threeStartVisibility = true;
                fourStartVisibility = true;
                fiveStartVisibility = true;
                rattingName = "My Favourite Teacher!";
                selectedRating = 5;
            }
            StaticPageToPassData.lastRate = selectedRating;
        }
        public void checkReportBtn()
        {
            if(firstChecked || secondChecked || thiedChecked|| forthChecked)
            {
                reportSubmitEnabled = true;
                if(firstChecked)
                {
                    StaticPageToPassData.reportIndex = 1;
                }
                if (secondChecked)
                {
                    StaticPageToPassData.reportIndex = 2;
                }
                if (thiedChecked)
                {
                    StaticPageToPassData.reportIndex = 3;
                }
                if (forthChecked)
                {
                    StaticPageToPassData.reportIndex = 4;
                }
                if (firstChecked)
                {
                    StaticPageToPassData.reportIndex = 5;
                }
            }
            else
            {
                reportSubmitEnabled = false;
            }
          
        }
        private void PerformpopUpReport()
        {
            reportVisibility = true;
        }
        private void PerformpopOutReport()
        {
            reportVisibility = false;
        }
        #region Binding
        private bool oneStartVisibility1;

        public bool oneStartVisibility { get => oneStartVisibility1; set => SetProperty(ref oneStartVisibility1, value); }

        private bool twoStartVisibility1;

        public bool twoStartVisibility { get => twoStartVisibility1; set => SetProperty(ref twoStartVisibility1, value); }

        private bool threeStartVisibility1;

        public bool threeStartVisibility { get => threeStartVisibility1; set => SetProperty(ref threeStartVisibility1, value); }

        private bool fourStartVisibility1;

        public bool fourStartVisibility { get => fourStartVisibility1; set => SetProperty(ref fourStartVisibility1, value); }

        private bool fiveStartVisibility1;

        public bool fiveStartVisibility { get => fiveStartVisibility1; set => SetProperty(ref fiveStartVisibility1, value); }

        private Command startClick;

        public ICommand StartClick
        {
            get
            {
                if (startClick == null)
                {
                    startClick = new Command<string>(PerformStartClick);
                }

                return startClick;
            }
        }

        private bool rateBtnEnabled1;

        public bool rateBtnEnabled { get => rateBtnEnabled1; set => SetProperty(ref rateBtnEnabled1, value); }

        private string rattingName1;

        public string rattingName { get => rattingName1; set => SetProperty(ref rattingName1, value); }

        private bool reportVisibility1;

        public bool reportVisibility { get => reportVisibility1; set => SetProperty(ref reportVisibility1, value); }

        private Command popOutReport1;

        public ICommand popOutReport
        {
            get
            {
                if (popOutReport1 == null)
                {
                    popOutReport1 = new Command(PerformpopOutReport);
                }

                return popOutReport1;
            }
        }

        

        private Command popUpReport1;

        public ICommand popUpReport
        {
            get
            {
                if (popUpReport1 == null)
                {
                    popUpReport1 = new Command(PerformpopUpReport);
                }

                return popUpReport1;
            }
        }

       

        private bool reportSubmitEnabled1;

        public bool reportSubmitEnabled { get => reportSubmitEnabled1; set => SetProperty(ref reportSubmitEnabled1, value); }

        private bool firstChecked1;

        public bool firstChecked { get => firstChecked1; set { firstChecked1 = value; checkReportBtn(); SetProperty(ref firstChecked1, value); } }

        private bool secondChecked1;

        public bool secondChecked { get => secondChecked1; set { secondChecked1 = value; checkReportBtn(); SetProperty(ref secondChecked1, value); } }

        private bool thiedChecked1;

        public bool thiedChecked { get => thiedChecked1; set { thiedChecked1 = value; checkReportBtn(); SetProperty(ref thiedChecked1, value); } }

        private bool forthChecked1;

        public bool forthChecked { get => forthChecked1; set { forthChecked1 = value; checkReportBtn(); SetProperty(ref forthChecked1, value); } }

        private string teacherName;

        public string TeacherName { get => teacherName; set => SetProperty(ref teacherName, value); }

        private string totalCost;

        public string TotalCost { get => totalCost; set => SetProperty(ref totalCost, value); }

        private string tuitionID;

        public string TuitionID { get => tuitionID; set => SetProperty(ref tuitionID, value); }

        private string time;

        public string Time { get => time; set => SetProperty(ref time, value); }

        private string cls1;

        public string cls { get => cls1; set => SetProperty(ref cls1, value); }

        private string subject1;

        public string subject { get => subject1; set => SetProperty(ref subject1, value); }

        private string chapter1;

        public string chapter { get => chapter1; set => SetProperty(ref chapter1, value); }

        private Command rateTeacher1;

        public ICommand rateTeacher
        {
            get
            {
                if (rateTeacher1 == null)
                {
                    rateTeacher1 = new Command(PerformrateTeacher);
                }

                return rateTeacher1;
            }
        }

        private void PerformrateTeacher()
        {
        }

    


        

        private bool addFavteacherVisbility1;

        public bool addFavteacherVisbility { get => addFavteacherVisbility1; set => SetProperty(ref addFavteacherVisbility1, value); }

        private bool nofavTeacherTextVisbility1;

        public bool nofavTeacherTextVisbility { get => nofavTeacherTextVisbility1; set => SetProperty(ref nofavTeacherTextVisbility1, value); }

        private string reportDescription1;

        public string reportDescription { get => reportDescription1; set { reportDescription1 = value; 
                if (reportDescription == null || reportDescription == "")
                {
                    StaticPageToPassData.reportDes = "N/A";
                }
                else
                { 
                    StaticPageToPassData.reportDes = reportDescription;
                }
                SetProperty(ref reportDescription1, value); } }

        private string nofavTeacherTxt1;

        public string nofavTeacherTxt { get => nofavTeacherTxt1; set => SetProperty(ref nofavTeacherTxt1, value); }
        #endregion

    }
}
