using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShikkhanobishStudentApp.Model
{
    public class RealTimeApiMethods
    {
        public async Task ExecuteRealTimeApi(string uri)
        {
            HttpClient client = new HttpClient();
            StringContent content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(uri, content).ConfigureAwait(true);
        }
    }
}
