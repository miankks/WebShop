using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using WebShop.Models.Pages;

namespace WebShop.Models.ViewModels
{
    public class ShoppingCartViewModel : PageViewModel<SitePageData>
    {
        public ShoppingCartViewModel(SitePageData currentPage) : base(currentPage)
        {
        }

        public CookieCart CurrentCart { get; set; }
    }
}