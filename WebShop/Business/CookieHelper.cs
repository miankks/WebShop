using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer;
using EPiServer.Core;
using Microsoft.Ajax.Utilities;
using SendGrid;
using WebShop.Models.Pages;

namespace WebShop.Business
{
    public class CookieHelper
    {
        private readonly IContentRepository _contentRepository;

        public CookieHelper()
        {
        }
        public CookieHelper(IContentRepository contentRepository)
        {
            this._contentRepository = contentRepository;
        }

        public HttpCookie Cookie;
        public void GetCookies(string output)
        {
            if (HttpContext.Current.Request["ShoppingCart"] == null)
            {

                Cookie = new HttpCookie("ShoppingCart")
                {
                    Value = output
                };
            }
            else
            {
                Cookie = HttpContext.Current.Request.Cookies["ShoppingCart"];
                if (Cookie != null)
                {
                    Cookie.Value =  output;
                }
            }
            if (Cookie != null)
            {
                Cookie.Expires = DateTime.Now.AddDays(4);
                HttpContext.Current.Response.Cookies.Add(Cookie);
            }
        }

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