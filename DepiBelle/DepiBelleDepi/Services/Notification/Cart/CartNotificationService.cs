using System;
using DepiBelleDepi.Models;

namespace DepiBelleDepi.Services.Notification.Cart
{
    public class CartNotificationService<T> : ICartNotificationService<T> where T : Purchasable
    {
        public EventHandler<CartItem<T>> ItemAdded { get; set; }

        public EventHandler<string> ItemRemoved { get; set; }
    }
}
