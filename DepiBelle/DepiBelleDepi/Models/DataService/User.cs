using System;
using Newtonsoft.Json;
using Plugin.CloudFirestore.Attributes;

namespace DepiBelleDepi.Models
{
    public class User:EntityBase
    {
        [MapTo("email")]
        [JsonProperty("email")]
        public string Email { get; set; }
        [MapTo("name")]
        [JsonProperty("name")]
        public string Name { get; set; }
        [MapTo("pushToken")]
        [JsonProperty("pushToken")]
        public string PushToken { get; set; }
    }
}
