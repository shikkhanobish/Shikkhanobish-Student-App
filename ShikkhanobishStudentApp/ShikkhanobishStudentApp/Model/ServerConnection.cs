using System;
using System.Collections.Generic;
using System.Text;
using Flurl.Http;
using System.Threading.Tasks;

namespace ShikkhanobishStudentApp.Model
{
    public class ServerConnection<T1,T2>
    {

        string baseURLStudent = "https://api.shikkhanobish.com/api/ShikkhanobishLogin/";
        string baseURLTeacher = "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/";
        public ServerConnection()
        {

        }
        public async Task<T1> postMethodStudnetWithReturn(object parameterObject, string apiCallName)
        {
             var st = await (baseURLStudent + apiCallName).PostUrlEncodedAsync(parameterObject)
                 .ReceiveJson<T1>();
            return st;
        }
        public async Task postMethodTeacher(object ob)
        {
            var st = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getStudentWithID".PostUrlEncodedAsync(ob)
                .ReceiveJson<Student>();
        }
    }
}
