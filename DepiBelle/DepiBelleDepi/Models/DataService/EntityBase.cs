using System;
using Newtonsoft.Json;

namespace DepiBelleDepi.Models
{
    //TODO entities must inherith from this class
    public class EntityBase
    {
        [JsonIgnore]
        public string Id { get; set; }

        public EntityBase()
        {

        }

        public EntityBase(string id)
        {
            this.Id = id;
        }

    }
}
