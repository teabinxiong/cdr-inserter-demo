using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdr.Inserter.Workers.Models
{
    public sealed class CdrRecord
    {
        // MSIDN:IMSI:IMEI:PLAN:CALL_TYPE:CORRESP_TYPE:CORRESP_ISDN:DURATION:TIME:DATE
        [JsonProperty("MSIDN")]
        public string Msidn { get; set; }

        [JsonProperty("IMSI")]
        public string Imsi { get; set; }

        [JsonProperty("IMEI")]
        public string Imei { get; set; }

        [JsonProperty("PLAN")]
        public string Plan { get; set; }

        [JsonProperty("CALL_TYPE")]
        public string CallType { get; set; }

        [JsonProperty("CORRESP_TYPE")]
        public string CorrespType { get; set; }

        [JsonProperty("CORRESP_ISDN")]
        public string CorrespIsdn { get; set; }

        [JsonProperty("DURATION")]
        public int Duration { get; set; }

        [JsonProperty("TIME")]
        public string Time { get; set; }

        [JsonProperty("DATE")]
        public string Date { get; set; }

    }
}
