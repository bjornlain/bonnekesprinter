using System;
using System.Collections.Generic;
using System.Text;

namespace WPFCore.Models
{
    public class ChosenExtra
    {
        public string name{ get; set; }
        public float? price { get; set; }
       
        public Option option { get; set; }
        public Restaurant restaurantid { get; set; }
    }
}
