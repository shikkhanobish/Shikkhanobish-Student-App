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
        List<Answer> alist = new List<Answer>();
        List<AnswerVote> avList = new List<AnswerVote>();
        Answer obj = new Answer();
        Post plist = new Post();
        public string thisPostID { get; set; }

        public AnswerCommentViewModel(string pid)
        {
            thisPostID = pid;
            GetPost(pid);
            showImg = false;
            ViewCount();
            //obj.voteFrameVisibility = true;
        }

        #region Methods
        public async Task ViewCount()
        {
            var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/viewCountWithPostID".PostJsonAsync(new { postID =thisPostID }).ReceiveJson<Response>();
            StaticPageToPassData.postViewEventStatic.CallPostViewEvent();
        }
        public async Task GetPost(string pid)
        {
            using (var dialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Please Wait..."))
            {
                plist = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getPostWithID".PostJsonAsync(new { postID = pid }).ReceiveJson<Post>();
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
        
        public async Task GetAnswer(Post plist)
        {
            alist = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getAnswer".GetJsonAsync<List<Answer>>();
            var postlist = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getPost".GetJsonAsync<List<Post>>();

            avList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getAnswerVote".GetJsonAsync<List<AnswerVote>>();

            List<Answer> updatedAnsList = new List<Answer>();
            foreach(var item in alist)
            {
                
                if (plist.postID==item.postID)
                {
                    foreach (var vote in avList)
                    {
                        if (item.answerID == vote.answerID)
                        {
                            
                            item.upBackColor = "Transparent";
                            item.downBackColor = "Transparent";
                            
                            if (StaticPageToPassData.thisStudentInfo.studentID == vote.userID)
                            {
                                
                                if (vote.upOrdownVote == 1)
                                {
                                    item.upBackColor = "#100DD545";
                                    item.downBackColor = "Transparent";
                                }
                          
                                if (vote.upOrdownVote == 2)
                                {
                                    item.downBackColor = "#10F0140E";
                                    item.upBackColor = "Transparent";
                                }

                            }
                           
                            
                            if (vote.upOrdownVote == 1)
                            {
                                item.upVoteCount++;
                            }

                            else if (vote.upOrdownVote == 2)
                            {
                                item.downVoteCount++;
                               
                                
                            }
                            break;
                        }
                    }
                    
                    if (StaticPageToPassData.thisStudentInfo.studentID == item.userID)
                    {
                        item.editVisible = true;
                        item.voteFrameVisibility = false;
                    }
                    else
                    {
                        item.voteFrameVisibility = true;
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
                    updatedAnsList.Add(item);
                }
                
            }
            List<Answer> SortedList = new List<Answer>();
            SortedList = updatedAnsList.OrderBy(x => x.review).ToList();
            SortedList.Reverse();
            post = plist;
            ansList = SortedList;
        }

        private async Task PerformsendAnswer()
        {
            using (var dialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Please Wait..."))
            {
                string ansID = StaticPageToPassData.GenarateIDString(20);
                var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setAnswer".PostJsonAsync(new { answerID = ansID, name = StaticPageToPassData.thisStudentInfo.name, answer = newComment, userID = StaticPageToPassData.thisStudentInfo.studentID, userType = 1, imgSrc = "n/a", review = 0, postID = thisPostID, answerDate = "n/a" }).ReceiveJson<Response>();
                await GetPost(thisPostID);


          
                var res2= await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setNotification".PostJsonAsync(new { notificationID = StaticPageToPassData.GenarateIDString(50), userId = StaticPageToPassData.thisStudentInfo.studentID, userType = 1, notificationType = 2, 
                    
                    description = "n/a", refIDOne = ansID, refIDTwo = "n/a", refIDThree = "n/a", refIDFour = "n/a", notificationDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                }).ReceiveJson<Response>();
            }

            //Call Push Notification
            newComment = "";
        }
        private async Task PerformupdateEdit()
        {
            showEdit = false;
            using (var dialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Updating Answer. Please Wait..."))
            {
                var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setAnswerWithID".PostJsonAsync(new { answerID = answer.answerID, name = answer.name, answer = answer.answer, userID = answer.userID, userType = answer.userType, imgSrc = answer.imgSrc, review = answer.review, postID = answer.postID, answerDate = answer.answerDate }).ReceiveJson<Response>();
                await GetPost(thisPostID);
            }
         
        }
        private async Task PerformdeleteAnswer()
        {
            showEdit = false;
            using (var dialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Deleting Answer. Please Wait..."))
            {
                var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/deleteAnswerWithID".PostJsonAsync(new { answerID=answer.answerID }).ReceiveJson<Response>();
                await GetPost(thisPostID);
            }
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
            if (post.imgSrc == "" || post.imgSrc == null)
            {
                //imgButtonEnable = false;
                showImg = false;
            }
            else if (post.imgSrc != "" || post.imgSrc != null)
            {
                //imgButtonEnable = true;
                showImg = true;
                
            }
        }

        public async Task PerformcloseImgPopUp()
        {
            showImg = false;

        }
        public void GetVote(Answer ans)
        {
            

        }

        public async Task PerformupVote(Answer ans)
        {
            bool isMatch = false;
            foreach (var item in avList)
            {
                if (ans.answerID == item.answerID && StaticPageToPassData.thisStudentInfo.studentID == item.userID && item.upOrdownVote == 1)
                {
                    isMatch = true;
                    var resp = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/deleteAnswerVote".PostJsonAsync(new { answerID = ans.answerID, userID = StaticPageToPassData.thisStudentInfo.studentID }).ReceiveJson<Response>();

                }

            }
            if (!isMatch)
            {
                var resp = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/deleteAnswerVote".PostJsonAsync(new { answerID = ans.answerID, userID = StaticPageToPassData.thisStudentInfo.studentID }).ReceiveJson<Response>();
                var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setAnswerVote".PostJsonAsync(new { ansvoteID = StaticPageToPassData.GenarateNewID(), answerID = ans.answerID, userID = StaticPageToPassData.thisStudentInfo.studentID, upOrdownVote = 1 }).ReceiveJson<Response>();
            }
            await GetAnswer(plist);
            //avList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getAnswerVote".GetJsonAsync<List<AnswerVote>>();
            //foreach (var item in avList)
            //{
            //    if (ans.answerID == item.answerID && StaticPageToPassData.thisStudentInfo.studentID == item.userID)
            //    {
            //        for (int i = 0; i < ansList.Count; i++)
            //        {
            //            if (newAns[i].answerID == ans.answerID)
            //            {
            //                if (item.upOrdownVote == 1)
            //                {
            //                    newAns[i].upBackColor = "Transparent";
            //                    newAns[i].upVoteCount--;

            //                }
            //                if (item.upOrdownVote == 2)
            //                {
            //                    newAns[i].upBackColor = "#100DD545";
            //                    newAns[i].upVoteCount++;
            //                    newAns[i].downVoteCount--;
            //                    newAns[i].downBackColor = "Transparent";
            //                }


            //            }
            //        }
            //        ansList = new List<Answer>();
            //        ansList = newAns;
            //    }
            //}

        }

        public async Task PerformdownVote(Answer ans)

        {
            bool isMatch = false;
            foreach (var item in avList)
            {
                if (ans.answerID == item.answerID && StaticPageToPassData.thisStudentInfo.studentID == item.userID && item.upOrdownVote == 2)
                {
                    isMatch = true;
                    var resp = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/deleteAnswerVote".PostJsonAsync(new { answerID = ans.answerID, userID = StaticPageToPassData.thisStudentInfo.studentID }).ReceiveJson<Response>();

                }
                
            }
            if (!isMatch)
            {
                var resp = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/deleteAnswerVote".PostJsonAsync(new { answerID = ans.answerID, userID = StaticPageToPassData.thisStudentInfo.studentID }).ReceiveJson<Response>();
                var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setAnswerVote".PostJsonAsync(new { ansvoteID = StaticPageToPassData.GenarateNewID(), answerID = ans.answerID, userID = StaticPageToPassData.thisStudentInfo.studentID, upOrdownVote = 2 }).ReceiveJson<Response>();
            }
            await GetAnswer(plist);
            //avList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getAnswerVote".GetJsonAsync<List<AnswerVote>>();
            //foreach (var item in avList)
            //{
            //    if (ans.answerID == item.answerID && StaticPageToPassData.thisStudentInfo.studentID == item.userID)
            //    {
            //        for (int i = 0; i < ansList.Count; i++)
            //        {
            //            if (newAns[i].answerID == ans.answerID)
            //            {
            //                if (item.upOrdownVote == 1)
            //                {
            //                    newAns[i].downBackColor = "#100DD545";
            //                    newAns[i].downVoteCount++;
            //                    newAns[i].upVoteCount--;
            //                    newAns[i].upBackColor = "Transparent";
            //                }
            //                if (item.upOrdownVote == 2)
            //                {
            //                    newAns[i].downBackColor = "Transparent";
            //                    newAns[i].downVoteCount--;
            //                }


            //            }
            //        }
            //        ansList = new List<Answer>();
            //        ansList = newAns;
            //    }
            //}


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

        
        private bool imgButtonEnable1;
        public bool imgButtonEnable { get => imgButtonEnable1; set => SetProperty(ref imgButtonEnable1, value); }

        private ICommand upVote1;

        public ICommand upVote
        {
            get
            {
                return new Command<Answer>(async (a) =>
                {

                    await PerformupVote(a);
                });
                
            }
        }

        private ICommand downVote1;

        public ICommand downVote
        {
            get
            {
                return new Command<Answer>(async (a) =>
                {

                    await PerformdownVote(a);
                });
            }
        }

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

        private Command sendAnswer1;

        public ICommand sendAnswer
        {
            get
            {
                if (sendAnswer1 == null)
                {
                    sendAnswer1 = new Command(async =>PerformsendAnswer());
                }

                return sendAnswer1;
            }
        }

        private string newComment1;

        public string newComment { get => newComment1; set => SetProperty(ref newComment1, value); }

        
        
        private Command deleteAnswer1;

        public ICommand deleteAnswer
        {
            get
            {
                if (deleteAnswer1 == null)
                {
                    deleteAnswer1 = new Command(async=> PerformdeleteAnswer());
                }

                return deleteAnswer1;
            }
        }
        #endregion

    }
}
