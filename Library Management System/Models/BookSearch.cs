using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library_Management_System.Models
{
    public class BookSearch
    {
        public string ISBN { get; set; }

        public string Title { get; set; }

        public string Authors { get; set; }

        public Boolean availability { get; set; }
    }
}