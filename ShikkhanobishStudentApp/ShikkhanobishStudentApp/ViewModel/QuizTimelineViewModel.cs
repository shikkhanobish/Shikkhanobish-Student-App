using Flurl.Http;
using ShikkhanobishStudentApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace ShikkhanobishStudentApp.ViewModel
{
    public class QuizTimelineViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public QuizTimelineViewModel()
        {
            //List<int> a = new List<int>();
            //a.Add(0);
            //a.Add(0);
            //a.Add(0);
            //a.Add(0);


            //postList = a;
            GetPostList();
        }


        #region Methods
        public async Task GetPostList()
        {
            var plist = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getPost".GetJsonAsync<List<Post>>();
            var tlist = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTag".GetJsonAsync<List<Tag>>();

            List<Post> updatedPostList = new List<Post>();
            foreach (var post in plist)
            {
                foreach(var tag in tlist)
                {
                    if (post.tagID == tag.tagID)
                    {
                        post.tagName = tag.tagName;
                        updatedPostList.Add(post);
                    }
                }
            }
            postList = updatedPostList;
        }
        #endregion

        #region Bindings

        private List<Post> postList1;

        public List<Post> postList { get => postList1; set => SetProperty(ref postList1, value); }
        #endregion
    }
}
