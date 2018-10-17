using System;
using DepiBelle.Models;

namespace DepiBelle.Services.Notification
{
    public interface ICartNotificationService<T> where T : Purchasable
    {
        EventHandler<CartItem<T>> ItemAddedEventHandler { get; set; }

        EventHandler<string> ItemRemoved { get; set; }

    }
}
