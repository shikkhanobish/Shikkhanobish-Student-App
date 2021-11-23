using System;
using System.Collections.Generic;
using System.Text;

namespace ShikkhanobishStudentApp.Model
{
    public class Answer
    {
        public string answerID { get; set; }
        public string name { get; set; }
        public string answer { get; set; }
        public string answerDate { get; set; }
        public int userID { get; set; }
        public int userType { get; set; }
        public string imgSrc { get; set; }
        public int review { get; set; }
        public string postID { get; set; }
        public string riviewImg { get; set; }
        public bool editVisible { get; set; }
        public string Response { get; set; }
    }
}
