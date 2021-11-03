using System;
using System.Collections.Generic;
using System.Text;

namespace ShikkhanobishStudentApp.Model
{
    public class StudentReport
    {
        public int studentReportID { get; set; }
        public int reportType { get; set; }
        public string description { get; set; }
        public int studentID { get; set; }
        public string teacherName { get; set; }
        public int teacherID { get; set; }
        public string ReportTypeText { get; set; }
        public string date { get; set; }
        public string Response { get; set; }
    }
}
