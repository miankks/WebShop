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
using WebShop.Business;

namespace WebShop.Controllers
{
    public class ShoppingCartPageController : PageController<ShoppingCartPage>
    {
        private readonly IContentRepository _contentRepository;

        public ShoppingCartPageController(IContentRepository contentRepository)
        {
            this._contentRepository = contentRepository;
        }
       
        public ActionResult Index(ShoppingCartPage currentPage)
        {
            var shoppingCartView = _contentRepository.GetChildren<ShoppingCartPage>(currentPage.ContentLink).ToList();
            var vm = new ShoppingCartViewModel(currentPage);
            vm.ProductIdsInCookie = new List<string>();
            vm.ShoppingCartPages = shoppingCartView;
            var cart = new ShoppingCartPage();

            var cartItems = new ShoppingCartViewModel.CartItem();
            
            HttpCookie cookies = Request.Cookies["ShoppingCart"];
            if (cookies != null)
            {
                cartItems.Size = cookies["Size"];
                cartItems.NumberOfItems = cookies["Quantity"];
                vm.ProductIdsInCookie.Add(cartItems.Size);
                vm.ProductIdsInCookie.Add(cartItems.NumberOfItems);
                vm.ProductIdsInCookie.Add(cart.CartId);
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(ShoppingCartPage currentPage, string numberOfItems, string sizes)
        {
            var shoppingCartView = _contentRepository.GetChildren<ShoppingCartPage>(currentPage.ContentLink).ToList();

            var cookie = new CookieHelper();

            var vm = new ShoppingCartViewModel(currentPage);
            vm.ShoppingCartPages = shoppingCartView;
            HttpCookie productCookies = new HttpCookie("ShoppingCart")
            {
                Expires = DateTime.Now.AddDays(5),
                ["Size"] = sizes,
                ["Quantity"] = numberOfItems,
                ["fe"] = currentPage.CartId,
                ["mainId"] =Convert.ToString(currentPage.ContentLink.ID)
    };
   
            cookie.GetCookies(productCookies.Name, productCookies["Quantity"], productCookies["Size"]);

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