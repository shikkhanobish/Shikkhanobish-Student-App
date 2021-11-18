using Flurl.Http;
using ShikkhanobishStudentApp.Model;
using ShikkhanobishStudentApp.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ShikkhanobishStudentApp.ViewModel
{
    public class QuizTimelineViewModel : BaseViewModel, INotifyPropertyChanged
    {
        List<UserTimelineTag> userTmTg = new List<UserTimelineTag>();
        List<Post> plist = new List<Post>();
        List<Tag> tlist = new List<Tag>();
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
            userTmTg = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getUserTimelineTagWithUserID".PostJsonAsync(new { userID = 10000152 }).ReceiveJson<List<UserTimelineTag>>();
            plist = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getPost".GetJsonAsync<List<Post>>();
            tlist = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTag".GetJsonAsync<List<Tag>>();

            

            List<Post> updatedPostList = new List<Post>();
            
            foreach (var post in plist)
            {
                foreach(var tag in tlist)
                {
                    foreach(var utt in userTmTg)
                    {
                        if (post.tagID == utt.tagID)
                        {
                            post.tagName = tag.tagName;
                            updatedPostList.Add(post);
                        }
                    }
                    
                }
            }
            postList = updatedPostList;
            await GetTagChip();


            List<Tag> popUpTagList = new List<Tag>();
            
            foreach (var item in tlist)
            {
                popUpTagList.Add(item);
            }
            popUptagList = popUpTagList;
        }
        public async Task GetTagChip()
        {
            List<TagChip> thisTagChipList = new List<TagChip>();
            int listCount = 0;
            TagChip tc = new TagChip();
            bool firstTime = true;
            List<string> tagname = new List<string>();
            foreach (var tl in userTmTg)
            {
                foreach(var tname in tlist)
                {
                    if(tl.tagID == tname.tagID)
                    {
                        tagname.Add(tname.tagName);
                    }
                }


            }
            
            for(int i = 0; i < tagname.Count+1; i++)
            {
                if (listCount % 3 == 0)
                {
                    if (!firstTime)
                    {
                        thisTagChipList.Add(tc);
                    }
                    firstTime = false;
                    tc = new TagChip();
                   
                    if (i != tagname.Count)
                    {
                        tc.tag1 = tagname[i];
                        string bc1 = ChooseRandomColor();
                        string bc2 = ChooseRandomColor();
                        string bc3 = ChooseRandomColor();
                        tc.backColor1 = "#10" + bc1;
                        tc.backColor2 = "#10" + bc2;
                        tc.backColor3 = "#10" + bc3;
                        tc.backColortxt1 = "#" + bc1;
                        tc.backColortxt2 = "#" + bc2;
                        tc.backColortxt3 = "#" + bc3;
                    }
                    
                }
                else if (listCount % 3 == 1)
                {
                    tc.tag2 = tagname[i];
                }
                else if (listCount % 3 == 2)
                {
                    tc.tag3 = tagname[i];
                }
                listCount++;
            }
            tagList = thisTagChipList;

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

        private List<Post> postList1;

        public List<Post> postList { get => postList1; set => SetProperty(ref postList1, value); }

        private List<TagChip> tagList1;

        public List<TagChip> tagList { get => tagList1; set => SetProperty(ref tagList1, value); }

        private List<Tag> popUptagList1;

        public List<Tag> popUptagList { get => popUptagList1; set => SetProperty(ref popUptagList1, value); }
        #endregion
    }

    
}
