using System;
using System.Collections.Generic;
using System.Text;

namespace ShikkhanobishStudentApp.Model
{
    public class AnswerVote
    {
        public int ansvoteID { get; set; }
        public string answerID { get; set; }
        public int userID { get; set; }
        public int upOrdownVote { get; set; }
    }
}
