using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library_Management_System.Models
{
    public class BookLoan
    {
        public string Loan_ID { get; set; }

        public string ISBN { get; set; }

        public string Borrower_Name { get; set; }

        public string Card_ID { get; set; }

        public string Date_Out { get; set; }

        public string Due_Date { get; set; }
    }
}