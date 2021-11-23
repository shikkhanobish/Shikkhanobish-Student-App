using Flurl.Http;
using ShikkhanobishStudentApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

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
            

           
            foreach (var item in tlist)
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
            var alist = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getAnswer".GetJsonAsync<List<Answer>>();
            var postlist = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getPost".GetJsonAsync<List<Post>>();

            List<Answer> updatedAnsList = new List<Answer>();
            foreach(var item in alist)
            {
                item.riviewImg = "";
                if (plist.postID==item.postID)
                {

                    if (item.review == 1)
                    {
                        item.riviewImg = "correctreview.gif";
                    }
                    updatedAnsList.Add(item);
                }
                if (plist.userID == item.userID)
                {
                    item.editVisible = true;
                }
                else
                {
                    item.editVisible = false;
                }
                
            }
            List<Answer> SortedList = new List<Answer>();
            SortedList = updatedAnsList.OrderBy(x => x.review).ToList();
            SortedList.Reverse();
            post = plist;
            ansList = SortedList;
        }

        public async Task PerformshowEditPopup()
        {
            showEdit = true;
        }
        public async Task PerformcloseEditPopUp()
        {
            showEdit = false;
        }

        public async Task PerformshowTinfoPopup()
        {
            showTinfo = true;
        }
        public async Task PerformcloseTinfoPopup()
        {
            showTinfo = false;
        }

        public async Task PerformshowReplyPopUp()
        {
            showReply = true;
        }
        public async Task PerformcloseReplyPopUp()
        {
            showReply = false;
        }


        #endregion


        #region Bindings
        private Post post1;

        public Post post{ get => post1; set => SetProperty(ref post1, value); }

        private List<Answer> ansList1;

        public List<Answer> ansList { get => ansList1; set => SetProperty(ref ansList1, value); }


        private bool showEdit1;
        public bool showEdit { get => showEdit1; set => SetProperty(ref showEdit1, value); }

        private bool showTinfo1;
        public bool showTinfo { get => showTinfo1; set => SetProperty(ref showTinfo1, value); }

        private bool showReply1;
        public bool showReply { get => showReply1; set => SetProperty(ref showReply1, value); }


        //Edit
        private ICommand showEditPopUp1;

        public ICommand showEditPopUp
        {
            get
            {
                if (showEditPopUp1 == null)
                {
                    showEditPopUp1 = new Command(async => PerformshowEditPopup());
                }

                return showEditPopUp1;
            }
        }

        private ICommand closeEditPopUp1;

        public ICommand closeEditPopUp
        {
            get
            {
                if (closeEditPopUp1 == null)
                {
                    closeEditPopUp1 = new Command(async => PerformcloseEditPopUp());
                }

                return closeEditPopUp1;
            }
        }


        //TeacherIfo
        private ICommand showTinfoPopUp1;
        public ICommand showTinfoPopUp
        {
            get
            {
                if (showTinfoPopUp1 == null)
                {
                    showTinfoPopUp1 = new Command(async => PerformshowTinfoPopup());
                }

                return showTinfoPopUp1;
            }
        }


        private ICommand closeTinfoPopUp1;

        public ICommand closeTinfoPopUp
        {
            get
            {
                if (closeTinfoPopUp1 == null)
                {
                    closeTinfoPopUp1 = new Command(async => PerformcloseTinfoPopup());
                }

                return closeTinfoPopUp1;
            }
        }


        //Reply
        private ICommand showReplyPopUp1;
        public ICommand showReplyPopUp
        {
            get
            {
                if (showReplyPopUp1 == null)
                {
                    showReplyPopUp1 = new Command(async => PerformshowReplyPopUp());
                }

                return showReplyPopUp1;
            }
        }

        private ICommand closeReplyPopUp1;

        public ICommand closeReplyPopUp
        {
            get
            {
                if (closeReplyPopUp1 == null)
                {
                    closeReplyPopUp1 = new Command(async => PerformcloseReplyPopUp());
                }

                return closeReplyPopUp1;
            }
        }



        #endregion

    }
}
