using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using WebShop.Models.Pages;
using WebShop.Models.ViewModels;

namespace WebShop.Controllers
{
    public class ShoppingCartPageController : PageController<ShoppingCartPage>
    {
        public ActionResult Index(ShoppingCartPage currentPage)
        {
            var vm = new ShoppingCartViewModel(currentPage);

            var cart = new ShoppingCartPage();

            HttpCookie cookies = Request.Cookies["ShoppingCart"];
            if (cookies != null)
            {
                cart.Size = cookies["Size"];
                cart.NumberOfItems = cookies["quantity"];
                vm.ProductIdsInCookie = new List<string>();
                vm.ProductIdsInCookie.Add(cart.Size);
                vm.ProductIdsInCookie.Add(cart.NumberOfItems);
                vm.ProductIdsInCookie.Add(cart.CartId);
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(ShoppingCartPage currentPage, string numberOfItems, string sizes)
        {
            var vm = new ShoppingCartViewModel(currentPage);
            HttpCookie productCookies = new HttpCookie("ShoppingCart")
            {
                Expires = DateTime.Now.AddDays(5),
                ["Size"] = sizes,
                ["quantity"] = numberOfItems,
                ["fe"] = currentPage.CartId,
                ["mainId"] =Convert.ToString(currentPage.ContentLink.ID) 
            };

            this.ControllerContext.HttpContext.Response.Cookies.Add(productCookies);
            vm.ProductIdsInCookie = new List<string>
            {
                sizes,
                numberOfItems,
                currentPage.CartId,
               Convert.ToString(currentPage.ContentLink.ID) 
            };

            return View(vm);
        }
    }
}