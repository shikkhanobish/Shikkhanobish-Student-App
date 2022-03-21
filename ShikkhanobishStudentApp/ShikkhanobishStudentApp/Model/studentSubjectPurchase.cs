using System;
using System.Collections.Generic;
using System.Text;

namespace ShikkhanobishStudentApp.Model
{
    public class studentSubjectPurchase
    {
        public int studentID { get; set; }
        public int subjectID { get; set; }
        public int chapterID { get; set; }
        public string date { get; set; }
        public string Response { get; set; }
    }
}
