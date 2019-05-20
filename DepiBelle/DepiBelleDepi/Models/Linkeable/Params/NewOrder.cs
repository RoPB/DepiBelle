using System;
using Newtonsoft.Json;

namespace DepiBelleDepi.Models
{
    public class NewOrder
    {
        [JsonProperty("orderId")]
        public string OrderId
        {
            get;
            set;
        }
    }
}
