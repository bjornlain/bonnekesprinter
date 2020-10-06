using System;
using System.Collections.Generic;
using System.Text;

namespace WPFCore.Models
{
    public class Category
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool available { get; set; }
        public string description { get; set; }

        public string restaurantid { get; set; }
    }
}
