using System;
using System.Collections.Generic;
using System.Text;

namespace ShikkhanobishStudentApp.Model
{
    public class ReferralTable
    {
        public string referralID { get; set; }
        public int studentID { get; set; }
        public int referredStudentID { get; set; }
        public string referralDate { get; set; }
        public string Response { get; set; }
    }
}
