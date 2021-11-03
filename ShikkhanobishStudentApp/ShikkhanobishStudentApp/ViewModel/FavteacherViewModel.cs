using Flurl.Http;
using ShikkhanobishStudentApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ShikkhanobishStudentApp.ViewModel
{
    public class FavteacherViewModel : BaseViewModel, INotifyPropertyChanged
    {
        int prStudentBuyingAMount = 0;
        List<favouriteTeacher> thisfavteacher = new List<favouriteTeacher>();
        public FavteacherViewModel()
        {
            getALlFavTeacher();
        }
        public async Task getALlFavTeacher()
        {

            var prm = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getPremiumStudentWithID".PostUrlEncodedAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID })
  .ReceiveJson<PremiumStudent>();
            prStudentBuyingAMount = prm.buyingAmount;
//prmbuyingamount = prm.buyingAmount + " Taka";
            if (prm.studentID == 0)
            {
               // makepremiumEnabled = true;
               // premiumStudentVisibility = true;
                prmStudentText = "*";
                studentstatus = "Normal";
                studentstatusColor = System.Drawing.Color.Black;
                maxnumteacher = prm.maxNumberofFavouriteTeacher;
                prmStudentTextVisibility = true;
            }
            else
            {
               // makepremiumEnabled = false;
                //premiumStudentVisibility = false;
                prmStudentText = "*";
                studentstatus = "Premium";
               // studentstatusColor = Color.FromHex("#864AE8");
                maxnumteacher = prm.maxNumberofFavouriteTeacher;
                prmStudentTextVisibility = false;
            }

            thisfavteacher = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getFavouriteTeacherwithStudentID".PostUrlEncodedAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID })
       .ReceiveJson<List<favouriteTeacher>>();


            for (int i = 0; i < thisfavteacher.Count; i++)
            {
                thisfavteacher[i].popupfavSelectedbackground = "Transparent";
                thisfavteacher[i].teacherRatting = Math.Round(thisfavteacher[i].teacherRatting, 2);
            }
            if (thisfavteacher.Count == 0)
            {
                prmStudentTextVisibility = true;
            }
            else
            {
                prmStudentTextVisibility = false;
            }
            favteacherItemSource = thisfavteacher;



        }
        public ICommand RemoveFavTeacher
        {
            get
            {
                return new Command<favouriteTeacher>((favteacher) =>
                {
                    Remove(favteacher);
                });
            }
        }
        public async Task Remove(favouriteTeacher favteacher)
        {
            favteacherItemSource.Clear();
            var res = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/removeFavTeacherWithTeacherID".PostUrlEncodedAsync(new { teacherID = favteacher.teacherID })
     .ReceiveJson<Response>();
            favteacherItemSource = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getFavouriteTeacherwithStudentID".PostUrlEncodedAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID })
      .ReceiveJson<List<favouriteTeacher>>();
            for (int i = 0; i < favteacherItemSource.Count; i++)
            {
                favteacherItemSource[i].teacherRatting = Math.Round(favteacherItemSource[i].teacherRatting, 2);
            }


        }
        #region Bindings
        private string prmStudentText1;

        public string prmStudentText { get => prmStudentText1; set => SetProperty(ref prmStudentText1, value); }

        private string studentstatus1;

        public string studentstatus { get => studentstatus1; set => SetProperty(ref studentstatus1, value); }

        private System.Drawing.Color studentstatusColor1;

        public System.Drawing.Color studentstatusColor { get => studentstatusColor1; set => SetProperty(ref studentstatusColor1, value); }

        private string maxnumteacher1;

        public string maxnumteacher { get => maxnumteacher1; set => SetProperty(ref maxnumteacher1, value); }

        private bool prmStudentTextVisibility1;

        public bool prmStudentTextVisibility { get => prmStudentTextVisibility1; set => SetProperty(ref prmStudentTextVisibility1, value); }
        private List<favouriteTeacher> favteacherItemSource1;

        public List<favouriteTeacher> favteacherItemSource { get => favteacherItemSource1; set => SetProperty(ref favteacherItemSource1, value); }
        #endregion
    }
}
