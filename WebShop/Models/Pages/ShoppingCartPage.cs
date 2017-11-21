using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using WebShop.Models.ViewModels;

namespace WebShop.Models.Pages
{
    [ContentType(DisplayName = "ShoppingCartPage", GUID = "4dacacf3-63a4-43d5-b8ab-ae3ae20276e2", Description = "")]
    public class ShoppingCartPage : SitePageData
    {
      
        [Display(
            Name = "Cart Id",
            Description = "Cart Id",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual string CartId
        {
            get
            {
                string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                char[] stringChars = new char[7];
                Random random = new Random();

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }
                string finalString = new String(stringChars);
                return finalString;
            }
            
        }

        [Display(
            Name = "Cart total price",
            GroupName = "Price for total objects in cart",
            Order = 20)]
        public virtual double TotalCartPrice { get; set; }

        [Display(
            Name = "Cart total price",
            GroupName = "Price for total objects in cart",
            Order = 30)]
        public virtual string Size { get; set; }

        [Display(
            Name = "Cart Moms",
            GroupName = "Moms for all objects in cart",
            Order = 40)]
        public virtual double CartMoms
        {
            get => TotalCartPrice * 0.25;
        }

        [Display(
            Name = "Cart total price",
            GroupName = "Price for total objects in cart",
            Order = 30)]
        public virtual string NumberOfItems { get; set; }

        [Display(
            Name = "Cart content area",
            GroupName = SystemTabNames.Content,
            Order = 50)]
        [CultureSpecific]
        public virtual ContentArea CartContentArea { get; set; }
    }
}