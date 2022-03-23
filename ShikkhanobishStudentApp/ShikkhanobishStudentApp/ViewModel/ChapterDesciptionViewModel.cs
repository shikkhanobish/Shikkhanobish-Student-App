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
using XF.Material.Forms.UI.Dialogs;

namespace ShikkhanobishStudentApp.ViewModel
{
    public class ChapterDesciptionViewModel: BaseViewModel, INotifyPropertyChanged
    {
        int chapterid;
        Chapter thisChapter = new Chapter();
        List<Chapter> chapList = new List<Chapter>();
        List<Topic> tList = new List<Topic>();
        public Topic ThisselectedTopic = new Topic();
        public int subID;
        public List<StudentTuitionHistory> sthis = new List<StudentTuitionHistory>();
        public List<studentSubjectPurchase> subjectPurchaseList = new List<studentSubjectPurchase>();
        public List<Subject> allsub = new List<Subject>();
        public Subject thisSub = new Subject();
        bool istimeChoose = false; 
        public int isDateChoose = 0;
        public ChapterDesciptionViewModel(int subjectID , int chapterID)
        {
         chapterid = chapterID;
            istimeChoose = false;
            timevisi = false;
            subID = subjectID;
            selectedtuitiondate = DateTime.Now;
            GetTopic(chapterid);
        }


        #region Methods    
        public async Task GetTopic(int chapId)
        {
            chapList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getChapter".GetJsonAsync<List<Chapter>>();
            tList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTopic".GetJsonAsync<List<Topic>>();
            allsub = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getSubject".GetJsonAsync<List<Subject>>();
            sthis = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getStudentTuitionHistory".GetJsonAsync<List<StudentTuitionHistory>>();
            subjectPurchaseList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getstudentSubjectPurchaseWithSt".PostUrlEncodedAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID })
        .ReceiveJson<List<studentSubjectPurchase>>();
            for(int i = 0; i < allsub.Count; i++)
            {
                if(allsub[i].subjectID == subID)
                {
                    thisSub = allsub[i];
                }
            }

            foreach (var item in chapList)
            {
                if (item.chapterID == chapId)
                {
                    thisChapter = item;
                    chapName = item.name;
                    chapDescription = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. ";
                    chapPurchaseRate = item.purchaseRate;
                }
            }
            List<Topic> tp = new List<Topic>();
            foreach (var item2 in tList)
            {
                if (chapterid == item2.chapterID)
                {
                    tp.Add(item2);
                }

            }
            topicNum = tp.Count.ToString();
            List<Topic> SortedList = new List<Topic>();
            SortedList = tp.OrderBy(x => x.topicIndex).ToList();
            tp = SortedList;
            bool purchased = false;
            for(int l = 0; l < subjectPurchaseList.Count; l++)
            {
                if(subjectPurchaseList[l].chapterID == chapterid)
                {
                    purchased = true;
                    break;
                }
            }
            if (purchased)
            {
                canBuyChapter = false;
                for (int i = 0; i < tp.Count; i++)
                {
                    bool taken = false;
                    bool tuitionDone = false;
                    for (int j = 0; j < sthis.Count; j++)
                    {
                        if (sthis[j].topicID == tp[i].topicID)
                        {
                            taken = true;
                            tp[i].istuitionrequested = true;
                            if (sthis[j].approval == 0)
                            {                               
                                tp[i].waitingVideoText = "Tuition Pending...";
                            }
                            else
                            {
                                tuitionDone = true;
                                tp[i].istuitionrequested = false;
                                tp[i].isSavedVideoAvailable = true;
                            }
                            break;
                        }
                    }
                    if (taken)
                    {
                        if (tuitionDone)
                        {
                            tp[i + 1].isTuitionAvailable = true;
                        }
                        for (int k = i + 2; k < tp.Count; k++)
                        {
                            tp[k].isTuitionNotAvailable = true;
                        }
                        break;
                    }
                    else
                    {
                        tp[i].isTuitionAvailable = true;
                        
                    }
                    

                }
            }
            else
            {
                canBuyChapter = true;
            }
            topicList = tp;
        }
        private async Task Performbuychapter()
        {
            using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Please Wait..."))
            {
                var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setstudentSubjectPurchase".PostUrlEncodedAsync(new
                {
                    studentID = StaticPageToPassData.thisStudentInfo.studentID,
                    subjectID = subID,
                    chapterID = chapterid,
                    date = DateTime.Now.ToString(StaticPageToPassData.timeFormat),
                }).ReceiveJson<Response>();
                await GetTopic(chapterid);
            }
                
        }
        public ICommand sendtuitionres =>
            new Command<Topic>(async (thisselectedTopic) =>
            {
               
                ThisselectedTopic = thisselectedTopic;
                confirmbookingvisi = true;
                selectedtuitiondate = DateTime.Now.AddDays(1);
                selectedtuitionTime = new TimeSpan();
                tuitionbookstudentname = StaticPageToPassData.thisStudentInfo.name;
                tuitionbooksubname = thisSub.name;
                tuitionbookchaptername = thisChapter.name;
                tuitionbooktopicname = thisselectedTopic.name;
                tuitionbookdate = "---";
                canBookTuition = false;
                booktuitiontextcolor = Color.FromHex("#9D9D9D");
                booktuitionbackcolor = Color.FromHex("#ECECEC");

            });
        private void Performexitchapbooking()
        {
            confirmbookingvisi = false;
        }
        private async Task Performbookthistuition()
        {
            StudentTuitionHistory thissth = new StudentTuitionHistory();
            using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Booking Your Tuition..."))
            {




                var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setStudentTuitionHistory".PostUrlEncodedAsync(new
                {
                    studentID = StaticPageToPassData.thisStudentInfo.studentID,
                    tuitionID = StaticPageToPassData.GenarateIDString(20),
                    time = "N/A",
                    teacherID = 0,
                    cost = 0,
                    ratting = 0,
                    firstChoiceID = "" + StaticPageToPassData.thisstClassChoice.institutionID,
                    secondChoiceID = "" + StaticPageToPassData.thisstClassChoice.classID,
                    thirdChoiceID = "" + thisSub.subjectID,
                    forthChoiceID = "" + chapterid,
                    date = tuitionbookdate,
                    firstChoiceName = StaticPageToPassData.thisstClassChoice.insName,
                    secondChoiceName = StaticPageToPassData.thisstClassChoice.className,
                    thirdChoiceName = "" + thisSub.name,
                    forthChoiceName = "" + chapName,
                    teacherName = "N/A",
                    studentName = StaticPageToPassData.thisStudentInfo.name,
                    teacherEarn = 0,
                    topicID = ThisselectedTopic.topicID,
                    topicName = ThisselectedTopic.name,
                    isTextOrVideo = 0,
                    videoURL = "N/A",
                    approval = 0
                }).ReceiveJson<Response>();
            }
            await MaterialDialog.Instance.AlertAsync(message: "You tuition is booked successfully! Time: " + thissth.date,
                                    title: "Successful!",
                                    acknowledgementText: "Got It");

        }
        public async Task StartPayment()
        {
            var redirectURL = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/RequestPayment".PostUrlEncodedAsync(new
            {
                name = StaticPageToPassData.thisStudentInfo.name,
                amount = thisChapter.purchaseRate,
                studentID = StaticPageToPassData.thisStudentInfo.studentID,
                phonenumber = StaticPageToPassData.thisStudentInfo.phonenumber,
            }).ReceiveJson<string>();

            await Application.Current.MainPage.Navigation.PushModalAsync(new PaymentView(redirectURL));
        }
        #endregion

        #region Bindings

        private int chapPurchaseRate1;
        public int chapPurchaseRate { get => chapPurchaseRate1; set => SetProperty(ref chapPurchaseRate1, value); }


        private string chapDescription1;
        public string chapDescription { get => chapDescription1; set => SetProperty(ref chapDescription1, value); }

        private string chapName1;
        public string chapName { get => chapName1; set => SetProperty(ref chapName1, value); }

        private List<Topic> topicList1;

        public List<Topic> topicList { get => topicList1; set => SetProperty(ref topicList1, value); }

        private Command buychapter1;

        public ICommand buychapter
        {
            get
            {
                if (buychapter1 == null)
                {
                    buychapter1 = new Command(async => Performbuychapter());
                }

                return buychapter1;
            }
        }

        private string topicNum1;

        public string topicNum { get => topicNum1; set => SetProperty(ref topicNum1, value); }

        private bool canBuyChapter1;

        public bool canBuyChapter { get => canBuyChapter1; set => SetProperty(ref canBuyChapter1, value); }

        private bool confirmbookingvisi1;

        public bool confirmbookingvisi { get => confirmbookingvisi1; set => SetProperty(ref confirmbookingvisi1, value); }

        private Command exitchapbooking1;
        public ICommand exitchapbooking => exitchapbooking1 ??= new Command(Performexitchapbooking);

        private DateTime selectedtuitiondate1;

        public DateTime selectedtuitiondate { get => selectedtuitiondate1; set { selectedtuitiondate1 = value; 
                if (isDateChoose == 2 && confirmbookingvisi) { timevisi = true; } isDateChoose++; SetProperty(ref selectedtuitiondate1, value); } }

        private TimeSpan selectedtuitionTime1;

        public TimeSpan selectedtuitionTime { get => selectedtuitionTime1; set { selectedtuitionTime1 = value; tuitionbookdate = selectedtuitiondate.ToString("dd '/' MM '/' yyyy") + " " + selectedtuitionTime; 
                canBookTuition = true;
                booktuitiontextcolor = Color.FromHex("#06522A");
                booktuitionbackcolor = Color.FromHex("#C7FFE2"); if (istimeChoose) { infoVisi = true; } istimeChoose = true; SetProperty(ref selectedtuitionTime1, value); } }

        private string tuitionbookstudentname1;

        public string tuitionbookstudentname { get => tuitionbookstudentname1; set => SetProperty(ref tuitionbookstudentname1, value); }

        private string tuitionbooksubname1;

        public string tuitionbooksubname { get => tuitionbooksubname1; set => SetProperty(ref tuitionbooksubname1, value); }

        private string tuitionbookchaptername1;

        public string tuitionbookchaptername { get => tuitionbookchaptername1; set => SetProperty(ref tuitionbookchaptername1, value); }

        private string tuitionbooktopicname1;

        public string tuitionbooktopicname { get => tuitionbooktopicname1; set => SetProperty(ref tuitionbooktopicname1, value); }

        private string tuitionbookdate1;

        public string tuitionbookdate { get => tuitionbookdate1; set => SetProperty(ref tuitionbookdate1, value); }

        private Command bookthistuition1;
        public ICommand bookthistuition => bookthistuition1 ??= new Command(async => Performbookthistuition());

        private Color booktuitionbackcolor1;

        public Color booktuitionbackcolor { get => booktuitionbackcolor1; set => SetProperty(ref booktuitionbackcolor1, value); }

        private Color booktuitiontextcolor1;

        public Color booktuitiontextcolor { get => booktuitiontextcolor1; set => SetProperty(ref booktuitiontextcolor1, value); }

        private bool canBookTuition1;

        public bool canBookTuition { get => canBookTuition1; set => SetProperty(ref canBookTuition1, value); }

        private bool timevisi1;

        public bool timevisi { get => timevisi1; set => SetProperty(ref timevisi1, value); }

        private bool infoVisi1;

        public bool infoVisi { get => infoVisi1; set => SetProperty(ref infoVisi1, value); }

        




        #endregion
    }
}
