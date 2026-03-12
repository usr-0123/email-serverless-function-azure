using System.Collections.Generic;

namespace EmailFunction.Models
{
    public class EmailRequest
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string TemplateType { get; set; }
        public Dictionary<string,string> Data { get; set; }
    }
}