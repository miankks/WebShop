using System;
using System.Collections.Generic;
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

namespace WebShop.Controllers
{
    public class ShoppingCartPageController : PageController<ShoppingCartPage>
    {
        private readonly IContentRepository _contentRepository;
        private int _id;
        public ShoppingCartPageController(IContentRepository contentRepository)
        {
            this._contentRepository = contentRepository;
        }

        public ActionResult Index(ShoppingCartPage currentPage, ShoppingPage shopPage)
        {
            var shoppingCartPages = _contentRepository.GetChildren<ShoppingCartPage>(currentPage.ContentLink).ToList();
            var vm = new ShoppingCartViewModel(currentPage)
            {
                ShoppingCartPages = shoppingCartPages,
                ProductIdsInCookie = new List<string>()
            };


            var cart = new ShoppingCartPage();
            var cartItems = new ShoppingCartViewModel.CartItem();


            HttpCookie cookies = Request.Cookies["ShoppingCart"];

            if (cookies != null)
            {
                _id = Convert.ToInt32(cookies["pageId"]);
                var reference = new ContentReference(_id);
                var shoppingPage = this._contentRepository.Get<ShoppingPage>(reference);
                cartItems.Price = shoppingPage.ProductPriceFor;
                cartItems.Size = cookies["Size"];
                cartItems.NumberOfItems = cookies["Quantity"];
                cartItems.Price = Convert.ToDouble(cookies["Price"]);
                cartItems.Price = cartItems.Price * Convert.ToDouble(cartItems.NumberOfItems);
                vm.ProductIdsInCookie.Add(cartItems.Size);
                vm.ProductIdsInCookie.Add(Convert.ToString(cartItems.NumberOfItems));
                vm.ProductIdsInCookie.Add(Convert.ToString(cartItems.Price));
                vm.ProductIdsInCookie.Add(cart.CartId);
                vm.ShoppingCartPages.Add(currentPage);

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
                    vm.CartTotal = vm.CartTotal + shoppingPage.ProductPriceFor;
                    cartData.Price = shoppingPage.ProductPriceFor;
                    cartData.CartItemTotal = cartData.CartItemTotal + shoppingPage.ProductPriceFor;
                    var cookie = new CookieHelper();
                    cookie.GetCookies(currentPage.CartId, sizes, numberOfItems, shoppingPage.ProductPriceFor, productPageId);

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
            return RedirectToAction("Index", shoppingPage);
        }

        [HttpPost]
        public ActionResult FinishShopping(ShoppingCartPage currentPage, string userName, string email, string adress, int productPageId)
        {
            var vm = new ShoppingCartViewModel(currentPage);
            vm.ProductIdsInCookie.Add(userName);
            vm.ProductIdsInCookie.Add(email);
            vm.ProductIdsInCookie.Add(adress);
            using (MailMessage mm = new MailMessage("sender@gmail.com", email))
            {
                mm.Subject = "Köpt klart";
                string body = "Hello " + userName + ",";
                body += "<br /><br />Din köp är handlat och ska vidare till leverans";
                body += "<br /><br />Thanks";
                mm.Body = body;
                mm.IsBodyHtml = true;
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    UseDefaultCredentials = true,
                    Port = 587
                };
                //smtp.Send(mm);
            }
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

            return View(vm);
        }
    }
}







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