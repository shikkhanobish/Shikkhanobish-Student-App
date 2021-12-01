using Flurl.Http;
using ShikkhanobishStudentApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShikkhanobishStudentApp.ViewModel
{
    public class StudentPerformanceViewModel: BaseViewModel, INotifyPropertyChanged
    {
        ClassChoice classChoice = new ClassChoice();
        public StudentPerformanceViewModel()
        {
            
            GetBarList();
        }

        #region Methods
        public async Task GetBarList()
        {
            var tlist = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTag".GetJsonAsync<List<Tag>>();
            barChartList = tlist;
            var classList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getClassInfo".GetJsonAsync<List<ClassInfo>>();
            var classChoiceobj = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getClassChoiceWithID".PostJsonAsync(new { studentID = StaticPageToPassData.thisStudentInfo.studentID }).ReceiveJson<ClassChoice>();
            
            classChoice = classChoiceobj;

            foreach(var classchoiceName in classList)
            {
                if (classChoice.classID == classchoiceName.classID)
                {
                    className = classchoiceName.name;
                }
            }
            sName = StaticPageToPassData.thisStudentInfo.name;
          
        }

        public void GetInfo()
        {

        }
        #endregion

        #region Binding

        private List<Tag> barChartList1;

        public List<Tag> barChartList { get => barChartList1; set => SetProperty(ref barChartList1, value); }

        private string sName1;

        public string sName { get => sName1; set => SetProperty(ref sName1, value); }

        private string className1;

        public string className { get => className1; set => SetProperty(ref className1, value); }
        #endregion

    }
}
