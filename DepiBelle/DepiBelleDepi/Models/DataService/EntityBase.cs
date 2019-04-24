using System;
using Newtonsoft.Json;
using Plugin.CloudFirestore.Attributes;

namespace DepiBelleDepi.Models
{
    //TODO entities must inherith from this class
    public class EntityBase
    {
        [Id]
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
