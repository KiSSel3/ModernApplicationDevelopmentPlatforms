using System.Text.Json.Serialization;
using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.Domain.Models;
using Web_153501_Kiselev.Extensions;

namespace Web_153501_Kiselev.Services.CartService
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;
            SessionCart cart = session?.Get<SessionCart>("Cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }

        [JsonIgnore]
        public ISession? Session { get; set; }
        public override void AddToCart(Vehicle vehicle)
        {
            base.AddToCart(vehicle);
            Session?.Set("Cart", this);
        }

        public override void RemoveItems(Guid id)
        {
            base.RemoveItems(id);
            Session?.Set("Cart", this);
        }

        public override void ClearAll()
        {
            base.ClearAll();
            Session?.Remove("Cart");
        }
    }
}
