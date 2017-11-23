using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using WebShop.Business;
using WebShop.Models.Pages;
using WebShop.Models.ViewModels;

namespace WebShop.Controllers
{
    public class ShoppingPageController : PageControllerBase<ShoppingPage>
    {
        private readonly IContentRepository _contentRepository;

        public ShoppingPageController(IContentRepository contentRepository)
        {
            this._contentRepository = contentRepository;
        }
        public ActionResult Shop(ShoppingPage currentPage)
        {
            var categoryPages = _contentRepository.GetChildren<ShoppingCategoryPage>(currentPage.ContentLink).ToList();
            var shoppingLinks = _contentRepository.GetChildren<ShoppingPage>(currentPage.ContentLink).ToList();

            var vm = new ShoppingCartViewModel(currentPage)
            {
                ShoppingCategoryPages = categoryPages,
                ShoppingPages = shoppingLinks,
                ProductIdsInCookie = new List<string>()
            };


            return View(currentPage);
        }

        [HttpPost]
        public ActionResult Index(ShoppingPage currentPage, ShoppingCartViewModel.CartItem cart, string quantity, string size)
        {
            var cookie = new CookieHelper();

            var vm = new ShoppingCartViewModel(currentPage);
            cart.NumberOfItems = size;
            cart.Price = currentPage.ProductPriceFor;
            HttpCookie productCookies = new HttpCookie("ShoppingCart")
            {
                Expires = DateTime.Now.AddDays(5),
                ["Size"] = size,
                ["Quantity"] = quantity,
                ["Price"] = currentPage.ProductPriceFor.ToString(),
                ["mainId"] = Convert.ToString(currentPage.ContentLink.ID)
            };

            cookie.GetCookies(productCookies.Name, productCookies["Quantity"], productCookies["Size"]);

            vm.ProductIdsInCookie = new List<string>
            {
                cart.Size,
                cart.NumberOfItems,
               Convert.ToString(currentPage.ContentLink.ID)
            };

            return View(vm);
        }
    }
}