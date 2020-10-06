using System;
using System.Collections.Generic;
using System.Text;

namespace WPFCore.Models
{
    public class Product
    {
        public string name { get; set; }
        public string description { get; set; }
        public float price { get; set; }
        public Category categoryid{ get; set; }

    }
}
