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
        int selectedSubID = 0;
        int selectedchapID = 0;
        string subname = "";
        string chapname = "";
        int selectedTextorVideo = 0;
        public LiveSupportViewModel()
        {
            GetAllInfo();
            //SubmitInfo();
        }

        #region Method

        public async Task GetAllInfo()
        {
            subList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getSubject".GetJsonAsync<List<Subject>>();
            chapList= await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getChapter".GetJsonAsync<List<Chapter>>();
            tuiTionLogList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTuiTionLogNeW".GetJsonAsync<List<TuiTionLog>>();
        }
        
        public async Task PerformSubjectChoose()
        {
            var stringList = new String[subList.Count];
            for (int i=0; i < subList.Count; i++)
            {
                stringList.SetValue(subList[i].name, i);
            }
            var actions = stringList;

            //Show simple dialog with title
            var result = await MaterialDialog.Instance.SelectActionAsync(title: "Select Subject",
                                                                         actions: actions);
            selectedSubID = subList[result].subjectID;
            subname = subList[result].name;
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
            selectedchapID = chapList[result].chapterID;
            chapname = chapList[result].name;
        }
        public async Task PerformTextOrVideoChoose()
        {
            var stringList = new String[tuiTionLogList.Count];
            for (int i = 0; i < tuiTionLogList.Count; i++)
            {
                if (tuiTionLogList[i].isTextOrVideo == 1)
                {
                    stringList.SetValue("Text", i);
                }
                else if(tuiTionLogList[i].isTextOrVideo == 2)
                {
                    stringList.SetValue("Video", i);
                }
                
            }
            var actions = stringList;
            

            //Show simple dialog with title
            var result = await MaterialDialog.Instance.SelectActionAsync(title: "Select Communication Type",
                                                                         actions: actions);
            selectedTextorVideo = tuiTionLogList[result].isTextOrVideo;
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
        
  
        #endregion
    }
}
