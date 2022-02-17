using Flurl.Http;
using ShikkhanobishStudentApp.Model;
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
    public class LiveSupportViewModel: BaseViewModel, INotifyPropertyChanged
    {
        List<Subject> subList = new List<Subject>();
        List<Chapter> chapList = new List<Chapter>();
        List<TuiTionLog> tuiTionLogList = new List<TuiTionLog>();
        List<Subject> sortedSubList = new List<Subject>();
        List<StudentTuitionHistory> tuitionHisList = new List<StudentTuitionHistory>();
        int selectedSubID = 0;
        int selectedchapID = 0;
        string subname = "";
        string chapname = "";
        int selectedTextorVideo = 0;
        int classSelc = 101;
        public LiveSupportViewModel()
        {
            chapterChooseText = "Choose Chapter";
            subjectChooseText = "Choose Subject";
            chooseansTypeTxt = "Choose Answer Type";
            GetAllInfo();
            GetTuitionHistory();
            //SubmitInfo();
            tuiHisList.Add(new StudentTuitionHistory());
        }

        #region Method

        public async Task GetAllInfo()
        {
            subList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getSubject".GetJsonAsync<List<Subject>>();
            for(int i = 0; i < subList.Count; i++)
            {
                if(subList[i].classID == classSelc)
                {
                    sortedSubList.Add(subList[i]);
                }
            }
            chapList= await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getChapter".GetJsonAsync<List<Chapter>>();
            tuiTionLogList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTuiTionLogNeW".GetJsonAsync<List<TuiTionLog>>();
            tuitionHisList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getstudentTuitionHistory".GetJsonAsync<List<StudentTuitionHistory>>();
        }
        
        public async Task PerformSubjectChoose()
        {
            var stringList = new String[sortedSubList.Count];
            for (int i=0; i < sortedSubList.Count; i++)
            {
                stringList.SetValue(sortedSubList[i].name, i);
            }
            var actions = stringList;
            //Show simple dialog with title
            var result = await MaterialDialog.Instance.SelectActionAsync(title: "Select Subject",
                                                                         actions: actions);
            selectedSubID = sortedSubList[result].subjectID;
            subname = sortedSubList[result].name;
            subjectChooseText = subname;
            chapterChooseText = "Choose Chapter";
        }
        public async Task PerformChapterChoose()
        {
            List<Chapter> ch = new List<Chapter>();
            foreach(var item in chapList)
            {
                if (selectedSubID == item.subjectID)
                {
                    ch.Add(item);
                }
            }
            var stringList = new String[ch.Count];
            for (int i = 0; i < ch.Count; i++)
            {
                stringList.SetValue(ch[i].name, i);

            }
            var actions = stringList;

            //Show simple dialog with title
            var result = await MaterialDialog.Instance.SelectActionAsync(title: "Select Chapter",
                                                                         actions: actions);
            selectedchapID = ch[result].chapterID;
            chapname = ch[result].name;
            chapterChooseText = chapname;
        }
        public async Task PerformTextOrVideoChoose()
        {
            var stringList = new String[2];
            stringList.SetValue("Text",0);
            stringList.SetValue("Video", 1);
            var actions = stringList;
            

            //Show simple dialog with title
            var result = await MaterialDialog.Instance.SelectActionAsync(title: "Select Communication Type",
                                                                         actions: actions);
            selectedTextorVideo = tuiTionLogList[result].isTextOrVideo;
            chooseansTypeTxt = actions[result];
        }

        #endregion

        public async Task SubmitInfo()
        {
            var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setTuitionLog".PostUrlEncodedAsync(new
            {
                studentName = StaticPageToPassData.thisStudentInfo.name,
                subjectname = subname,
                tuitionLogID = chapname,
                description = "n/a",
                date = DateTime.Now,
                subjectID = selectedSubID,
                studentID = StaticPageToPassData.thisStudentInfo.studentID,
                tuitionLogStatus = 0,
                pendingTeacherID = 0,
                chapterName = chapname,
                chapterID = selectedchapID,
                isTextOrVideo = selectedTextorVideo,
                img1 = "n/a",
                img2 = "n/a",
                img3 = "n/a",
                img4 = "n/a"
            }).ReceiveJson<TuiTionLog>();
        }
        private string subjectChooseText1;

        public string subjectChooseText { get => subjectChooseText1; set => SetProperty(ref subjectChooseText1, value); }
        private string chapterChooseText1;

        public string chapterChooseText { get => chapterChooseText1; set => SetProperty(ref chapterChooseText1, value); }
        public async Task GetTuitionHistory()
        {
            tuiHisList = tuitionHisList;
        }

        #region Bindings
        private Command subjectChoose1;

        public ICommand subjectChoose
        {
            get
            {
                if (subjectChoose1 == null)
                {
                    subjectChoose1 = new Command(async => PerformSubjectChoose());
                }

                return subjectChoose1;
            }
        }
        private Command chapterChoose1;

        public ICommand chapterChoose
        {
            get
            {
                if (chapterChoose1 == null)
                {
                    chapterChoose1 = new Command(async => PerformChapterChoose());
                }

                return chapterChoose1;
            }
        }
        private Command textOrVideoChoose1;

        public ICommand textOrVideoChoose
        {
            get
            {
                if (textOrVideoChoose1 == null)
                {
                    textOrVideoChoose1 = new Command(async => PerformTextOrVideoChoose());
                }

                return textOrVideoChoose1;
            }
        }

        private List<StudentTuitionHistory> tuiHisList1;

        public List<StudentTuitionHistory> tuiHisList { get => tuiHisList1; set => SetProperty(ref tuiHisList1, value); }

        private string chooseansTypeTxt1;

        public string chooseansTypeTxt { get => chooseansTypeTxt1; set => SetProperty(ref chooseansTypeTxt1, value); }
        #endregion
    }
}
