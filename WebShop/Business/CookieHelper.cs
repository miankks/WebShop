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
    }
}


                //Cookie = HttpContext.Current.Request.Cookies["ShoppingCart"];
                //    Cookie = new HttpCookie("ShoppingCart");
                //    var products = Cookie.Value.Split(',').ToList();
                //    products.Add(output);
                //    Cookie.Value = String.Join(",", products.ToArray());











            //if (HttpContext.Current.Request["ShoppingCart"] == null)
            //{
            //if (!string.IsNullOrWhiteSpace(Cookie.Value))
            //{
            //    Cookie = new HttpCookie("ShoppingCart");
            //    var products = Cookie.Value.Split(',').ToList();
            //    products.Add(output);

            //    Cookie.Value = String.Join(",", products.ToArray());
            //}
            //else
            //{
            //    Cookie.Value = output;
            //}
            ////}

            //if (Cookie != null)
            //{
            //    Cookie.Expires = DateTime.Now.AddDays(4);
            //    HttpContext.Current.Response.Cookies.Add(Cookie);
            //}


        //public void Cookies_Set(string key, string value)
        //{
        //    HttpCookie cookie = new HttpCookie(key, value);
        //    cookie.Expires = DateTime.Now.AddDays(4);

        //    HttpContext.Current.Response.SetCookie(cookie);
        //}


        //public string Cookies_Get(string key)
        //{
        //    string value = "";
        //    if (HttpContext.Current.Request.Cookies.AllKeys.Contains(key))
        //    {
        //        value = HttpContext.Current.Request.Cookies[key]?.Value;
        //    }

        //    return value;
        //}
  



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