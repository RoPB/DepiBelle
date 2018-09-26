using System;
namespace DepiBelle.Models
{

    public enum SubscriptionEventType {InsertOrUpdate,Delete}

    public class ServiceSubscriberEventParam<T>
    {

        public SubscriptionEventType Type
        {
            get;
            set;
        }

        public T Item
        {
            get;
            set;
        }
    }
}
