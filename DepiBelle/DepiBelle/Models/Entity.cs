using System;
using Newtonsoft.Json;

namespace DepiBelle.Models
{
    //TODO entities must inherith from this class
    public class Entity
    {
        [JsonIgnore]
        public string id { get; set; }

    }
}
