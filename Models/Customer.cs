using System;
using System.Collections.Generic;
using System.Text;

namespace WPFCore.Models
{
    public class Customer 
    {
        public string _id { get; set; }
        public Table table { get; set; }
        public bool paid { get; set; }
        public DateTime time{ get; set; }

        public Order[] orders { get; set; }

        public string[] comments { get; set; }
        public bool delivered { get; set; }

        public Restaurant restaurantid { get; set; }
    }
}
