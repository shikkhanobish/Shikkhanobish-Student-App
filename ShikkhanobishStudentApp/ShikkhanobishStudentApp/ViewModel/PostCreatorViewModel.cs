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
    public class PostCreatorViewModel : BaseViewModel, INotifyPropertyChanged
    {
        List<UserTimelineTag> userTmTg = new List<UserTimelineTag>();
        List<Post> plist = new List<Post>();
        List<Tag> tlist = new List<Tag>();
        List<Answer> anslist = new List<Answer>();
        public PostCreatorViewModel()
        {
            showTag = false;
        }
        #region Methods
        public async Task BindSelectedTagList()
        {
            //userTmTg = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getUserTimelineTagWithUserID".PostJsonAsync(new { userID = 10000152 }).ReceiveJson<List<UserTimelineTag>>();
            //List<Tag> updatedTagList = new List<Tag>();

            //foreach (var item in tlist)
            //{
            //    item.popUpSelected = false;
            //    foreach (var item2 in userTmTg)
            //    {
            //        if (item.tagID == item2.tagID)
            //        {
            //            item.popUpSelected = true;
            //        }
            //    }
            //    updatedTagList.Add(item);

            //}
            //if (popUptagList != null)
            //{
            //    popUptagList.Clear();
            //}
            //List<Tag> SortedList = new List<Tag>();
            //SortedList = updatedTagList.OrderBy(x => x.popUpSelected).ToList();
            //SortedList.Reverse();
            //popUptagList = SortedList;
            tlist = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTag".GetJsonAsync<List<Tag>>();

            popUptagList = tlist;

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
       
    
        


        #endregion



        #region Binding
        private bool showTag1;

        public bool showTag { get => showTag1; set => SetProperty(ref showTag1, value); }
        private ICommand showTagList1;

        private List<Tag> popUptagList1;
        public List<Tag> popUptagList { get => popUptagList1; set => SetProperty(ref popUptagList1, value); }


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

        #endregion
    }
}
