using System;
using System.Collections.Generic;
using System.Text;

namespace ShikkhanobishStudentApp.Model
{
    public class Post
    {
        public string postID { get; set; }
        public string name { get; set; }
        public string post { get; set; }
        public string postDate { get; set; }
        public int userID { get; set; }
        public int userType { get; set; }
        public string imgSrc { get; set; }
        public string postTitle { get; set; }
        public int noOfComment { get; set; }
        public int numOFCmtR { get; set; }
        public int numOFCmtN { get; set; }
        public bool imgButtonEnable { get; set; }
        public int tagID { get; set; }
        public string tagName { get; set; }
        public string ansIconR { get; set; }
        public string ansIconN { get; set; }
        public string dotDotDot { get; set; }
        public string Response { get; set; }
        
    }
}
