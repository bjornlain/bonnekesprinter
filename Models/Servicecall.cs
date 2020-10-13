using System;
using System.Collections.Generic;
using System.Text;
using WPFCore.Models;

namespace bonnekesprinter.Models
{
    public class Servicecall
    {
        public string _id { get; set; }
        public Boolean status { get; set; }

        public string description { get; set; }

        public string tablenumber { get; set; }

        public string tableid { get; set; }
        public string name { get; set; }
        public string restaurantid { get; set; }
    }
}
