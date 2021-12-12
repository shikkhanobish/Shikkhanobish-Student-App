﻿using System;
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
        public bool tinfoVisible { get; set; }
        public string newComment { get; set; }
        public int upVoteCount { get; set; }
        public int downVoteCount { get; set; }
        public string upBackColor { get; set; }
        public string downBackColor { get; set; }
        public bool voteFrameVisibility { get; set; }
        public string Response { get; set; }
    }
}
