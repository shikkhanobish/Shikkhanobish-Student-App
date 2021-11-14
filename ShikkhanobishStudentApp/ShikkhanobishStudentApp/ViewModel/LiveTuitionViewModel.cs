using Flurl.Http;
using ShikkhanobishStudentApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ShikkhanobishStudentApp.ViewModel
{
    public class LiveTuitionViewModel: BaseViewModel, INotifyPropertyChanged
    {
        #region Methods
        public LiveTuitionViewModel()
        {
            GetLogList();
        }
        public async Task GetLogList()
        {
            var lList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTuiTionLogNeW".GetJsonAsync<List<TuiTionLog>>();
            List<TuiTionLog> ntll = new List<TuiTionLog>();
            foreach (var item in lList)
            {
                if (item.studentID == StaticPageToPassData.thisStudentInfo.studentID && item.tuitionLogStatus == 1)
                {
                    ntll.Add(item);
                }
            }

            //liveTuitionList = ntll;
        }
        private void Performgobakc()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }
        #endregion


        #region Bindings
        private List<TuiTionLog> liveTuitionList1;

        public List<TuiTionLog> liveTuitionList { get => liveTuitionList1; set => SetProperty(ref liveTuitionList1, value); }

        private Command gobakc1;

        public ICommand gobakc
        {
            get
            {
                if (gobakc1 == null)
                {
                    gobakc1 = new Command(Performgobakc);
                }

                return gobakc1;
            }
        }

       
        #endregion
    }
}
