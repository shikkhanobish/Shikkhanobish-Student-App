using Flurl.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShikkhanobishStudentApp.Model
{
    public class StaticPageToPassData
    {
        public static string RegisteredPhonenNumber;
        public static string thisStPh;
        public static string thisstPass;
        public static string otpcode;
        public static bool isLoginOK;

        public static Student thisStudentInfo;
        public static bool isFromLogin;
        public static bool isInBackground;
        public static string lastTuitionHistoryID { get; set; }
        public static int lastRate { get; set; }
        public static int lastTeacherID { get; set; }
        public static int reportIndex { get; set; }
        public static string reportDes { get; set; }
        public static PerMinPassModel perMinCall { get; set; }
        public static StudentPaymentHistory thispayment { get; set; }
        public static favouriteTeacher selectedPopupFavTeacher { get; set; }
        public static bool isFromReg { get; set; }
        public static string LastPaymentRequestID { get; set; }
        public static PostEvent eventController { get; set; }
        public static PostViewEvent postViewEventStatic { get; set; }


        public static int GenarateNewID()
        {
            Random rnd = new Random();
            int newID = rnd.Next(10000000, 99999999);
            return newID;
        }

        public static string GenarateIDString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static async Task GetStudent()
        {
            thisStudentInfo = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getStudentWithID".PostUrlEncodedAsync(new { studentID = thisStudentInfo.studentID })
  .ReceiveJson<Student>();
        }

        public static async Task OnStart()
        {
            isInBackground = false;
        }

        public static async Task OnPause()
        {
            isInBackground = true;
        }

        public static async Task MakeActiveInServer()
        {
            int i = 0;
            var resn = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setActiveStatus".PostUrlEncodedAsync(new { userID = thisStudentInfo.studentID, activeStatus = 1, type = 1 })
.ReceiveJson<Response>();
            while (i == 0)
            {
                var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/updateActiveStatus".PostUrlEncodedAsync(new { userID = thisStudentInfo.studentID, activeStatus = 1 })
 .ReceiveJson<Response>();
                await Task.Delay(1000);
            }
        }
    }
}