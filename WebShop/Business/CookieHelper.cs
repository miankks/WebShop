using System;
using System.Web;
using Newtonsoft.Json;
using WebShop.Models.Pages;

namespace WebShop.Business
{
    public class CookieHelper
    {
       
        public HttpCookie Cookie;

        public CookieCart GetCartFromCookie()
        {
            if (HttpContext.Current.Request.Cookies["ShoppingCart"] != null)
            {
                Cookie = HttpContext.Current.Request.Cookies["ShoppingCart"];
                    //if (Cookie != null && !string.IsNullOrWhiteSpace(Cookie.Value))
                if (!string.IsNullOrWhiteSpace(Cookie?.Value))
                {
                    CookieCart deserializedCookieCart = JsonConvert.DeserializeObject<CookieCart>(Cookie.Value);
                    
                    return deserializedCookieCart;
                }
            }

            return new CookieCart();
        }

        public void SaveCartToCookie(CookieCart cart)
        {
            string cartAsJsonString = JsonConvert.SerializeObject(cart);

            if (HttpContext.Current.Request.Cookies["ShoppingCart"] == null)
            {
                Cookie = new HttpCookie("ShoppingCart")
                {
                    Value = cartAsJsonString
                };
            }
            else
            {
                Cookie = HttpContext.Current.Request.Cookies["ShoppingCart"];

                if (Cookie != null)
                {
                    Cookie.Value = cartAsJsonString;
                }
            }

            if (Cookie != null)
            {
                Cookie.Expires = DateTime.Now.AddDays(4);
                HttpContext.Current.Response.Cookies.Add(Cookie);

            }
        }

        public void UpdateSelectedCookie(CookieCart cart, int pageImageId)
        {
            HttpContext.Current.Response.SetCookie(Cookie);
        }
    }
}
