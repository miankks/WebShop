using System.Web;
using System.Web.Mvc;
using EPiServer.SpecializedProperties;
using WebShop.Models.Blocks;

namespace WebShop.Models.ViewModels
{
    public class LayoutModel
    {
        public SiteLogotypeBlock Logotype { get; set; }
        public IHtmlString LogotypeLinkUrl { get; set; }
        public bool HideHeader { get; set; }
        public bool HideFooter { get; set; }
        public LinkItemCollection ProductPages { get; set; }
        public LinkItemCollection ShoppingPages { get; set; }
        public LinkItemCollection CompanyInformationPages { get; set; }
        public LinkItemCollection NewsPages { get; set; }
        public LinkItemCollection CustomerZonePages { get; set; }
        public bool LoggedIn { get; set; }
        public MvcHtmlString LoginUrl { get; set; }
        public MvcHtmlString LogOutUrl { get; set; }
        public MvcHtmlString SearchActionUrl { get; set; }
        public MvcHtmlString CartUrl { get; set; }
        public MvcHtmlString ShoppingCategoryPageUrl { get; set; }
        public bool IsInReadonlyMode {get;set;}
    }
}
