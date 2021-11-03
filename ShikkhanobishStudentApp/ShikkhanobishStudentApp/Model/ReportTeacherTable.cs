using System;
using System.Collections.Generic;
using System.Text;

namespace ShikkhanobishStudentApp.Model
{
    public class ReportTeacherTable
    {
        public int reportID { get; set; }
        public int studentID { get; set; }
        public int teacherID { get; set; }
        public int reportIndex { get; set; }
        public string description { get; set; }

        public string Response { get; set; }
    }
}
