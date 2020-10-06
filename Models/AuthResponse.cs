using System;
using System.Collections.Generic;
using System.Text;

namespace WPFCore.Models
{
    public class AuthResponse
    {
        public string userid { get; set; }
        public Token token { get; set; }

        public string restaurantid { get; set; }
    }
}
