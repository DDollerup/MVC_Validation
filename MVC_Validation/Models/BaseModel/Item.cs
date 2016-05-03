using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Validation.Models.BaseModel
{
    public class Item
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
    }
}