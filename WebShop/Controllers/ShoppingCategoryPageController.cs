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
using EPiServer.Filters;

namespace WebShop.Controllers
{
    public class ShoppingCategoryPageController : PageControllerBase<ShoppingCategoryPage>
    {
        private readonly IContentRepository _contentRepository;
        private readonly IPublishedStateAssessor _publishedStateAssessor;
        public ShoppingCategoryPageController(IContentRepository contentRepository, IPublishedStateAssessor publishedStateAssessor)
        {
            this._contentRepository = contentRepository;
            this._publishedStateAssessor = publishedStateAssessor;
        }
        public ActionResult Index(ShoppingCategoryPage currentPage)
        {
            //done by IPublishedStateAssessor
            //var categoryPages = _contentRepository.GetChildren<ShoppingCategoryPage>(currentPage.ContentLink).Where(_publishedStateAssessor.IsPublished).ToList(); 
            var categoryPages = FilterForVisitor.Filter(_contentRepository.GetChildren<ShoppingCategoryPage>(currentPage.ContentLink)).Cast<ShoppingCategoryPage>().ToList();
            var shopplinks = _contentRepository.GetChildren<ShoppingPage>(currentPage.ContentLink).ToList();
            var shoppingLinks = FilterForVisitor.Filter(shopplinks).Cast<ShoppingPage>().ToList();

            var model = new ShoppingCategoryPageViewModel(currentPage)
            {
               ShoppingCategoryPages = categoryPages,
               ShoppingPages = shoppingLinks
            };
            return View(model);
        }
    }
}

#region post action from SCPC

//[HttpPost]
//public ActionResult Index(ShoppingCategoryPage currentPage, string sizes, string numberOfItems,  ShoppingCartViewModel.CartItem item, ShoppingPage shopPage)
//{
//    var cookie = new CookieHelper();
//    cookie.GetCookies(currentPage.Name, sizes, numberOfItems);

//    TempData["successmessage"] = "Objektet har lagts i korgen!";
//    return RedirectToAction("Index");
//}




//public ActionResult ShoppingPage(ShoppingPage currentPage)
//{
//    var categoryPages = _contentRepository.GetChildren<ShoppingCategoryPage>(currentPage.ContentLink).ToList();
//Response.Write(item.ShoppingPage.Id); 
//Response.Write(item.ShoppingPage.ProductPriceFor);
//    var shoppingLinks = _contentRepository.GetChildren<ShoppingPage>(currentPage.ContentLink).ToList();

//    var model = new ShoppingCategoryPageViewModel(currentPage)
//    {
//        ShoppingCategoryPages = categoryPages,
//        ShoppingPages = shoppingLinks
//    };
//    return View(currentPage);
//}
#endregion
