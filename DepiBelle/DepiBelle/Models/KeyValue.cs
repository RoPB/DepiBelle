using System;
namespace DepiBelle.Models
{
    public class KeyValue<T>:Entity
    {
        public T value
        {
            get;
            set;
        }
    }
}
