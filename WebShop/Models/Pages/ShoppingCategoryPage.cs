using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using EPiServer.Web;

namespace WebShop.Models.Pages
{
    [SiteContentType(
        GroupName = Global.GroupNames.Products)]
    [SiteImageUrl(Global.StaticGraphicsFolderPath + "ShoppingCategoryPage.png")]
    [AvailableContentTypes(
        Availability.Specific,
        Include = new[] { typeof(ShoppingPage), typeof(ShoppingCategoryPage) })]
    [ContentType(DisplayName = "ShoppingCategoryPage", GUID = "629c3db2-a49f-41bd-b296-3e3bbd82397d", Description = "")]
    public class ShoppingCategoryPage : SitePageData
    {
        [Display(
            Name = "Main Product Image",
            Description = "Product Image",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        [UIHint(UIHint.Image)]
        public virtual ContentReference MainProductImage { get; set; }

      
    }
}