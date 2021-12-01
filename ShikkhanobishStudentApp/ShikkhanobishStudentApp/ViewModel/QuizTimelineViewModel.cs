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
    public class QuizTimelineViewModel : BaseViewModel, INotifyPropertyChanged
    {
        List<UserTimelineTag> userTmTg = new List<UserTimelineTag>();
        List<Post> plist = new List<Post>();
        List<Tag> tlist = new List<Tag>();
        List<Answer> anslist = new List<Answer>();
        bool isTagChanged;
        public QuizTimelineViewModel()
        {
            isTagChanged = false;

            showTag = false;
            showImg = false;
           
            GetPostList();
        }


        #region Methods
        public async Task GetPostList()
        {
            using (var dialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Please Wait..."))
            {
                userTmTg = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getUserTimelineTagWithUserID".PostJsonAsync(new { userID = StaticPageToPassData.thisStudentInfo.studentID }).ReceiveJson<List<UserTimelineTag>>();
                plist = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getPost".GetJsonAsync<List<Post>>();
                tlist = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTag".GetJsonAsync<List<Tag>>();
                anslist = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getAnswer".GetJsonAsync<List<Answer>>();



                List<Post> updatedPostList = new List<Post>();
               

                foreach (var post in plist)
                {
                    string postString = "";
                    for (int i = 0; i < post.post.Length; i++)
                    {
                        postString = postString + post.post[i].ToString();
                       
                        if (i==150)
                        {
                            post.dotDotDot = " ....See more";
                            break;
                            
                        }

                    }
                  
                    post.post = postString;

                    foreach (var ans in anslist)
                    {
                        if (post.postID == ans.postID)
                        {

                            if (ans.review == 1)
                            {
                                post.numOFCmtR++;
                                post.ansIconR = "answericon.png";
                            }
                            else if (ans.review == 0)
                            {
                                post.numOFCmtN++;
                                post.ansIconN = "noanswericon.png";
                            }
                            

                        }

                    }
                    foreach (var tag in tlist)
                    {
                        foreach (var utt in userTmTg)
                        {
                            if (post.tagID == utt.tagID && tag.tagID == utt.tagID)
                            {
                                post.tagName = tag.tagName;


                                updatedPostList.Add(post);
                            }
                                                                                                                                                        }

                    }
                    
                }
                GetTagChip();
                postList = updatedPostList;
            }

            
        }
        public void GetTagChip()
        {
            List<TagChip> thisTagChipList = new List<TagChip>();
            int listCount = 0;
            TagChip tc = new TagChip();
            bool firstTime = true;
            List<string> tagname = new List<string>();
            foreach (var tl in userTmTg)
            {
                foreach (var tname in tlist)
                {
                    if (tl.tagID == tname.tagID)
                    {
                        tagname.Add(tname.tagName);
                    }
                }
            }

            int numOfOB, numObExtraTag;
            numOfOB = tagname.Count/3;
            numObExtraTag = tagname.Count % 3;
            int add = 0;
            if(numObExtraTag > 0)
            {
                add = 1;
            }
            int indexCount = 0;
            for (int i = 0; i < numOfOB+add; i++)
            {
                tc = new TagChip();
                tc.backColor1 = "white";
                tc.backColor2 = "white";
                tc.backColor3 = "white";
                tc.backColortxt1 = "white";
                tc.backColortxt2 = "white";
                tc.backColortxt3 = "white";

                if(numOfOB > i)
                {
                    string bc1 = ChooseRandomColor();
                    tc.backColor1 = "#10" + bc1;
                    tc.backColortxt1 = "#" + bc1;
                    tc.tag1 = tagname[indexCount];
                    indexCount++;

                    string bc2 = ChooseRandomColor();
                    tc.backColor2 = "#10" + bc2;
                    tc.backColortxt2 = "#" + bc2;
                    tc.tag2 = tagname[indexCount];
                    indexCount++;

                    string bc3 = ChooseRandomColor();
                    tc.backColor3 = "#10" + bc3;
                    tc.backColortxt3 = "#" + bc3;
                    tc.tag3 = tagname[indexCount];
                    indexCount++;
                    thisTagChipList.Add(tc);
                }
                else if(i == numOfOB)
                {
                    if(numObExtraTag == 1)
                    {
                        string bc1 = ChooseRandomColor();
                        tc.backColor1 = "#10" + bc1;
                        tc.backColortxt1 = "#" + bc1;
                        tc.tag1 = tagname[indexCount];
                        indexCount++;
                        thisTagChipList.Add(tc);
                    }
                    if (numObExtraTag == 2)
                    {
                        string bc1 = ChooseRandomColor();
                        tc.backColor1 = "#10" + bc1;
                        tc.backColortxt1 = "#" + bc1;
                        tc.tag1 = tagname[indexCount];
                        indexCount++;

                        string bc2 = ChooseRandomColor();
                        tc.backColor2 = "#10" + bc2;
                        tc.backColortxt2 = "#" + bc2;
                        tc.tag2 = tagname[indexCount];
                        indexCount++;
                        thisTagChipList.Add(tc);
                    }
                }                
            }
            tagList = thisTagChipList;

        }

        public async Task BindSelectedTagList()
        {
            userTmTg = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getUserTimelineTagWithUserID".PostJsonAsync(new { userID = StaticPageToPassData.thisStudentInfo.studentID }).ReceiveJson<List<UserTimelineTag>>();
            List<Tag> updatedTagList = new List<Tag>();

            foreach (var item in tlist)
            {
                item.popUpSelected = false;
                foreach (var item2 in userTmTg)
                {
                    if (item.tagID == item2.tagID)
                    {
                        item.popUpSelected = true;
                    }
                }
                updatedTagList.Add(item);
                
            }
            if (popUptagList != null)
            {
                popUptagList.Clear();
            }
            List<Tag> SortedList = new List<Tag>();
            SortedList = updatedTagList.OrderBy(x => x.popUpSelected).ToList();
            SortedList.Reverse();
            popUptagList = SortedList;
           


        }
        public ICommand changeTagCmd
        {
            get
            {
                return new Command<Tag>(async(t) =>
                {

                    if (t.popUpSelected)
                    {
                        var thisustt = new UserTimelineTag();
                        thisustt.tagID = t.tagID;
                        thisustt.userID = StaticPageToPassData.thisStudentInfo.studentID;  //StaticPageToPassData.thisStudentInfo.studentID;
                        await DeleteUserTmTg(thisustt);
                    }
                    if(!t.popUpSelected)
                    {
                        var thisustt = new UserTimelineTag();
                        thisustt.tagID = t.tagID;
                        thisustt.userID = StaticPageToPassData.thisStudentInfo.studentID;  //StaticPageToPassData.thisStudentInfo.studentID;
                        await AddUserTmTg(thisustt);
                    }
                    isTagChanged = true;
                    await BindSelectedTagList();
                });
            }
        }
       
        public async Task PerformshowTagList()
        {
           
            await BindSelectedTagList();
            showTag = true;
        }
        public async Task PerformcloseTagPopUp()
        {
            showTag = false;
            if (isTagChanged)
            {
                using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Please Wait..."))
                {
                    await GetPostList();
                }
            }
            
            isTagChanged = false;
        }


        public async Task PerformshowImgPopUp(Post imgpost)
        {
            showImg = true;
            post = imgpost;
        }

        public async Task PerformcloseImgPopUp()
        {
            showImg = false;
        }
        public async Task DeleteUserTmTg(UserTimelineTag t)
        {
            await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/deleteUserTimelineTagWithBothID".PostJsonAsync(new { userID = t.userID, tagID = t.tagID }).ReceiveJson<UserTimelineTag>();

        }
        public async Task AddUserTmTg(UserTimelineTag t)
        {
            await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setUserTimelineTag".PostJsonAsync(new { userID = t.userID, tagID = t.tagID }).ReceiveJson<UserTimelineTag>();

        }
        public void TagBackColor() {

            TagChip tc = new TagChip();
            Random rnd = new Random();
            string bc1 = ChooseRandomColor();
            string bc2 = ChooseRandomColor();
            string bc3 = ChooseRandomColor();
            tc.backColor1= "#10"+bc1;
            tc.backColor2 = "#10"+ bc2;
            tc.backColor3 = "#10"+bc3;
            tc.backColortxt1 = "#"+bc1;
            tc.backColortxt2 = "#" + bc2;
            tc.backColortxt3 = "#" + bc3;

        }
        public ICommand answerQuestion
        {
            get
            {
                return new Command<Post>((thisPost) =>
                {
                    Application.Current.MainPage.Navigation.PushAsync(new AnswerComment(thisPost.postID));
                });
            }
        }
       
        public string ChooseRandomColor()
        {
            List<string> colorName = new List<string>();
            colorName.Add("7012B0");
            colorName.Add("B01E12");
            colorName.Add("58B012");
            colorName.Add("12ACB0");
            colorName.Add("1252B0");
            colorName.Add("7212B0");
            colorName.Add("B012A8");
            colorName.Add("B01252");

            colorName.Add("1287B1");
            colorName.Add("0D8275");
            colorName.Add("820D60");
            colorName.Add("0D8244");
            colorName.Add("6C16A3");
            colorName.Add("B92815");
            colorName.Add("51B915");
            colorName.Add("D91843");
            Random rnd = new Random();
            int index = rnd.Next(0,15);

            return colorName[index];
        }






        #endregion

        #region Bindings

        //private string dotDotDot1;

        //public string dotDotDot { get => dotDotDot1; set => SetProperty(ref dotDotDot1, value); }

        private Post post1;

        public Post post{ get => post1; set => SetProperty(ref post1, value); }

        private List<Post> postList1;

        public List<Post> postList { get => postList1; set => SetProperty(ref postList1, value); }

        private List<TagChip> tagList1;

        public List<TagChip> tagList { get => tagList1; set => SetProperty(ref tagList1, value); }

        private bool showTag1;

        public bool showTag { get => showTag1; set => SetProperty(ref showTag1, value); }
       

        private bool showImg1;
        public bool showImg { get => showImg1; set => SetProperty(ref showImg1, value); }


        private List<Tag> popUptagList1;
        public List<Tag> popUptagList { get => popUptagList1; set => SetProperty(ref popUptagList1, value); }


        private ICommand showTagList1;

        public ICommand showTagList
        {
            get
            {
                if (showTagList1 == null)
                {
                    showTagList1 = new Command( async => PerformshowTagList());
                }

                return showTagList1;
            }
        }



        private ICommand closeTagPopUp1;

        public ICommand closeTagPopUp
        {
            get
            {
                if (closeTagPopUp1 == null)
                {
                    closeTagPopUp1 = new Command(async => PerformcloseTagPopUp());
                }

                return closeTagPopUp1;
            }
        }


        private ICommand showImagePopUp1;
        public ICommand showImagePopUp
        {
            get
            {
                return new Command<Post>(async (p) =>
                {

                    await PerformshowImgPopUp(p);
                });
              
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
        private ICommand goToPostCreator1;

        public ICommand goToPostCreator
        {
            get
            {
                return new Command<Post>((thisPost) =>
                {
                    Application.Current.MainPage.Navigation.PushAsync(new PostCreator());
                });
            }
        }

        #endregion
    }

    
}
