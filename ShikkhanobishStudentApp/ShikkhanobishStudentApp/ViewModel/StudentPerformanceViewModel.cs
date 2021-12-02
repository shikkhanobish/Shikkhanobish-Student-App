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
        List<Tag> tlist = new List<Tag>();
        List<PerformancePrediction> prdFinalList = new List<PerformancePrediction>();

        ClassChoice classChoice = new ClassChoice();
        public StudentPerformanceViewModel()
        {
            getAllInfo();
        }

        public async Task getAllInfo()
        {
            await GetBarList();
            await GetPredictionList();
        }

        #region Methods
       
        public async Task GetBarList()
        {
            tlist = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTag".GetJsonAsync<List<Tag>>();
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
        public async Task GetPredictionList()
        {
            List<PerformancePrediction> prdList = new List<PerformancePrediction>();
           foreach(var item in tlist)
            {
                PerformancePrediction obj = new PerformancePrediction();
                obj.subject = item.tagName;
                obj.predictNumber = 50;
                prdList.Add(obj);
            }
            predictionList = prdList;
        }

        #endregion

        #region Binding

        private List<Tag> barChartList1;

        public List<Tag> barChartList { get => barChartList1; set => SetProperty(ref barChartList1, value); }

        private string sName1;

        public string sName { get => sName1; set => SetProperty(ref sName1, value); }

        private string className1;

        public string className { get => className1; set => SetProperty(ref className1, value); }

        private List<PerformancePrediction> predictionList1;
        public List<PerformancePrediction> predictionList { get => predictionList1; set => SetProperty(ref predictionList1, value); }
        #endregion

    }
}
