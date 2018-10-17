using System;
using DepiBelle.Models;

namespace DepiBelle.Services.Notification
{
    public class CartNotificationService<T> : ICartNotificationService<T> where T:Purchasable
    {
        public EventHandler<CartItem<T>> ItemAdded { get; set; }

        public EventHandler<string> ItemRemoved { get; set; }
    }
}
