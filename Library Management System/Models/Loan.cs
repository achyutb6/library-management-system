using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library_Management_System.Models
{
    public class Loan
    {
        public string loan_id { get; set; }

        public string isbn { get; set; }

        public string card_id { get; set; }

        public DateTime date_out { get; set; }

        public DateTime due_date { get; set; }

        public DateTime date_in { get; set; }

    }
}