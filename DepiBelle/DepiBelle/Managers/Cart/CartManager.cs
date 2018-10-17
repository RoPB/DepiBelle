using System;
using DepiBelle.Models;

namespace DepiBelle.Managers.Cart
{
    public class CartManager<T> : ICartManager<T> where T:Purchasable
    {
        public EventHandler<CartItem<T>> ItemAddedEventHandler { get; set; }

        public EventHandler<string> ItemRemoved { get; set; }
    }
}
