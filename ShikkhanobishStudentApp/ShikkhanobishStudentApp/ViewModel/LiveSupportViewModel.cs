using Flurl.Http;
using ShikkhanobishStudentApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace ShikkhanobishStudentApp.ViewModel
{
    public class LiveSupportViewModel : BaseViewModel, INotifyPropertyChanged
    {
        TuiTionLog tuitionObj = new TuiTionLog();
        List<Subject> subList = new List<Subject>();
        List<Chapter> chapList = new List<Chapter>();
        List<TuiTionLog> tuiTionLogList = new List<TuiTionLog>();
        List<Subject> sortedSubList = new List<Subject>();
        List<StudentTuitionHistory> tuitionHisList = new List<StudentTuitionHistory>();
        List<TuiTionLog> savedTuitionLog = new List<TuiTionLog>();
        int selectedSubID = 0;
        int selectedchapID = 0;
        string subname = "";
        string chapname = "";
        int selectedTextorVideo = 0;
        int classSelc = 101;
        FileResult img1file;
        FileResult img2file;
        FileResult img3file;
        FileResult img4file;
        public LiveSupportViewModel()
        {
            sortedName = "All";
            sortBtntxt = Color.FromHex("#FFFFFF");
            sortBack = Color.FromHex("#7D51DD");
            chapterChooseText = "Choose Chapter";
            subjectChooseText = "Choose Subject";
            chooseansTypeTxt = "Choose Answer Type";
           
            GetAllInfo();
            GetTuitionHistory();
            //SubmitInfo();
            //tuiHisList = tuitionHisList;
            //tuiHisList.Add(new StudentTuitionHistory());
        }

        #region Method

        public async Task GetAllInfo()
        {
            subList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getSubject".GetJsonAsync<List<Subject>>();
            for (int i = 0; i < subList.Count; i++)
            {
                if (subList[i].classID == classSelc)
                {
                    sortedSubList.Add(subList[i]);
                }
            }
            chapList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getChapter".GetJsonAsync<List<Chapter>>();
            tuiTionLogList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTuiTionLogNeW".GetJsonAsync<List<TuiTionLog>>();
            tuitionHisList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getstudentTuitionHistory".GetJsonAsync<List<StudentTuitionHistory>>();
        }

        public async Task PerformSubjectChoose()
        {
            var stringList = new String[sortedSubList.Count];
            for (int i = 0; i < sortedSubList.Count; i++)
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
            foreach (var item in chapList)
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
            stringList.SetValue("Text", 0);
            stringList.SetValue("Video", 1);
            var actions = stringList;


            //Show simple dialog with title
            var result = await MaterialDialog.Instance.SelectActionAsync(title: "Select Communication Type",
                                                                         actions: actions);
            selectedTextorVideo = result + 1;
            chooseansTypeTxt = actions[result];
        }

        #endregion
        public async Task<string> UploadFileResult( FileResult fl)
        {
            UploadImage up = new UploadImage();
            var fs = await fl.OpenReadAsync();
            FileStream fileStream = fs as FileStream;
            var result = await up.UploadFileSample("shlivesupportimage", fileStream);
            return result;
        }
        private async Task PerformchooseImage()
        {
            try
            {
                img1file = await FilePicker.PickAsync(PickOptions.Images);
                var stream = await img1file.OpenReadAsync();
                imgOne = ImageSource.FromStream(() => stream);
                imgOnevisis = true;
            }
            catch (Exception ex)
            {
                await MaterialDialog.Instance.AlertAsync(message: ex.Message);
            }
        }
        private async Task PerformchooseImage2()
        {
            try
            {
                img2file = await FilePicker.PickAsync(PickOptions.Images);
                var stream = await img2file.OpenReadAsync();
                imgTWo = ImageSource.FromStream(() => stream);
                imgTwovisis = true;
            }
            catch (Exception ex)
            {
                await MaterialDialog.Instance.AlertAsync(message: ex.Message);
            }
        }
        private async Task PerformchooseImage3()
        {
            try
            {
                img3file = await FilePicker.PickAsync(PickOptions.Images);
                var stream = await img3file.OpenReadAsync();
                imgThree = ImageSource.FromStream(() => stream);
                imgThreevisis = true;
            }
            catch (Exception ex)
            {
                await MaterialDialog.Instance.AlertAsync(message: ex.Message);
            }
        }
        private async Task PerformchooseImage4()
        {
            try
            {
                img4file = await FilePicker.PickAsync(PickOptions.Images);
                var stream = await img4file.OpenReadAsync();
                imgFour = ImageSource.FromStream(() => stream);
                imgFourvisis = true;
            }
            catch (Exception ex)
            {
                await MaterialDialog.Instance.AlertAsync(message: ex.Message);
            }
        }
        private void PerformdeleteImg(string i)
        {
            if(i == "1")
            {
                img1file = null;
                imgOne = "";
                imgOnevisis = false;
            }
            if (i == "2")
            {
                img2file = null;
                imgTWo = "";
                imgTwovisis = false;
            }
            if (i == "3")
            {
                img3file = null;
                imgThree = "";
                imgThreevisis = false;
            }
            if (i == "4")
            {
                img4file = null;
                imgFour = "";
                imgFourvisis = false;
            }
        }
        public async Task PerformsubmitTution()
        {
            if (subname == null || chapname == null ||  descriptionEntry == null || subname ==  "" || chapname == "" || descriptionEntry == "" || selectedTextorVideo == 0)
            {

                await MaterialDialog.Instance.AlertAsync(message: "Please fill up all requirements");
            }
            else
            {
                try
                {
                    using (var dialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Please Wait..."))
                    {
                        string link1 = "N/A";
                        string link2 = "N/A";
                        string link3 = "N/A";
                        string link4 = "N/A";
                        if (img1file != null)
                        {
                            link1 = await UploadFileResult(img1file);
                        }
                        if (img2file != null)
                        {
                            link2 = await UploadFileResult(img2file);
                        }
                        if (img3file != null)
                        {
                            link3 = await UploadFileResult(img3file);
                        }
                        if (img4file != null)
                        {
                            link4 = await UploadFileResult(img4file);
                        }


                        var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setTuitionLog".PostUrlEncodedAsync(new
                        {
                            studentName = StaticPageToPassData.thisStudentInfo.name,
                            subjectname = subname,
                            tuitionLogID = StaticPageToPassData.GenarateNewID(),
                            description = descriptionEntry,
                            date = DateTime.Now.ToString("dd'/'MM'/'yyyy hh:mm:ss"),
                            subjectID = selectedSubID,
                            studentID = StaticPageToPassData.thisStudentInfo.studentID,
                            tuitionLogStatus = 0,
                            pendingTeacherID = 0,
                            chapterName = chapname,
                            chapterID = selectedchapID,
                            startingDate = "N/A",
                            isTextOrVideo = selectedTextorVideo,
                            img1 = link1,
                            img2 = link2,
                            img3 = link3,
                            img4 = link4
                        }).ReceiveJson<Response>();

                        await GetTuitionHistory();
                    }
                    await MaterialDialog.Instance.AlertAsync(message: "Question will be answerd within 2 hours. ");
                }
                catch (Exception ex)
                {
                    await MaterialDialog.Instance.AlertAsync(message: ex.Message);
                }
                
                
            }
           

        }
    

        private string subjectChooseText1;

        public string subjectChooseText { get => subjectChooseText1; set => SetProperty(ref subjectChooseText1, value); }
        private string chapterChooseText1;

        public string chapterChooseText { get => chapterChooseText1; set => SetProperty(ref chapterChooseText1, value); }
        public async Task GetTuitionHistory()
        {
            List<TuiTionLog> tList = new List<TuiTionLog>();
            var thisList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTuitionLogWithStudentID".PostUrlEncodedAsync(new
            {
                studentID = StaticPageToPassData.thisStudentInfo.studentID,
            }).ReceiveJson<List<TuiTionLog>>();
            List<string> timerID = new List<string>();
            List<double> timetList = new List<double>();
            int hr = 0, min = 0, sec = 0;
            string active = "352F04", complete = "04351A", pendignForTuition = "043035", tuitionDidntTake = "3A3A3A";
            foreach (var item in thisList)
            {
                if (item.isTextOrVideo == 1)
                {
                    item.tuitionTypeTxt = "Text";
                    if (item.tuitionLogStatus == 0)
                    {
                        item.btntxtColor = "#"+ active;
                        item.statusType = 1;
                        item.btnBackColor = "#FFF9CD";                    
                        item.activeOrComplete = "Active";
                        item.answeredOrNot = "Pending...";
                        item.seeAnsOrStartTuiVisibility = false;
                        
                    }
                    if (item.tuitionLogStatus== 1)
                    {
                        item.statusType = 2;
                        item.btntxtColor = "#" + complete;
                        item.btnBackColor = "#DAFFEB";
                        item.activeOrComplete = "Completed";
                        item.answeredOrNot = "Answered";
                        item.isText = "See Answer";
                        item.seeAnsOrStartTuiVisibility = true;
                    }
                }
                if (item.isTextOrVideo == 2)
                {
                    item.tuitionTypeTxt = "Video";
                    item.seeAnsOrStartTuiVisibility = true;
                   

                    if (item.tuitionLogStatus==0)
                    {
                        item.statusType = 3;
                        item.btntxtColor = "#" + active;
                        item.btnBackColor = "#FFF9CD";
                        item.activeOrComplete = "Active";
                        item.answeredOrNot = "Request Sent";
                        item.seeAnsOrStartTuiVisibility = false;                      
                    }

                    if(item.tuitionLogStatus == 2)
                    {
                        item.statusType = 4;
                        item.btntxtColor = "#" + tuitionDidntTake;
                        item.btnBackColor = "#D0D0D0";
                        item.activeOrComplete = "Session Quit";
                        item.answeredOrNot = "";
                        item.seeAnsOrStartTuiVisibility = false;
                    }

                    if (item.tuitionLogStatus == 1)
                    {
                        item.statusType = 5;
                        item.btntxtColor = "#" + pendignForTuition;
                        item.btnBackColor = "#DBFBFF";
                        item.activeOrComplete = "Waiting For Tuition";
                        item.answeredOrNot = "Waiting...";
                        item.seeAnsOrStartTuiVisibility = true;
                        item.isText = "Starting in.....";

                        CultureInfo culture = new CultureInfo("en-US");
                        DateTime oldDate = DateTime.ParseExact(item.startingDate, "dd'/'MM'/'yyyy hh':'mm':'ss tt", culture);

                        var endDate = oldDate.AddHours(2);
                        TimeSpan value = endDate.Subtract(DateTime.Now);
                        var totalsec = value.TotalSeconds;
                        if(totalsec > 0)
                        {
                            timetList.Add(totalsec);
                            timerID.Add(item.tuitionLogID);
                            
                        }
                        else
                        {
                            item.statusType = 4;
                            item.btntxtColor = "#" + tuitionDidntTake;
                            item.btnBackColor = "#D0D0D0";
                            item.activeOrComplete = "Time is up";
                            item.answeredOrNot = "";
                            item.isText = "Request For Tuition";
                        }
                        
                        
                    }

                }
                tList.Add(item);

            }
            var type1 = tList.Where(t => t.statusType == 2).ToList();
            var type2 = tList.Where(t => t.statusType == 5).ToList();
            var type3 = tList.Where(t => t.statusType == 1).ToList();
            var type4 = tList.Where(t => t.statusType == 3).ToList();
            var type5 = tList.Where(t => t.statusType == 4).ToList();
            tuiHisList = type1.Concat(type2).Concat(type3).Concat(type4).Concat(type5).ToList();
            savedTuitionLog = new List<TuiTionLog>();
            savedTuitionLog = tuiHisList;
            if (timerID.Count != 0)
            {
                StartTimer(timerID, timetList);
            }
        }
        public void StartTimer(List<string> idList, List<double>timeList)
        {
            bool TimerContinue = true;
            string btntxt = "";
            List<TuiTionLog> thislog = new List<TuiTionLog>();
            thislog = tuiHisList;
            
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {                
                for (int i = 0; i < thislog.Count; i++)
                {
                    for(int j = 0; j < idList.Count; j++)
                    {
                        if (idList[j] == thislog[i].tuitionLogID)
                        {
                            int hr = (int)timeList[j] / 3600;
                            int min = (int)(timeList[j] - hr * 3600) / 60;
                            int sec = (int)(timeList[j] - (hr * 3600 + min * 60));
                            
                            
                            if (sec == 0)
                            {
                                sec = 59;
                                min--;
                                if (min == 0)
                                {
                                    min = 59;
                                    hr--;
                                    if (hr == 0)
                                    {
                                        TimerContinue = false;
                                    }
                                }
                            }
                            
                            btntxt = "Starting in " + hr + " : " + min + " : " + sec;
                            timeList[j]--;
                            thislog[i].isText = btntxt;
                        }
                        
                    }
                    
                }
                Device.BeginInvokeOnMainThread(() => {
                    tuiHisList = null;
                    var type1 = thislog.Where(t => t.statusType == 2).ToList();
                    var type2 = thislog.Where(t => t.statusType == 5).ToList();
                    var type3 = thislog.Where(t => t.statusType == 1).ToList();
                    var type4 = thislog.Where(t => t.statusType == 3).ToList();
                    var type5 = thislog.Where(t => t.statusType == 4).ToList();
                    tuiHisList = type1.Concat(type2).Concat(type3).Concat(type4).Concat(type5).ToList();
                    savedTuitionLog = tuiHisList;
                });
                
                return TimerContinue;
            });
        }
        public void StratSpecidicTimer()
        {

        }
        private async Task PerformclickSort()
        {
            var action = new string[] { "All", "Complete","Active(Text)", "Active(Video)","Waiting For Tuition", "Didn't Take Tuition"};
            var result = await MaterialDialog.Instance.SelectActionAsync(title: "Select Tuition Type",
                                                                         actions: action);
            if (result == 0)
            {
                sortedName = action[result];
                sortBtntxt = Color.FromHex("#FFFFFF");
                sortBack = Color.FromHex("#7D51DD");
            }
            else if (result == 1)
            {
                sortedName = action[result];
                result = 2;
                sortBtntxt = Color.FromHex("#04351A");
                sortBack = Color.FromHex("#DAFFEB");
            }
            else if (result == 4)
            {
                sortedName = action[result];
                result = 5;
                sortBtntxt = Color.FromHex("#043035");
                sortBack = Color.FromHex("#DBFBFF");
            }
            else if (result == 2)
            {
                sortedName = action[result];
                result = 1;
                sortBtntxt = Color.FromHex("#352F04");
                sortBack = Color.FromHex("#FFF9CD");
            }
            else if (result == 3)
            {
                sortedName = action[result];
                result = 3;
                sortBtntxt = Color.FromHex("#04351A");
                sortBack = Color.FromHex("#DAFFEB");
            }
            else if (result == 5)
            {
                sortedName = action[result];
                result = 4;
                sortBtntxt = Color.FromHex("#3A3A3A");
                sortBack = Color.FromHex("#D0D0D0");
            }
            var thisTui = savedTuitionLog;
            if(result >= 1 && result <= 5)
            {
                var type1 = thisTui.Where(t => t.statusType == (result)).ToList();
                tuiHisList = new List<TuiTionLog>();
                List<TuiTionLog> SortedList = new List<TuiTionLog>();
                SortedList = type1.OrderBy(x => x.date).ToList();
                tuiHisList = SortedList;
            }
            else 
            {
                tuiHisList = thisTui;
            }

            
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

        private List<TuiTionLog> tuiHisList1;

        public List<TuiTionLog> tuiHisList { get => tuiHisList1; set => SetProperty(ref tuiHisList1, value); }

        private string chooseansTypeTxt1;

        public string chooseansTypeTxt { get => chooseansTypeTxt1; set => SetProperty(ref chooseansTypeTxt1, value); }

        private string descriptionEntry1;

        public string descriptionEntry { get => descriptionEntry1; set => SetProperty(ref descriptionEntry1, value); }


        private Command submitTuition1;

        public ICommand submitTuition
        {
            get
            {
                if (submitTuition1 == null)
                {
                    submitTuition1 = new Command( async => PerformsubmitTution());
                }

                return submitTuition1;
            }
        }


        private Command clickSort1;

        public ICommand clickSort
        {
            get
            {
                if (clickSort1 == null)
                {
                    clickSort1 = new Command(async =>  PerformclickSort());
                }

                return clickSort1;
            }
        }

        private string sortedName1;

        public string sortedName { get => sortedName1; set => SetProperty(ref sortedName1, value); }

        private Color sortBtntxt1;

        public Color sortBtntxt { get => sortBtntxt1; set => SetProperty(ref sortBtntxt1, value); }

        private Color sortBack1;

        public Color sortBack { get => sortBack1; set => SetProperty(ref sortBack1, value); }

        private Command chooseImage1;
        public ICommand chooseImage => chooseImage1 ??= new Command(async => PerformchooseImage());

        private ImageSource imgOne1;

        public ImageSource imgOne { get => imgOne1; set => SetProperty(ref imgOne1, value); }

        private ImageSource imgTWo1;

        public ImageSource imgTWo { get => imgTWo1; set => SetProperty(ref imgTWo1, value); }

        private ImageSource imgThree1;

        public ImageSource imgThree { get => imgThree1; set => SetProperty(ref imgThree1, value); }

        private ImageSource imgFour1;

        public ImageSource imgFour { get => imgFour1; set => SetProperty(ref imgFour1, value); }

        private Command chooseImage21;
        public ICommand chooseImage2 => chooseImage21 ??= new Command(async =>  PerformchooseImage2());

   

        private Command chooseImage31;
        public ICommand chooseImage3 => chooseImage31 ??= new Command(async =>  PerformchooseImage3());

    

        private Command chooseImage41;
        public ICommand chooseImage4 => chooseImage41 ??= new Command(async => PerformchooseImage4());

 

        private bool imgOnevisis1;

        public bool imgOnevisis { get => imgOnevisis1; set => SetProperty(ref imgOnevisis1, value); }

        private bool imgTwovisis1;

        public bool imgTwovisis { get => imgTwovisis1; set => SetProperty(ref imgTwovisis1, value); }

        private bool imgThreevisis1;

        public bool imgThreevisis { get => imgThreevisis1; set => SetProperty(ref imgThreevisis1, value); }

        private bool imgFourvisis1;

        public bool imgFourvisis { get => imgFourvisis1; set => SetProperty(ref imgFourvisis1, value); }

        private Command deleteImg1;
        public ICommand deleteImg => deleteImg1 ??= new Command<string>(PerformdeleteImg);

        


        #endregion
    }
}
