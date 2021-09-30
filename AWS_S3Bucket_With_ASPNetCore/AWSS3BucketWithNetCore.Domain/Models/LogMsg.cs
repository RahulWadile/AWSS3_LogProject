using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AWSS3BucketWithNetCore.Domain.Models
{
    [DataContract]
    public class LogMsg
    {
        [DataMember(Name = "LogMessage")]
        public string LogMessage { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public string CreatedDateTime { get; set; }
    }

    
}
