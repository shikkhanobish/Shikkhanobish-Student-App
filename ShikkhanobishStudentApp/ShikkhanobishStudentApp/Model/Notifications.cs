using System;
using System.Collections.Generic;
using System.Text;

namespace ShikkhanobishStudentApp.Model
{
    public class Notifications
    {
        public string notificationID { get; set; }
        public int userId { get; set; }
        public int userType { get; set; }
        public int notificationType { get; set; }
        public string description { get; set; }
        public string refIDOne { get; set; }
        public string refIDTwo { get; set; }
        public string refIDThree { get; set; }
        public string refIDFour { get; set; }
        public string notificationDate { get; set; }
        public string titleColor { get; set; }
        public string titleName { get; set; }
        public string spanOne { get; set; }
        public string spanTwo { get; set; }
        public string spanThree { get; set; }

    }
}
