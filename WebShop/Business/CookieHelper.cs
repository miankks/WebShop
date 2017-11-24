using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop.Business
{
    public class CookieHelper
    {

        public HttpCookie Cookie;
        public void GetCookies(string name, string quantity, string size)
        {
            if (HttpContext.Current.Request[name] == null)
            {
                Cookie = new HttpCookie("ShoppingCart")
                {
                    ["Quantity"] = quantity,
                    ["Size"] = size
                };
            }
            else
            {
                Cookie = HttpContext.Current.Request.Cookies["ShoppingCart"];
                if (Cookie != null)
                {
                    Cookie["Quantity"] = quantity;
                    Cookie["Size"] = size;
                }
            }
            if (Cookie != null)
            {
                Cookie.Expires = DateTime.Now.AddDays(4);
                HttpContext.Current.Response.Cookies.Add(Cookie);
            }
        }

    }
}
        //public void AddSelectedProducts(HttpContextBase context, int productPage, string productId)
        //{
        //    var cookie = this.GetProductOrderCookie(context, productPage);
        //    var cookieProducts = cookie.Value;

        //    if (!string.IsNullOrWhiteSpace(cookieProducts))
        //    {
        //        var products = cookieProducts.Split(',').ToList();
        //        if (products != null)
        //        {

        //            products.Add(productId);

        //        }

        //        cookieProducts = String.Join(",", products.ToArray());
        //    }
        //    else
        //    {
        //        cookieProducts = productId;
        //    }

        //    cookie.Value = cookieProducts;
        //    cookie.Expires = DateTime.UtcNow.AddDays(3);

        //    context.Response.SetCookie(cookie);
        //}