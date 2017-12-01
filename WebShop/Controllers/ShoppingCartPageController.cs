using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
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
using System.Net.Mail;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;

namespace WebShop.Controllers
{
    public class ShoppingCartPageController : PageController<ShoppingCartPage>
    {
        private readonly IContentRepository _contentRepository;
        //private int _id;
        public ShoppingCartPageController(IContentRepository contentRepository)
        {
            this._contentRepository = contentRepository;
        }

        public ActionResult Index(ShoppingCartPage currentPage)
        {
            #region Commented code
            //var cart = new ShoppingCartPage();
            //var shoppingCartPages = _contentRepository.GetChildren<ShoppingCartPage>(currentPage.ContentLink).ToList();
            //   ShoppingCartPages = shoppingCartPages,
            //cartItems.CartItemTotal = shoppingPage.ProductPriceFor;
            // cartItems.CartItemTotal = cartItems.CartItemTotal * Convert.ToDouble(cartItems.NumberOfItems);
            //vm.ShoppingCartPages.Add(currentPage);
            //vm.ShoppingPagesInCart.Add(cartItems);
            //return JsonRequestBehavior.AllowGet;
                //_id = Convert.ToInt32(cookies["pageId"]);
                //var reference = new ContentReference(_id);
                //var shoppingPage = this._contentRepository.Get<ShoppingPage>(reference);
            #endregion

            var cartItems = new ShoppingCartViewModel.CartItem();

            var vm = new ShoppingCartViewModel(currentPage)
            {
                ProductIdsInCookie = new List<string>()
            };


            HttpCookie cookies = Request.Cookies["ShoppingCart"];

            if (cookies != null)
            {
                cartItems.Size = cookies["Size"];
                cartItems.NumberOfItems = cookies["Quantity"];
                cartItems.CartItemTotal = Convert.ToDouble(cookies["Price"]);
                vm.ProductIdsInCookie.Add(cartItems.Size);
                vm.ProductIdsInCookie.Add(cartItems.NumberOfItems);
                vm.ProductIdsInCookie.Add(Convert.ToString(cartItems.CartItemTotal));
                vm.ProductIdsInCookie.Add(Convert.ToString(cartItems.TotalMoms));
                vm.ProductIdsInCookie.Add(currentPage.CartId);
            }
            else
            {
                TempData["TomVarukorg"] = "Varukorg är tom";
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(ShoppingCartPage currentPage, string sizes, string numberOfItems, int productPageId)
        {
            var cartData = new ShoppingCartViewModel.CartItem();
            var vm = new ShoppingCartViewModel(currentPage);
                var reference = new ContentReference(productPageId);
                var shoppingPage = this._contentRepository.Get<ShoppingPage>(reference);
            if (productPageId > 0)
            {
                if (shoppingPage != null)
                {
                    // TODO: Add to cart
                    vm.CartTotal = Convert.ToDouble(numberOfItems) * shoppingPage.ProductPriceFor;
                    //cartData.CartItemTotal = shoppingPage.ProductPriceFor;
                    //cartData.CartItemTotal = vm.CartTotal;
                    var moms = cartData.TotalMoms;
                    var cookie = new CookieHelper();
                    cookie.GetCookies(sizes, numberOfItems, vm.CartTotal, productPageId, moms);
                }
            }

            var cart = new ShoppingCartViewModel.CartItem
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
        }

        [HttpPost]
        public ActionResult FinishShopping(ShoppingCartPage currentPage, string userName, string email, string adress, int productPageId)
        {
            HttpCookie cookies = Request.Cookies["ShoppingCart"];

            var vm = new ShoppingCartViewModel(currentPage);
            vm.ProductIdsInCookie.Add(userName);
            vm.ProductIdsInCookie.Add(email);
            vm.ProductIdsInCookie.Add(adress);
            if (cookies != null)
            {
                vm.ProductIdsInCookie.Add(cookies["Size"]);
                vm.ProductIdsInCookie.Add(cookies["Quantity"]);
                vm.ProductIdsInCookie.Add(cookies["Price"]);
            }
            using (MailMessage kop = new MailMessage("sender@gmail.com", email))
            {
                kop.Subject = "Köpt klart";
                string body = "Hello " + userName + ",";
                if (cookies != null)
                {
                    body += "<br /><br />Storlek: " + cookies["Size"];
                    body += "<br /><br />Antal objekt: " + cookies["Quantity"];
                    body += "<br /><br />Pris: " + cookies["Price"];
                }
                body += "<br /><br />Din köp är handlat och ska vidare till leverans";
                body += "<br /><br />Thanks";
                

                kop.Body = body;
                kop.IsBodyHtml = true;
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    UseDefaultCredentials = true,
                    Port = 587
                };
                //smtp.Send(mm);
            }
         

            return View(vm);
        }

        public ActionResult DeleteCookies()
        {
            var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            var resultPage = contentLoader.Get<StartPage>(ContentReference.StartPage);
            var pageUrl = urlResolver.GetVirtualPath(resultPage.ContentLink);
            //var reference = new ContentReference(resultPage);
            //var shoppingPage = this._contentRepository.Get<StartPage>(reference);
            if (Request.Cookies["ShoppingCart"] != null)
            {
                var c = new HttpCookie("ShoppingCart")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(c);
            }
            return RedirectToAction("Index", "StartPage", pageUrl);
            //return  RedirectToRoute(pageUrl, resultPage);
        }
    }
}


#region Sending email




//MailMessage msg = new MailMessage();

//msg.From = new MailAddress("sender@gmail.com");
//msg.To.Add(email);
//msg.Subject = "test";
//msg.Body = "<br /><br />Din köp är handlat och ska vidare till leverans";
//msg.Priority = MailPriority.High;

//SmtpClient client = new SmtpClient();

//client.Credentials = new NetworkCredential("mymailid", "mypassword", "smtp.gmail.com");
//client.Host = "smtp.gmail.com";
//client.Port = 587;
//client.DeliveryMethod = SmtpDeliveryMethod.Network;
//client.EnableSsl = true;
//client.UseDefaultCredentials = true;

//client.Send(msg);
#endregion
#region Array cookie



//[HttpPost]
//public void Index(HttpContextBase context/*, int productPage, string productId*/, int productPageId)
//{
// var cookie = this.GetProductOrderCookie(context, productPage);
// var cookieProducts = cookie.Value;

//if (!string.IsNullOrWhiteSpace(cookieProducts))
//{
//    var products = cookieProducts.Split(',').ToList();
//    if (products != null)
//    {

//        products.Add(productId);

//    }

//    cookieProducts = String.Join(",", products.ToArray());
//}
//else
//{
//    cookieProducts = productId;
//}

//cookie.Value = cookieProducts;
//cookie.Expires = DateTime.UtcNow.AddDays(3);

//context.Response.SetCookie(cookie);
//}
//HttpCookie productCookies = new HttpCookie("ShoppingCart")
//{
//    Expires = DateTime.Now.AddDays(5),
//    ["Size"] = cart.Size,
//    ["Quantity"] = cart.NumberOfItems,
//    ["mainId"] = Convert.ToString(currentPage.ContentLink.ID)
//};

//cookie.GetCookies(productCookies.Name, productCookies["Quantity"], productCookies["Size"]);
//cookie.GetCookies(currentPage.CartId, sizes, numberOfItems);
#endregion
#region working cookies

//var cookie = new HttpCookie("ShoppingCart")
//{
//    Value = Convert.ToString(productPageId),

//};

// var cookieProducts = cookie.Value;

// if (!string.IsNullOrWhiteSpace(cookieProducts))
// {
//     var reference = new ContentReference(productPageId);
//     var shoppingPage = this._contentRepository.Get<ShoppingPage>(reference);
//     var products = cookieProducts.Split(',').ToList();
//     if (shoppingPage != null)
//     {

//         products.Add(Convert.ToString(productPageId) );
//         products.Add(Convert.ToString(shoppingPage.ProductPriceFor));
//         products.Add(numberOfItems);
//         products.Add(sizes);

//     }

//     cookieProducts = String.Join(",", products.ToArray());
// }
// else
// {
//     cookieProducts = Convert.ToString(productPageId);
// }

// cookie.Value = cookieProducts;
// cookie.Expires = DateTime.UtcNow.AddDays(3);
// return Content(cookieProducts);

#endregion
#region LINQ

//var allReferences = _contentRepository.GetDescendents(ContentReference.StartPage);
//var allPages = allReferences.Select(x => this._contentRepository.Get<PageData>(x)).ToList();

//var selectedBySomething = allPages.Where(x => x.StartPublish > DateTime.Now);

//var otherSyntax = from page in allPages
//    where page.StartPublish > DateTime.Now
//    orderby page.StartPublish descending 
//    select page;

#endregion