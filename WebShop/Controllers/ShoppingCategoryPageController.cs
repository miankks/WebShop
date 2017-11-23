using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using WebShop.Models.Pages;
using WebShop.Models.ViewModels;
using WebShop.Business;
using System.Web;
using System;

namespace WebShop.Controllers
{
    public class ShoppingCategoryPageController : PageControllerBase<ShoppingCategoryPage>
    {
        private readonly IContentRepository _contentRepository;

        public ShoppingCategoryPageController(IContentRepository contentRepository)
        {
            this._contentRepository = contentRepository;
        }
        public ActionResult Index(ShoppingCategoryPage currentPage)
        {
            var categoryPages = _contentRepository.GetChildren<ShoppingCategoryPage>(currentPage.ContentLink).ToList();
            var shoppingLinks = _contentRepository.GetChildren<ShoppingPage>(currentPage.ContentLink).ToList();

            var model = new ShoppingCategoryPageViewModel(currentPage)
            {
                ShoppingCategoryPages = categoryPages,
               ShoppingPages = shoppingLinks
            };
            return View(model);
        }

        //[HttpPost]
        //public ActionResult Index(ShoppingCategoryPage currentPage, ShoppingCartViewModel.CartItem items)
        //{
        //    var cookie = new CookieHelper();

        //    HttpCookie productCookies = new HttpCookie("ShoppingCart")
        //    {
        //        Expires = DateTime.Now.AddDays(5),
        //        ["Size"] = items.Size,
        //        ["Quantity"] = items.NumberOfItems,
        //        //["Quantity"] = items.Price,
        //        ["Price"] = Convert.ToString(items.ShoppingPage.ProductPriceFor),
        //        ["mainId"] = Convert.ToString(currentPage.ContentLink.ID)
        //    };
        //    cookie.GetCookies(productCookies.Name, productCookies["Quantity"], productCookies["Size"]);

        //    var vm = new ShoppingCartViewModel(currentPage);
        //    vm.ProductIdsInCookie = new List<string>
        //    {
        //        items.Size,
        //        items.NumberOfItems,
        //        items.ShoppingPage.ProductPriceFor.ToString(),
        //        //currentPage.ProductName,
        //        Convert.ToString(currentPage.ContentLink.ID)
        //    };

        //    TempData["successmessage"] = "Objektet har lagts i korgen!";
        //    return RedirectToAction("Index", vm);

        //    //return View(vm);
        //    //var categoryPages = _contentRepository.GetChildren<ShoppingCategoryPage>(currentPage.ContentLink).ToList();
        //    //var shoppingLinks = _contentRepository.GetChildren<ShoppingPage>(currentPage.ContentLink).ToList();
        //    //var model = new ShoppingCategoryPageViewModel(currentPage)
        //    //{
        //    //    ShoppingCategoryPages = categoryPages,
        //    //    ShoppingPages = shoppingLinks
        //    //};
        //    //return View(model);
        //}

    }
}