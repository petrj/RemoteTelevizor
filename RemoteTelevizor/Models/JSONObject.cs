using System;
using System.IO;
using Newtonsoft.Json;

namespace RemoteTelevizor
{
    public class JSONObject
    {
        public string status { get; set; }
        public string error { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
