using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebShop.Models.Pages;

namespace WebShop.Models.ViewModels
{
    public class ShoppingCategoryPageViewModel : PageViewModel<SitePageData>
    {
        public ShoppingCategoryPageViewModel(SitePageData currentPage) : base(currentPage)
        {
            this.ShoppingCategoryPages = new List<ShoppingCategoryPage>();
            this.ShoppingPages = new List<ShoppingPage>();
        }

        public List<ShoppingPage> ShoppingPages { get; set; }
        public List<ShoppingCategoryPage> ShoppingCategoryPages { get; set; }

        public string CartUrl { get; set; }
    }
}