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
    public class ChapterDesciptionViewModel: BaseViewModel, INotifyPropertyChanged
    {
        int chapterid;
        List<Chapter> chapList = new List<Chapter>();
        List<Topic> tList = new List<Topic>();
        public ChapterDesciptionViewModel(int chapterID)
        {
         chapterid = chapterID;   
         GetTopic(chapterid);
        }


        #region Methods
     
        public async Task GetTopic(int chapId)
        {
            chapList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getChapter".GetJsonAsync<List<Chapter>>();
            tList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTopic".GetJsonAsync<List<Topic>>();


            foreach (var item in chapList)
            {
                if (item.chapterID == chapId)
                {
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
            topicList = tp;
        }
        private void Performbuychapter()
        {
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
                    buychapter1 = new Command(Performbuychapter);
                }

                return buychapter1;
            }
        }

        private string topicNum1;

        public string topicNum { get => topicNum1; set => SetProperty(ref topicNum1, value); }


        #endregion
    }
}
