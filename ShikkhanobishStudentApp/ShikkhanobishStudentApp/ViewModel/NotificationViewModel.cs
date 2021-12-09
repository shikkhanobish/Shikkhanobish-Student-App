using Flurl.Http;
using Microsoft.AspNetCore.SignalR.Client;
using ShikkhanobishStudentApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace ShikkhanobishStudentApp.ViewModel
{
    public class NotificationViewModel : BaseViewModel, INotifyPropertyChanged
    {
        List<Notifications> nList = new List<Notifications>();
        List<TuiTionLog> tuitonList = new List<TuiTionLog>();
        List<TutionRequestCount> tutionreqList = new List<TutionRequestCount>();
        List<Teacher> teacherList = new List<Teacher>();
        List<Subject> subList = new List<Subject>();
        List<Chapter> chList = new List<Chapter>();
        List<Post> postList = new List<Post>();
        List<Answer> ansList = new List<Answer>();

        public NotificationViewModel()
        {
            GetAll();
        }

        public async Task GetAll()
        {
            await GetNotification();
            await ConnectToRealTimeApiServer();
        }

        #region Methods
        public async Task GetNotification()
        {
            using (var dialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Please Wait..."))
            {
                nList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getNotification".GetJsonAsync<List<Notifications>>();
                tuitonList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTuiTionLogNeW".GetJsonAsync<List<TuiTionLog>>();
                tutionreqList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTuitionRequestCount".GetJsonAsync<List<TutionRequestCount>>();
                teacherList = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getAllTeacher".PostJsonAsync(new { }).ReceiveJson<List<Teacher>>();
                subList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getSubject".GetJsonAsync<List<Subject>>();
                chList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getChapter".GetJsonAsync<List<Chapter>>();

                postList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getPost".GetJsonAsync<List<Post>>();
                ansList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getAnswer".GetJsonAsync<List<Answer>>();

                List<Notifications> updatedList = new List<Notifications>();

                foreach (var item in nList)
                {
                    if (item.userId == StaticPageToPassData.thisStudentInfo.studentID)
                    {
                        if (item.notificationType == 1)
                        {
                            foreach (var tuition in tuitonList)
                            {
                                if (item.refIDOne == tuition.tuitionLogID)
                                {

                                    foreach (var sub in subList)
                                    {

                                        foreach (var chap in chList)
                                        {
                                            if (tuition.subjectID == sub.subjectID && tuition.chapterID == chap.chapterID)
                                            {
                                                item.notificationDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                                                item.titleName = "Tuition";

                                                item.spanTwo = " accepted your tuition request on ";
                                                item.titleColor = "#0D94D5";

                                                item.spanThree = "Subject - " + sub.name + " , " + "Chapter - " + chap.name;

                                                foreach (var tuitionReq in tutionreqList)
                                                {
                                                    foreach (var teacher in teacherList)
                                                    {
                                                        if (tuitionReq.tuitionID == tuition.tuitionLogID && tuitionReq.teacherID == teacher.teacherID)
                                                        {
                                                            item.spanOne = teacher.name;

                                                        }
                                                    }

                                                }
                                                updatedList.Add(item);
                                            }

                                        }
                                    }
                                }
                            }
                        }

                        else if (item.notificationType == 2)
                        {
                            foreach (var ans in ansList)
                            {
                                if (item.refIDOne == ans.answerID)
                                {

                                    foreach (var post in postList)
                                    {
                                        if (ans.postID == post.postID)
                                        {
                                            item.notificationDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                                            item.titleName = "Question";
                                            item.spanTwo = " answered your question.";
                                            item.titleColor = "#A20DD5";

                                            item.spanOne = ans.name;

                                            item.spanThree = " Question : " + post.post;
                                            updatedList.Add(item);
                                        }
                                    }

                                }

                            }

                        }

                    }

                }
                notificationList = updatedList;
            }
        }


        HubConnection _connection = null;
        string url = "https://shikkhanobishRealTimeAPi.shikkhanobish.com/ShikkhanobishHub";

        public async Task ConnectToRealTimeApiServer()
        {

            _connection = new HubConnectionBuilder()
                 .WithUrl("https://shikkhanobishRealTimeAPi.shikkhanobish.com/ShikkhanobishHub")
                 .Build();
            try
            {
                await _connection.StartAsync();
            }
            catch (Exception ex)
            {
                var ss = ex.InnerException;
            }


            _connection.Closed += async (s) =>
            {
                await _connection.StartAsync();
            };


            _connection.On<int, string, string, int>("TuitionRequestNotification", async (teacherID, notificationID, tuitionID, studentID) =>
            {
                if (studentID == StaticPageToPassData.thisStudentInfo.studentID)
                {

                }

            });

            _connection.On<int, string, string, string, int>("PostAnswerNotification", async (teacherID, notificationID, postID, answerID, studentID) =>
            {
                if (studentID == StaticPageToPassData.thisStudentInfo.studentID)
                {
                    //show notification
                }

            });

        }

        public async Task PerformnotificationObject(Notifications notifi)
        {
            notification = notifi;
        }

        #endregion

        #region Bindings
        private ICommand notificationObject1;
        public ICommand notificationObject
        {
            get
            {
                return new Command<Notifications>(async (n) =>
                {

                    await PerformnotificationObject(n);
                });

            }
        }

        private Notifications notification1;

        public Notifications notification { get => notification1; set => SetProperty(ref notification1, value); }

        private List<Notifications> notificationList1;

        public List<Notifications> notificationList { get => notificationList1; set => SetProperty(ref notificationList1, value); }

        #endregion


    }
}
