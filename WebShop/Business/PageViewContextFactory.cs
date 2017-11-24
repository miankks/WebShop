using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WebShop.Models.Pages;
using WebShop.Models.ViewModels;
using EPiServer;
using EPiServer.Core;
using EPiServer.Data;
using EPiServer.Web;
using EPiServer.Web.Routing;

namespace WebShop.Business
{
    public class PageViewContextFactory
    {
        private readonly IContentLoader _contentLoader;
        private readonly UrlResolver _urlResolver;
        private readonly IDatabaseMode _databaseMode;

        public PageViewContextFactory(IContentLoader contentLoader, UrlResolver urlResolver, IDatabaseMode databaseMode)
        {
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
            _databaseMode = databaseMode;
        }

        public virtual LayoutModel CreateLayoutModel(ContentReference currentContentLink, RequestContext requestContext)
        {
            var startPageContentLink = SiteDefinition.Current.StartPage;

            // Use the content link with version information when editing the startpage,
            // otherwise the published version will be used when rendering the props below.
            if (currentContentLink.CompareToIgnoreWorkID(startPageContentLink))
            {
                startPageContentLink = currentContentLink;
            }

            var startPage = _contentLoader.Get<StartPage>(startPageContentLink);

            var cartPages = _contentLoader.GetChildren<ShoppingCartPage>(ContentReference.StartPage).ToList();
            var cartUrl = string.Empty;

            if (cartPages.Any())
            {
                cartUrl = _urlResolver.GetUrl(cartPages.First().ContentLink);
            }

            var shoppingCategoryPages = _contentLoader.GetChildren<ShoppingCategoryPage>(ContentReference.StartPage).ToList();
            var shoppingCategoryPageUrl = string.Empty;

            if (shoppingCategoryPages.Any())
            {
                shoppingCategoryPageUrl = _urlResolver.GetUrl(shoppingCategoryPages.First().ContentLink);
            }

            var shoppingPages = _contentLoader.GetChildren<ShoppingPage>(ContentReference.StartPage).ToList();
            var shoppingPageUrl = string.Empty;

            if (shoppingPages.Any())
            {
                shoppingPageUrl = _urlResolver.GetUrl(shoppingCategoryPages.First().ContentLink);
            }


            return new LayoutModel
                {
                    Logotype = startPage.SiteLogotype,
                    LogotypeLinkUrl = new MvcHtmlString(_urlResolver.GetUrl(SiteDefinition.Current.StartPage)),
                    ProductPages = startPage.ProductPageLinks,
                    CompanyInformationPages = startPage.CompanyInformationPageLinks,
                    NewsPages = startPage.NewsPageLinks,
                    CustomerZonePages = startPage.CustomerZonePageLinks,
                    LoggedIn = requestContext.HttpContext.User.Identity.IsAuthenticated,
                    LoginUrl = new MvcHtmlString(GetLoginUrl(currentContentLink)),
                    CartUrl = new MvcHtmlString(cartUrl),
                    ShoppingCategoryPageUrl = new MvcHtmlString(shoppingCategoryPageUrl),
                    ShoppingPageUrl = new MvcHtmlString(shoppingPageUrl),

                SearchActionUrl = new MvcHtmlString(EPiServer.Web.Routing.UrlResolver.Current.GetUrl(startPage.SearchPageLink)),
                    IsInReadonlyMode = _databaseMode.DatabaseMode == DatabaseMode.ReadOnly
                };
        }

        private string GetLoginUrl(ContentReference returnToContentLink)
        {
            return string.Format(
                "{0}?ReturnUrl={1}",
                (FormsAuthentication.IsEnabled ? FormsAuthentication.LoginUrl : VirtualPathUtility.ToAbsolute(Global.AppRelativeLoginPath)),
                _urlResolver.GetUrl(returnToContentLink));
        }

        public virtual IContent GetSection(ContentReference contentLink)
        {
            var currentContent = _contentLoader.Get<IContent>(contentLink);
            if (currentContent.ParentLink != null && currentContent.ParentLink.CompareToIgnoreWorkID(SiteDefinition.Current.StartPage))
            {
                return currentContent;
            }

            return _contentLoader.GetAncestors(contentLink)
                .OfType<PageData>()
                .SkipWhile(x => x.ParentLink == null || !x.ParentLink.CompareToIgnoreWorkID(SiteDefinition.Current.StartPage))
                .FirstOrDefault();
        }
    }
}
