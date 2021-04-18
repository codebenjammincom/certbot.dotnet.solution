using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace godaddy.api.console
{
    public class DnsEntry
    {
        [JsonPropertyName("data")]
        public string Data { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("ttl")]
        public int TTL { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

    }

}
