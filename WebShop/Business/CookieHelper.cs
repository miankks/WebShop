﻿using System;
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
        public void GetCookies( string size, string quantity, double price, int productPageId)
        {
           
            if (HttpContext.Current.Request["ShoppingCart"] == null)
            {
                
                Cookie = new HttpCookie("ShoppingCart")
                {

                    ["Size"] = size,
                    ["Quantity"] = quantity,
                    ["Price"] = Convert.ToString(price),
                    ["pageId"] = Convert.ToString(productPageId),
                    //["Moms"] = Convert.ToString(moms),
                    //Value = size + "   " + quantity + "   " + price 

                };
            }
            else
            {
                Cookie = HttpContext.Current.Request.Cookies["ShoppingCart"];
               //HttpContext.Current.Response.Cookies["value"].Value =
               //     quantity + size + Convert.ToString(price) + Convert.ToString(productPageId) +
               //     Convert.ToString(productPageId);
                if (Cookie != null)
                {
                    Cookie.Values["Quantity"] = quantity;
                    Cookie.Values["Size"] = size;
                    Cookie.Values["Price"] = Convert.ToString(price);
                    //Cookie["Moms"] = Convert.ToString(moms);
                    //Cookie.Values["pageId"] = Convert.ToString(productPageId);
                    //Cookie.Value = Cookie.Value + "|" + size + "   " + quantity + "   " + price;
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
        //public void GetCookies(int productPageId)
        //{
        //    if (productPageId > 0)
        //    {
        //        var reference = new ContentReference(productPageId);
        //        var shoppingPage = this._contentRepository.Get<ShoppingPage>(reference);
        //        if (shoppingPage != null)
        //        {
        //            // TODO: Add to cart
        //        }
        //    }
            //if (HttpContext.Current.Request[name] == null)
            //{
            //    Cookie = new HttpCookie("ShoppingCart")
            //    {
            //        ["Quantity"] = quantity,
            //        ["Size"] = size,
            //        ["Price"] = Convert.ToString(price)
            //    };
            //}
            //else
            //{
            //    Cookie = HttpContext.Current.Request.Cookies["ShoppingCart"];
            //    if (Cookie != null)
            //    {
            //        Cookie["Quantity"] = quantity;
            //        Cookie["Size"] = size;
            //        Cookie["Price"] = Convert.ToString(price);

            //    }
            //}
            //if (Cookie != null)
            //{
            //    Cookie.Expires = DateTime.Now.AddDays(4);
            //    HttpContext.Current.Response.Cookies.Add(Cookie);
            //}
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