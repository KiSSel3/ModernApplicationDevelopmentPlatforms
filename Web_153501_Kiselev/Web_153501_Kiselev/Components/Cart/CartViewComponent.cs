using Microsoft.AspNetCore.Mvc;

namespace Web_153501_Kiselev.Components.Cart
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("CartView");
        }
    }
}
