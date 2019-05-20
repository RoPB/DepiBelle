using System;
using Newtonsoft.Json;

namespace DepiBelleDepi.Models
{
    public enum LinkeableItemType { DeepLink=1, WebView=0, Browser=2 }

    public class LinkeableItem : EntityBase
    {
        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("type")]
        public LinkeableItemType Type { get; set; }

        public DeepLink DeepLink
        {
            get
            {
                try
                {
                    return Type.Equals(LinkeableItemType.DeepLink) && !string.IsNullOrEmpty(Link) ?
                                        JsonConvert.DeserializeObject<DeepLink>(Link) : null;
                }
                catch(Exception ex)
                {
                    return null;
                }
            }
        }            

    }
}
