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
        public NotificationViewModel()
        {
            GetNotification();
        }

        #region Methods
        public async Task GetNotification()
        {

            nList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getNotification".GetJsonAsync<List<Notifications>>();
            List<Notifications> updatedList = new List<Notifications>();
            foreach (var item in nList)
            {
                if (item.notificationType==1)
                {
                    item.titleColor = "#0D94D5";
                }
                if (item.notificationType == 2)
                {
                    item.titleColor = "#A20DD5";
                }
                if (item.notificationType == 3)
                {
                    item.titleColor = "#0D6AD5";
                }
                if (item.notificationType == 4)
                {
                    item.titleColor = "#D5800D";
                }
                updatedList.Add(item);
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
