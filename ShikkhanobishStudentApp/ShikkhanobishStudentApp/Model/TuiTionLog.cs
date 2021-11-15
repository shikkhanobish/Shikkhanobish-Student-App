using System;
using System.Collections.Generic;
using System.Text;

namespace ShikkhanobishStudentApp.Model
{
   public  class TuiTionLog
    {
        public string tuitionLogID { get; set; }
        public string studentName { get; set; }
        public string subjectName { get; set; }
        public string chapterName { get; set; }
        public int subjectID { get; set; }
        public string description { get; set; }
        public string date { get; set; }
        public int studentID { get; set; }
        public int tuitionLogStatus { get; set; }
        public int pendingTeacherID { get; set; }
        public string teacherName { get; set; } //"---"
        public bool isPendingTeacherAvailable { get; set; }
        public string Response { get; set; }
    }
}
