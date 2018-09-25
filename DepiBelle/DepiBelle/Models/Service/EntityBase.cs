using System;
using Newtonsoft.Json;

namespace DepiBelle.Models
{
    //TODO entities must inherith from this class
    public class EntityBase
    {
        [JsonIgnore]
        public string Id { get; set; }

    }
}
