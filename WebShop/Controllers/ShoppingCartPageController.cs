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

            var vm = new ShoppingCartViewModel(currentPage)
            {
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
                cartItems.Price =Convert.ToDouble(cookies["Price"]) ;
                vm.ProductIdsInCookie.Add(cartItems.Size);
                vm.ProductIdsInCookie.Add(cartItems.NumberOfItems);
                vm.ProductIdsInCookie.Add(Convert.ToString(cartItems.Price) );
                vm.ProductIdsInCookie.Add(cart.CartId);
            }
                         
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(ShoppingCartPage currentPage, string sizes, string numberOfItems, int productPageId)
        {
            //var allReferences = _contentRepository.GetDescendents(ContentReference.StartPage);
            //var allPages = allReferences.Select(x => this._contentRepository.Get<PageData>(x)).ToList();

            //var selectedBySomething = allPages.Where(x => x.StartPublish > DateTime.Now);

            //var otherSyntax = from page in allPages
            //    where page.StartPublish > DateTime.Now
            //    orderby page.StartPublish descending 
            //    select page;

            var vm = new ShoppingCartViewModel(currentPage);



            if (productPageId > 0)
            {
                var reference = new ContentReference(productPageId);
                var shoppingPage = this._contentRepository.Get<ShoppingPage>(reference);

                if (shoppingPage != null)
                {
                    // TODO: Add to cart
                    vm.CartTotal = vm.CartTotal+shoppingPage.ProductPriceFor;

            var cookie = new CookieHelper();
            cookie.GetCookies(currentPage.CartId, sizes, numberOfItems, shoppingPage.ProductPriceFor, productPageId);
                   // cookie.GetCookies(productPageId);

                }
            }


            

            ShoppingCartViewModel.CartItem cart = new ShoppingCartViewModel.CartItem
            {
                NumberOfItems = numberOfItems,
                Size = sizes,
                
            };


            vm.ProductIdsInCookie = new List<string>
            {
                cart.NumberOfItems,
                cart.Size,
                
               Convert.ToString(currentPage.ContentLink.ID)
            };
            return RedirectToAction("Index");
            //return View(vm);
        }
    }
}
            //HttpCookie productCookies = new HttpCookie("ShoppingCart")
            //{
            //    Expires = DateTime.Now.AddDays(5),
            //    ["Size"] = cart.Size,
            //    ["Quantity"] = cart.NumberOfItems,
            //    ["mainId"] = Convert.ToString(currentPage.ContentLink.ID)
            //};

            //cookie.GetCookies(productCookies.Name, productCookies["Quantity"], productCookies["Size"]);
            //cookie.GetCookies(currentPage.CartId, sizes, numberOfItems);