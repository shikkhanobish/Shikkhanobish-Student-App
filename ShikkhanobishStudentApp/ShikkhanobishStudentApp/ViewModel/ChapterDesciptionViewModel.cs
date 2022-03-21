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
        public Topic thisselectedTopic = new Topic();
        public int subID;
        public List<StudentTuitionHistory> sthis = new List<StudentTuitionHistory>();
        public List<studentSubjectPurchase> subjectPurchaseList = new List<studentSubjectPurchase>();
        public ChapterDesciptionViewModel(int subjectID , int chapterID)
        {
         chapterid = chapterID;
            subID = subjectID;
         GetTopic(chapterid);
        }


        #region Methods    
        public async Task GetTopic(int chapId)
        {
            chapList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getChapter".GetJsonAsync<List<Chapter>>();
            tList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTopic".GetJsonAsync<List<Topic>>();
            sthis = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getStudentTuitionHistory".GetJsonAsync<List<StudentTuitionHistory>>();
            subjectPurchaseList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getstudentSubjectPurchaseWithSt".PostUrlEncodedAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID })
        .ReceiveJson<List<studentSubjectPurchase>>();


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
                    for (int j = 0; j < sthis.Count; j++)
                    {
                        if (sthis[j].topicID == tp[i].topicID)
                        {
                            taken = true;
                            if (sthis[j].videoURL != "N/A")
                            {
                                tp[i].isSavedVideoAvailable = false;
                            }
                        }
                    }
                    if (!taken)
                    {
                        tp[i].isTuitionAvailable = true;
                        tp[i].isTuitionNotAvailable = false;
                        for (int k = i + 1; k < tp.Count; k++)
                        {
                            tp[k].isTuitionNotAvailable = true;
                        }
                        break;
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
                    date = DateTime.Now.ToString("dd'/'MM'/'yyyy hh':'mm':'ss"),
                }).ReceiveJson<Response>();
                await GetTopic(chapterid);
            }
                
        }
        public ICommand sendtuitionres =>
            new Command<Topic>(async (thisselectedTopic) =>
            {
                TimePicker timePicker = new TimePicker
                {
                    Time = new TimeSpan(4, 15, 26) // Time set to "04:15:26"
                };
            });
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


        #endregion
    }
}
