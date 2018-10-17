using System;
using DepiBelle.Models;

namespace DepiBelle.Managers.Cart
{
    public interface ICartManager<T> where T : Purchasable
    {
        EventHandler<CartItem<T>> ItemAddedEventHandler { get; set; }

        EventHandler<string> ItemRemoved { get; set; }

    }
}
