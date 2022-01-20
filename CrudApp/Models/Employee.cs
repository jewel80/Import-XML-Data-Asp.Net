using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrudApp.Models
{
    public class Employee
    {
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string title { get; set; }
        public string division { get; set; }
        public string building { get; set; }
        public string room { get; set; }
    }
}