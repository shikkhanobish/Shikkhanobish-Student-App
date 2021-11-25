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
using XF.Material.Forms.UI.Dialogs;

namespace ShikkhanobishStudentApp.ViewModel
{
    public class AnswerCommentViewModel : BaseViewModel, INotifyPropertyChanged
    
    {
     
        List<Teacher> teacherList = new List<Teacher>();
        public string thisPostID { get; set; }
        public AnswerCommentViewModel(string pid)
        {
            thisPostID = pid;
            GetPost(pid);
            
        }

        #region Methods
        public async Task GetPost(string pid)
        {
            using (var dialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Please Wait..."))
            {
                var plist = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getPostWithID".PostJsonAsync(new { postID = pid }).ReceiveJson<Post>();
                var tlist = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTag".GetJsonAsync<List<Tag>>();
                teacherList = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getAllTeacher".PostJsonAsync(new { }).ReceiveJson<List<Teacher>>();



                foreach (var item in tlist)
                {

                    if (plist.tagID == item.tagID)
                    {
                        plist.tagName = item.tagName;

                    }
                }

                await GetAnswer(plist);
               
            }
        }
        private async Task PerformupdateEdit()
        {
            showEdit = false;
            using (var dialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Updateing Answer. Please Wait..."))
            {
                await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setAnswerWithID".PostJsonAsync(new { answerID = answer.answerID, name = answer.name, answer = answer.answer, userID = answer.userID, userType = answer.userType, imgSrc = answer.imgSrc, review = answer.review, postID = answer.postID }).ReceiveJson<Answer>();
                await GetPost(thisPostID);
            }
                
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
                if (item.userType == 2)
                {
                    item.tinfoVisible = true;
                }
                else
                {
                    item.tinfoVisible = false;
                }
                
            }
            List<Answer> SortedList = new List<Answer>();
            SortedList = updatedAnsList.OrderBy(x => x.review).ToList();
            SortedList.Reverse();
            post = plist;
            ansList = SortedList;
        }

        public async Task PerformshowEditPopup(Answer ans)
        {
            showEdit = true;
            answer = ans;
        }
        public async Task PerformcloseEditPopUp()
        {
            showEdit = false;
        }

        public async Task PerformshowTinfoPopup(Teacher tch)
        {
            showTinfo = true;
            teacher = tch;
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

        public async Task PerformshowImgPopUp()
        {
            showImg = true;
    
        }

        public async Task PerformcloseImgPopUp()
        {
            showImg = false;
        }


        #endregion


        #region Bindings

        private Answer answer1;

        public Answer answer { get => answer1; set => SetProperty(ref answer1, value); }

        private Post post1;

        public Post post{ get => post1; set => SetProperty(ref post1, value); }

        private List<Answer> ansList1;

        public List<Answer> ansList { get => ansList1; set => SetProperty(ref ansList1, value); }

        private Teacher teacher1;

        public Teacher teacher { get => teacher1; set => SetProperty(ref teacher1, value); }


        private bool editAbleAnswer1;
        public bool editAbleAnswer { get => editAbleAnswer1; set => SetProperty(ref editAbleAnswer1, value); }
        

        private bool showEdit1;
        public bool showEdit { get => showEdit1; set => SetProperty(ref showEdit1, value); }

        private bool showTinfo1;
        public bool showTinfo { get => showTinfo1; set => SetProperty(ref showTinfo1, value); }

        private bool showReply1;
        public bool showReply { get => showReply1; set => SetProperty(ref showReply1, value); }

        private bool showImg1;
        public bool showImg { get => showImg1; set => SetProperty(ref showImg1, value); }

        //Edit
        private ICommand showEditPopUp1;

        public ICommand showEditPopUp
        {
            get
            {
                return new Command<Answer>(async (a) =>
                {

                    await PerformshowEditPopup(a);
                });

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
                return new Command<Answer>(async (a) =>
                {
                    
                    foreach(var item in teacherList)
                    {
                        if (a.userID == item.teacherID)
                        {
                            await PerformshowTinfoPopup(item);
                        }
                    }
                });
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

        private ICommand showImagePopUp1;
        public ICommand showImagePopUp
        {
            get
            {
                if (showImagePopUp1 == null)
                {
                    showImagePopUp1 = new Command(async => PerformshowImgPopUp());
                }

                return showImagePopUp1;
            }
        }

        private ICommand closeImgPopUp1;

        public ICommand closeImgPopUp
        {
            get
            {
                if (closeImgPopUp1 == null)
                {
                    closeImgPopUp1 = new Command(async => PerformcloseImgPopUp());
                }

                return closeImgPopUp1;
            }
        }

        private Command updateEdit1;

        public ICommand updateEdit
        {
            get
            {
                if (updateEdit1 == null)
                {
                    updateEdit1 = new Command(async => PerformupdateEdit());
                }

                return updateEdit1;
            }
        }

        

        #endregion

    }
}
