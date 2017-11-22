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
                Cookie = new HttpCookie(name);
                Cookie["Quantity"] = quantity;
                Cookie["Size"] = size;
            }
            else
            {
                Cookie = HttpContext.Current.Request.Cookies[name];
                Cookie["Quantity"] = quantity;
                Cookie["Size"] = size;
            }
            Cookie.Expires = DateTime.Now.AddDays(4);
            HttpContext.Current.Response.Cookies.Add(Cookie);
        }
    }
}