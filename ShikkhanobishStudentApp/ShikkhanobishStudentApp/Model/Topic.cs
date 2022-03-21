using System;
using System.Collections.Generic;
using System.Text;

namespace ShikkhanobishStudentApp.Model
{
    public class Topic
    {
        public int topicID { get; set; }
        public int chapterID { get; set; }
        public string name { get; set; }
        public int topicIndex { get; set; }
        public bool isTuitionAvailable { get; set; }
        public bool isSavedVideoAvailable { get; set; }
        public bool isTuitionNotAvailable { get; set; }
        public string description { get; set; }
      
    }
}
