using System;
using DepiBelleDepi.Models;

namespace DepiBelleDepi.Services.Notification.Cart
{
    public interface ICartNotificationService<T> where T : Purchasable
    {
        EventHandler<CartItem<T>> ItemAdded { get; set; }

        EventHandler<string> ItemRemoved { get; set; }

    }
}
