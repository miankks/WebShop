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
            var categoryPages = _contentRepository.GetChildren<ShoppingCategoryPage>(currentPage.ContentLink).ToList();
            var shoppingLinks = _contentRepository.GetChildren<ShoppingPage>(currentPage.ContentLink).ToList();

            var vm = new ShoppingCartViewModel(currentPage)
            {
                ShoppingCategoryPages = categoryPages,
                ShoppingPages = shoppingLinks,
                ProductIdsInCookie = new List<string>()
            };


            var cart = new ShoppingCartPage();
            var cartItems = new ShoppingCartViewModel.CartItem();

            vm.ShoppingPagesInCart = new List<ShoppingCartViewModel.CartItem>();

            HttpCookie cookies = Request.Cookies["ShoppingCart"];
            if (cookies != null)
            {
                cartItems.Size = cookies["Size"];
                cartItems.NumberOfItems = cookies["Quantity"];
                //cartItems.Price = Convert.ToDouble( cookies["Price"]);
                vm.ProductIdsInCookie.Add(cartItems.Size);
                vm.ProductIdsInCookie.Add(cartItems.NumberOfItems);
                vm.ProductIdsInCookie.Add(cart.CartId);
            }
            return View(vm);
        }

        //[HttpPost]
        //public ActionResult Index( ShoppingCartPage currentPage, ShoppingPage curPage, ShoppingCartViewModel.CartItem cart)
        //{
        //    var cookie = new CookieHelper();

        //    var vm = new ShoppingCartViewModel(currentPage);

        //    //curPage.ProductPriceFor = cart.Price;

        //    HttpCookie productCookies = new HttpCookie("ShoppingCart")
        //    {
        //        Expires = DateTime.Now.AddDays(5),
        //        ["Size"] = cart.Size,
        //        ["Quantity"] =cart.NumberOfItems,
        //         //["Price"] =cart.Price.ToString() ,
        //        //["fe"] = currentPage.CartId,
        //        ["mainId"] = Convert.ToString(currentPage.ContentLink.ID)
        //    };

        //    cookie.GetCookies(productCookies.Name, productCookies["Quantity"], productCookies["Size"]);

        //    vm.ProductIdsInCookie = new List<string>
        //    {
        //        cart.Size,
        //        cart.NumberOfItems,
        //        //cart.Price.ToString(),
                
        //       Convert.ToString(currentPage.ContentLink.ID)
        //    };

        //    return View(vm);
        //}
    }
}