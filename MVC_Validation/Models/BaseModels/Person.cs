using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Validation.Models.BaseModels
{
    public class Person
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
    }
}