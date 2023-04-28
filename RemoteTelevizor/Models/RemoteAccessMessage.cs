﻿using System;
using System.Collections.Generic;
using System.Text;


namespace RemoteTelevizor.Models
{
    public class RemoteAccessMessage : JSONObject
    {
        public string securityKey { get; set; }
        public string command { get; set; }
        public string commandArg1 { get; set; }
        public string commandArg2 { get; set; }
    }
}