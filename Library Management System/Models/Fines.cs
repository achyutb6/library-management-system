using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library_Management_System.Models
{
    public class Fines
    {
        public string loan_id { get; set; }

        public Double fine_amt { get; set; }

        public bool paid { get; set; }
    }
}