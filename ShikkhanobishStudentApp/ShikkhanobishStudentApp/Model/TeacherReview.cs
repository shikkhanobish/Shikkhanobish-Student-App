using System;
using System.Collections.Generic;
using System.Text;

namespace ShikkhanobishStudentApp.Model
{
    public class TeacherReview
    {
        public string reviewID { get; set; }
        public int teacherID { get; set; }
        public int studentID { get; set; }
        public string review { get; set; }
        public string tuitionID { get; set; }
        public string Response { get; set; }
    }
}
