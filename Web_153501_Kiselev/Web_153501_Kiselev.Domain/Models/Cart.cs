using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_153501_Kiselev.Domain.Entities;

namespace Web_153501_Kiselev.Domain.Models
{
    public class Cart
    {
        public int Count => CartItems.Sum(item => item.Value.Count);
        public decimal TotalPrice => CartItems.Sum(item => item.Value.Vehicle.Price.Value * item.Value.Count);

        public Dictionary<Guid, CartItem> CartItems { get; set; } = new();

        public virtual void AddToCart(Vehicle vehicle)
        {
            if (!CartItems.ContainsKey(vehicle.Id))
            {
                CartItems.Add(vehicle.Id, new CartItem() { Vehicle = vehicle, Count = 1 });
            }
            else
            {
                CartItems[vehicle.Id].Count += 1;
            }
        }

        public virtual void RemoveItems(Guid id)
        {
            CartItems[id].Count -= 1;

            if (CartItems[id].Count <= 0)
            {
                CartItems.Remove(id);
            }
        }

        public virtual void ClearAll()
        {
            CartItems.Clear();
        }
    }
}
