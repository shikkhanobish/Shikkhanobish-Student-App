using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ShikkhanobishStudentApp.Model
{
    public class OnCallingmsg
    {
        public string name { get; set; }
        public string msg { get; set; }
       public  Color lblColor { get; set; }
        public string date { get; set; }
        public bool isThisUser { get; set; }
        public bool isOtherUser { get; set; }
    }
}
