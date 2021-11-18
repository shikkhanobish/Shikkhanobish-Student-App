using Flurl.Http;
using ShikkhanobishStudentApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace ShikkhanobishStudentApp.ViewModel
{
    public class AnswerCommentViewModel : BaseViewModel, INotifyPropertyChanged
    
    {

        public AnswerCommentViewModel(string pid)
        {
            GetPost(pid);
            
        }

        #region Methods
        public async Task GetPost(string pid)
        {
            var plist = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getPostWithID".PostJsonAsync(new { postID =  pid}).ReceiveJson<Post>();
            var tlist = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTag".GetJsonAsync<List<Tag>>();
            

            foreach(var item in tlist)
            {
                if (plist.tagID == item.tagID)
                {
                    plist.tagName = item.tagName;
                    
                }
            }
            
            await GetAnswer(plist);
        }
        public async Task GetAnswer(Post plist)
        {
            var list = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getAnswer".GetJsonAsync<List<Answer>>();
            List<Answer> updatedAnsList = new List<Answer>();
            foreach(var item in list)
            {
                if (plist.postID==item.postID)
                {
                    plist.numOFCmt++;
                    updatedAnsList.Add(item);
                }
                
            }
            post = plist;
            ansList = updatedAnsList;
        }
        #endregion


        #region Bindings
        private Post post1;

        public Post post{ get => post1; set => SetProperty(ref post1, value); }

        private List<Answer> ansList1;

        public List<Answer> ansList { get => ansList1; set => SetProperty(ref ansList1, value); }


        #endregion

    }
}
