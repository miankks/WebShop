using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using WebShop.Models.ViewModels;

namespace WebShop.Models.Pages
{
    [SiteContentType(
        GroupName = Global.GroupNames.Products)]
    [SiteImageUrl(Global.StaticGraphicsFolderPath + "shoppingPage.png")]
    [ContentType(DisplayName = "ShoppingPage", GUID = "5086689c-4f8f-4052-8b33-56924be644ad", Description = "")]
    public class ShoppingPage : SitePageData
    {
        [Display(
            Name = "Product Id",
            Description = "Product Id",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual string Id
        {
            get
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var stringChars = new char[7];
                var random = new Random();

                for (var i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }
                var finalString = new String(stringChars);
                return finalString;
            }
        }

        [Display(
            Name = "Product Image",
            Description = "Product Image",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        [UIHint(UIHint.Image)]
        public virtual ContentReference ProductImage { get; set; }

        [Display(
            Name = "Product name",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual string ProductName { get; set; }

        [Display(
            Name = "Product price",
            GroupName = "Price",
            Order = 20)]
        public virtual double ProductPriceFor { get; set; }

        [Display(
            Name = "Product Moms",
            GroupName = "Price",
            Order = 20)]
        public virtual double Moms => ProductPriceFor * 0.25;

        [Display(
            Name = "Product description",
            GroupName = SystemTabNames.Content,
            Order = 40)]
        public virtual XhtmlString ProductDecscription { get; set; }

        [Display(
            Name = "Product content area",
            GroupName = SystemTabNames.Content,
            Order = 50)]
        [CultureSpecific]
        public virtual ContentArea ProductContentArea { get; set; }

        [Display(
            Name = "Product availability",
            GroupName = SystemTabNames.Content,
            Order = 60)]
        public virtual bool Productavailability { get; set; }

        //[Display(GroupName = "Sizes", Order = 70)]
        //[UIHint("CustomLanguage")]
        //public virtual string ProductSize { get; set; }
        //public class LanguageSelectorSelectionFactory : ISelectionFactory
        //{
        //    public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        //    {
        //        var typeOfLanguage = new List<SelectItem>
        //        {
        //            new SelectItem() {Value = "sm", Text = "Small"},
        //            new SelectItem() {Value = "md", Text = "Medium"},
        //            new SelectItem() {Value = "lg", Text = "Large"}
        //        };

        //        return typeOfLanguage;
        //    }
        //}

        //[EditorDescriptorRegistration(TargetType = typeof(string), UIHint = "CustomLanguage")]
        //public class LanguageMultipleEditorDescriptor : EditorDescriptor
        //{
        //    public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        //    {
        //        SelectionFactoryType = typeof(LanguageSelectorSelectionFactory);
        //        ClientEditingClass = "epi.cms.contentediting.editors.SelectionEditor";
        //        base.ModifyMetadata(metadata, attributes);
        //    }
        //}
    }
}