using System;
using System.Collections.Generic;
using System.Text;

namespace ShikkhanobishStudentApp.Model
{
    public class VoucherHistory
    {
        public int studentID { get; set; }
        public int voucherHistoryID { get; set; }
        public int paymentID { get; set; }
        public string Response { get; set; }
    }
}
