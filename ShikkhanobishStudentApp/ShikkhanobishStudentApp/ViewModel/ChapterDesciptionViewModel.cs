using Flurl.Http;
using ShikkhanobishStudentApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace ShikkhanobishStudentApp.ViewModel
{
    public class ChapterDesciptionViewModel: BaseViewModel, INotifyPropertyChanged
    {
        List<Tag> tList = new List<Tag>();
        public ChapterDesciptionViewModel()
        {
            
         GetTopic();
        }


        #region Methods
       
        public async Task GetTopic()
        {
            tList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTag".GetJsonAsync<List<Tag>>();
            topicList = tList;
        }
        #endregion

        #region Bindings
        private List<Tag> topicList1;

        public List<Tag> topicList { get => topicList1; set => SetProperty(ref topicList1, value); }

        #endregion
    }
}
