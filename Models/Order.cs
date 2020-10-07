using System;
using System.Collections.Generic;
using System.Text;

namespace WPFCore.Models
{
    public class Order
    {
        public string customerid { get; set; }
        public Product product{ get; set; }
        public int amount { get; set; }
        public string tableid{ get; set; }

        public ChosenExtra[] chosenExtra { get; set; }

        public string restaurantid { get; set; }
    }
}
