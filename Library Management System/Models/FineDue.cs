using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library_Management_System.Models
{
    public class FineDue
    {
        public string card_id { get; set; }

        public string bname { get; set; }

        public double fine_due { get; set; }
    }
}