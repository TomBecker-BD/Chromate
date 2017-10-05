using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Toaster.WebAPI
{
    [DataContract(Name = "ToasterStatus", Namespace = "")]
    public class ToasterStatus
    {
        [DataMember(Name = "setting")]
        public double setting { get; set; }

        [DataMember(Name = "content")]
        public string content { get; set; }

        [DataMember(Name = "toasting")]
        public bool toasting { get; set; }

        [DataMember(Name = "color")]
        public string color { get; set; }
    }
}
