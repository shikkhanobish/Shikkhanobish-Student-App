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
    public class PostCreatorViewModel : BaseViewModel, INotifyPropertyChanged
    {
        List<UserTimelineTag> userTmTg = new List<UserTimelineTag>();
        List<Post> plist = new List<Post>();
        List<Tag> tlist = new List<Tag>();
        List<Answer> anslist = new List<Answer>();
        
        public PostCreatorViewModel()
        {
            validationTxt = "";
            showTag = false;
        }
        #region Methods
        public async Task BindSelectedTagList()
        {
            
            tlist = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTag".GetJsonAsync<List<Tag>>();
           
            popUptagList = tlist;

        }

  
        public async Task PerformselectTagCmd(Tag tag)
        {

            tag.popUpSelected = true;
            selectedTag = tag;
            SubTxt = "Subject : ";
            showTag = false;
        }

        public async Task PerformshowTagList()
        {

            await BindSelectedTagList();
            showTag = true;
        }
        public async Task PerformcloseTagPopUp()
        {
            showTag = false;
        }

        private async Task PerformsendPost()
        {
            if (selectedTag != null && newPost != "" && titleText != "" && newPost != null && titleText != null)
            {
                using (var dialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Please Wait..."))
                {
                    var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setPost".PostJsonAsync(new { postID = StaticPageToPassData.GenarateIDString(20), name = StaticPageToPassData.thisStudentInfo.name, post = newPost, postDate = "n/a", userID = StaticPageToPassData.thisStudentInfo.studentID, userType = 1, imgSrc = "n/a", postTitle = titleText, noOfComment = 0, tagID = selectedTag.tagID, }).ReceiveJson<Response>();
                    selectedTag.tagName = "";

                    StaticPageToPassData.eventController.CallEvent();

                    Application.Current.MainPage.Navigation.PopAsync();
                }
                
            }
            else
            {
                validationTxt = "Please Fill up All Requirements";
            }
            
        }



        #endregion



        #region Binding
        private bool showTag1;

        public bool showTag { get => showTag1; set => SetProperty(ref showTag1, value); }
        private ICommand showTagList1;

        private List<Tag> popUptagList1;
        public List<Tag> popUptagList { get => popUptagList1; set => SetProperty(ref popUptagList1, value); }

        public ICommand selectTagCmd
        {
            get
            {
                return new Command<Tag>(async (t) =>
                {

                    await PerformselectTagCmd(t);
                });

            }

        }
        
        public ICommand showTagList
        {
            get
            {
                if (showTagList1 == null)
                {
                    showTagList1 = new Command(async => PerformshowTagList());
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

        private Tag selectedTag1;

        public Tag selectedTag { get => selectedTag1; set => SetProperty(ref selectedTag1, value); }

        
        private Command sendPost1;

        public ICommand sendPost
        {
            get
            {
                if (sendPost1 == null)
                {
                    sendPost1 = new Command(async=> PerformsendPost());
                }

                return sendPost1;
            }
        }

        private string newPost1;

        public string newPost { get => newPost1; set => SetProperty(ref newPost1, value); }

        private string titleText1;

        public string titleText { get => titleText1; set => SetProperty(ref titleText1, value); }

        private string subTxt;

        public string SubTxt { get => subTxt; set => SetProperty(ref subTxt, value); }

        private string validationTxt1;

        public string validationTxt { get => validationTxt1; set => SetProperty(ref validationTxt1, value); }


        #endregion
    }
}
