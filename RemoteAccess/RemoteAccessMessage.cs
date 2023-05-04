using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteAccess
{
    public class RemoteAccessMessage
    {
        public string command { get; set; }

        public string commandArg1 { get; set; }
        public string commandArg2 { get; set; }

        public override string ToString()
        {
            return $"Message: Command: {command}";
        }
    }
}
