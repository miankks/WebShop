﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Core;
using WebShop.Models.ViewModels;

namespace WebShop.Models.Pages
{
    public class CookieCart
    {
        public CookieCart()
        {
            this.CartItems = new List<CartCookieItem>();
        }

        public List<CartCookieItem> CartItems { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public class CartCookieItem : IContentData
        {
           
            public string Size { get; set; }
            public string NumberOfItems { get; set; }
            public double Price { get; set; }
            public string ProductName { get; set; }
            public int ImageId { get; set; }
            public double TotalAmount { get; set; }
            public double TotalMoms
            {
                get { return Price * .25; }
                set { }
            }

            public PropertyDataCollection Property { get; }
        }
    }
}