using Microsoft.AspNetCore.Mvc;
using Web_153501_Kiselev.Domain.Models;

namespace Web_153501_Kiselev.Components.Cart
{
    public class CartViewComponent : ViewComponent
    {
        private readonly Web_153501_Kiselev.Domain.Models.Cart _cart;

        public CartViewComponent(Web_153501_Kiselev.Domain.Models.Cart cart) =>
            (_cart) = (cart);
        public IViewComponentResult Invoke()
        {
            return View("CartView", _cart);
        }
    }
}
