using System;
using System.Collections.Generic;
using System.Text;
using Flurl.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using ShikkhanobishStudentApp.Model;

namespace ShikkhanobishStudentApp.Server_Connection
{
    public class ServerConnection
    {
        public async Task<ObservableCollection<Institution>> GetInstitution()
        {
            ObservableCollection<Institution> institutionList = new ObservableCollection<Institution>();
            institutionList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getInstitution".GetJsonAsync<ObservableCollection<Institution>>();
            return institutionList;
        }
    }
}
