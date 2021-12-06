using Flurl.Http;
using ShikkhanobishStudentApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

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
            GetNotification();
        }

        #region Methods
        public async Task GetNotification()
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
                                            item.notificationDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                                            item.titleName = "Tuition";
                                            
                                            item.spanTwo = " accepted your tuition request on ";
                                            item.titleColor = "#0D94D5";

                                            item.spanThree ="Subject - "+ sub.name + " , "+ "Chapter - " + chap.name;
                                          
                                            foreach (var tuitionReq in tutionreqList)
                                            {
                                                foreach (var teacher in teacherList)
                                                {
                                                    if (tuitionReq.tuitionID == tuition.tuitionLogID && tuitionReq.teacherID==teacher.teacherID)
                                                    {
                                                        item.spanOne =teacher.name;
                                                        
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
                            if (item.refIDTwo== ans.answerID)
                            {
                                
                                foreach (var post in postList)
                                {
                                    if (ans.postID == post.postID)
                                    {
                                        item.notificationDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                                        item.titleName = "Question";
                                        item.spanTwo = " answered your question.";
                                        item.titleColor = "#A20DD5";

                                        item.spanOne = ans.name;

                                        item.spanThree = " Question : "+post.post;
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
        #endregion

        #region Bindings
        private List<Notifications> notificationList1;

        public List<Notifications> notificationList { get => notificationList1; set => SetProperty(ref notificationList1, value); }

        #endregion


    }
}
