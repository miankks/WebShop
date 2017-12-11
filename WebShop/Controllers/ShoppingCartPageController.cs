using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using WebShop.Models.Pages;
using WebShop.Business;
using System.Net.Mail;
using WebShop.Models.ViewModels;
using static Newtonsoft.Json.JsonConvert;

namespace WebShop.Controllers
{
    public class ShoppingCartPageController : PageController<ShoppingCartPage>
    {
        private double _totalPrice = 0;
        private double _totalMomsInCart = 0;
        private readonly IContentRepository _contentRepository;
        public ShoppingCartPageController(IContentRepository contentRepository)
        {
            this._contentRepository = contentRepository;
        }

        public ActionResult Index(ShoppingCartPage currentPage)
        {
            var cookieHelper = new CookieHelper();
            var cart = cookieHelper.GetCartFromCookie();
            var model = new ShoppingCartViewModel(currentPage);
            foreach (var item in cart.CartItems)
            {
                _totalPrice += item.Price;
                _totalMomsInCart += item.TotalMoms;
            }
            TempData["TotalPrice"] = _totalPrice;
            TempData["TotalMomsInCart"] = _totalMomsInCart;
            model.CurrentCart = cart;

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ShoppingCartPage currentPage, string sizes, string numberOfItems, int productPageId, string productPageName)
        {
            var cookieHelper = new CookieHelper();
            var reference = new ContentReference(productPageId);
            var shoppingPage = this._contentRepository.Get<ShoppingPage>(reference);

            var newCartItem = new CookieCart.CartCookieItem
            {
                Size = sizes,
                NumberOfItems = numberOfItems,
                Price = shoppingPage.ProductPriceFor * Convert.ToDouble(numberOfItems),
                ProductName = productPageName,
                ImageId = shoppingPage.ProductImage.ID,
                
            };
            var currentCart = cookieHelper.GetCartFromCookie();

            currentCart.CartItems.Add(newCartItem);

            cookieHelper.SaveCartToCookie(currentCart);

            return RedirectToAction("Index");
        }


        public ActionResult DeleteCookies()
        {
           

            if (Request.Cookies["ShoppingCart"] != null)
            {
                var c = new HttpCookie("ShoppingCart")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(c);
            }

            return RedirectToAction("ToShoppingPage");
            
        }

        public ActionResult Delete()
        {
            if (Request.Cookies["ShoppingCart"] != null)
            {
                var c = new HttpCookie("ShoppingCart")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(c);
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteACookie(int? product)
        {

            //var reference = new ContentReference(product?.ToString());
            //var shoppingPage = this._contentRepository.Get<CookieCart.CartCookieItem>(reference);
            
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult FinishShopping(ShoppingCartPage currentPage, string userName, string email, string adress, int? productPageId)
        {
            var sendEmail = new EmailConfirmation();
            
            var cookieHelper = new CookieHelper();
            var currentCart = cookieHelper.GetCartFromCookie();
            if (currentCart != null)
            {
                sendEmail.EmailSend(currentCart, userName, email, adress);
                foreach (var item in currentCart.CartItems)
                {
                    _totalPrice += item.Price;
                    _totalMomsInCart += item.TotalMoms;
                }
                TempData["TotalPrice"] = _totalPrice;
                TempData["TotalMomsInCart"] = _totalMomsInCart;
            }
            var model = new ShoppingCartViewModel(currentPage);
           
            model.CurrentCart = currentCart;
            return View(model);
        }

        public ActionResult ToShoppingPage()
        {
            return RedirectToAction("Index", new { node = ContentReference.StartPage });
        }
    }
}

//var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
//var message = new MailMessage();
//message.To.Add(new MailAddress(email));
//message.From = new MailAddress("bilal@bilal.com");
//message.Subject = "Köp klart";
//body += "Hello, " + userName + "," + Environment.NewLine;
//body +=Environment.NewLine;
//foreach (var item in currentCart.CartItems)
//{
//    body += "Produkt namn:  " + item.ProductName+ Environment.NewLine;
//    body += "Antal beställt:  " + item.NumberOfItems+ Environment.NewLine;
//    body += "Storlek:  " + item.Size + Environment.NewLine;
//    body += "pris:  " + item.Price+ Environment.NewLine;
//    body += Environment.NewLine;
//}
//foreach (var item in currentCart.CartItems)
//{
//    TotalPrice += item.Price;
//    TotalMomsInCart += item.TotalMoms;
//}
//body +="Total pris:  " + TotalPrice + Environment.NewLine;
//body += "Total moms:  " + TotalMomsInCart + Environment.NewLine;

//message.Body = string.Format(body, userName, email, message);

//message.IsBodyHtml = true;




//public ActionResult UpdateForm(ShoppingCartPage currentPage, string items)
//{
//    var cookieHelper = new CookieHelper();

//    var currentCart = cookieHelper.GetCartFromCookie();

//    currentCart.CartItems.Add(newCartItem);

//    cookieHelper.SaveCartToCookie(currentCart);
//    return RedirectToAction("Index");
//}
#region cart deserialize
//CookieCart.CartCookieItem deserialize = DeserializeObject<CookieCart.CartCookieItem>(cookies.Value);
// cart.CartItems.Add(deserialize);
#endregion
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
#region old cookie style
//var vm = new ShoppingCartViewModel(currentPage);
//    var reference = new ContentReference(productPageId);
//    var shoppingPage = this._contentRepository.Get<ShoppingPage>(reference);
//if (productPageId > 0)
//{
//    if (shoppingPage != null)
//    {
//        // TODO: Add to cart
//        vm.CartTotal = Convert.ToDouble(numberOfItems) * shoppingPage.ProductPriceFor;

//        var cookie = new CookieHelper();
//        cookie.GetCookies(sizes, numberOfItems, vm.CartTotal, productPageId, productPageName);
//    }
//}

//var cart = new ShoppingCartViewModel.CartItem
//{
//    NumberOfItems = numberOfItems,
//    Size = sizes,

//};


//vm.ProductIdsInCookie = new List<string>
//{
//    cart.NumberOfItems,
//    cart.Size,

//   Convert.ToString(currentPage.ContentLink.ID)
//};
#endregion
//cartItems.Size = cookies["Size"];
//cartItems.NumberOfItems = cookies["Quantity"];
//cartItems.CartItemTotal = Convert.ToDouble(cookies["Price"]);
//cartItems.TotalMoms = Convert.ToDouble(cookies["Moms"]);
//vm.ProductIdsInCookie.Add(cookies["pageName"]);
//vm.ProductIdsInCookie.Add(cartItems.Size);
//vm.ProductIdsInCookie.Add(cartItems.NumberOfItems);
//vm.ProductIdsInCookie.Add(Convert.ToString(cartItems.CartItemTotal));
//vm.ProductIdsInCookie.Add(Convert.ToString(cartItems.TotalMoms));
//vm.ProductIdsInCookie.Add(currentPage.CartId);
//vm.ProductIdsInCookie.Add(cookies.Value);


//var vm = new ShoppingCartViewModel(currentPage);
//vm.ProductIdsInCookie.Add(userName);
//vm.ProductIdsInCookie.Add(email);
//vm.ProductIdsInCookie.Add(adress);
//vm.ProductIdsInCookie.Add(deserialize.ProductName);
//vm.ProductIdsInCookie.Add(deserialize.Size);
//vm.ProductIdsInCookie.Add(deserialize.NumberOfItems);
//vm.ProductIdsInCookie.Add(Convert.ToString(deserialize.Price));
//vm.ProductIdsInCookie.Add(Convert.ToString(deserialize.Moms));
#region email configuration in controller
//if (currentCart != null)
//{
//    if (ModelState.IsValid)
//    {
//        MailMessage mailMessage = new MailMessage();
//        mailMessage.From = new MailAddress("customercare@butik.com");
//        mailMessage.To.Add(email);
//        mailMessage.Subject = "Kop klart";
//        mailMessage.Body = "Hello " + userName +"," + Environment.NewLine + Environment.NewLine;

//        foreach (var item in currentCart.CartItems)
//        {
//            mailMessage.Body += Environment.NewLine+ "Produkt namn:  " + item.ProductName + Environment.NewLine;
//            mailMessage.Body += "Antal bestallt:  " + item.NumberOfItems + Environment.NewLine;
//            mailMessage.Body += "Storlek:  " + item.Size + Environment.NewLine;
//            mailMessage.Body += "pris:  " + item.Price + Environment.NewLine;
//            mailMessage.Body += Environment.NewLine;
//        }
//        foreach (var item in currentCart.CartItems)
//        {
//            _totalPrice += item.Price;
//            _totalMomsInCart += item.TotalMoms;
//        }
//        mailMessage.Body += "Total pris:  " + _totalPrice + Environment.NewLine;
//        mailMessage.Body += "Total moms:  " + _totalMomsInCart + Environment.NewLine;
//        mailMessage.Body += "Din adress:  " + adress;
//        mailMessage.IsBodyHtml = false;
//        using (var smtp = new SmtpClient())
//        {
//            smtp.Send(mailMessage);
//            var c = new HttpCookie("ShoppingCart")
//            {
//                Expires = DateTime.Now.AddDays(-1)
//            };
//            Response.Cookies.Add(c);
//            return RedirectToAction("Index");
//        }
//    }
//}
#endregion

#region Add same product to same list
//var pageId = from page in currentCart.CartItems
//                  where page.pageId == newCartItem.pageId && page.Size == newCartItem.Size
//                  select page.pageId;
////var dog = currentCart.Where(d => d.Id == pageId).FirstOrDefault();
//if (pageId != null)
//{

//}
//var allReferences = _contentRepository.GetDescendents(ContentReference.StartPage);
//var allPages = allReferences.Select(x => this._contentRepository.Get<CookieCart.CartCookieItem>(x)).ToList();

//var selectedBySomething = allPages.Where(x => x.ImageId == newCartItem.ImageId);
//if (selectedBySomething!=null)
//{
//cookieHelper.UpdateSelectedCookie(currentCart,Convert.ToInt32());
//}
//foreach (var item in currentCart.CartItems)
//{
//    if (newCartItem.ImageId == item.ImageId && newCartItem.Size == item.Size)
//    {
//        //newCartItem.NumberOfItems += item.NumberOfItems;
//        item.NumberOfItems = item.NumberOfItems + newCartItem.NumberOfItems;
//        cookieHelper.UpdateSelectedCookie(currentCart);

//    }
//    else
//    {
//        Console.Write("do nothing");
//    }
//}
#endregion
