using System;
using System.Collections.Generic;
using System.Text;

namespace WPFCore.Models
{
    public class Restaurant
    {

        public string _id { get; set; }
        public string name { get; set; }

        public string street{ get; set; }

        public string streetnumber { get; set; }

        public int postalcode { get; set; }
        public string city { get; set; }

        public string website { get; set; }
        public string information { get; set; }
        public bool printOut { get; set; }
        public string printerUrl { get; set; }

        public bool metBonnen { get; set; }
    }
}
