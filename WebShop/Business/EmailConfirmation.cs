using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Http.ModelBinding;
using SendGrid;
using WebShop.Models.Pages;
using WebShop.Business;

namespace WebShop.Business
{
    public class EmailConfirmation
    {
        private double _totalPrice = 0;
        private double _totalMomsInCart = 0;
        public HttpCookie Cookie;
        public void EmailSend(CookieCart currentCart, string userName, string email, string adress)
        {
            
            if (currentCart != null)
            {
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("customercare@butik.com");
                    mailMessage.To.Add(email);
                    mailMessage.Subject = "Kop klart";
                    mailMessage.Body = "Hello " + userName + "," + Environment.NewLine + Environment.NewLine;

                    foreach (var item in currentCart.CartItems)
                    {
                        mailMessage.Body += Environment.NewLine + "Produkt namn:  " + item.ProductName +
                                            Environment.NewLine;
                        mailMessage.Body += "Antal bestallt:  " + item.NumberOfItems + Environment.NewLine;
                        mailMessage.Body += "Storlek:  " + item.Size + Environment.NewLine;
                        mailMessage.Body += "pris:  " + item.Price + Environment.NewLine;
                        mailMessage.Body += Environment.NewLine;
                    }
                    foreach (var item in currentCart.CartItems)
                    {
                        _totalPrice += item.Price;
                        _totalMomsInCart += item.TotalMoms;
                    }
                    mailMessage.Body += "Total pris:  " + _totalPrice + Environment.NewLine;
                    mailMessage.Body += "Total moms:  " + _totalMomsInCart + Environment.NewLine;
                    mailMessage.Body += "Din adress:  " + adress;
                    mailMessage.IsBodyHtml = false;
                    using (var smtp = new SmtpClient())
                    {
                        smtp.Send(mailMessage);
                        //var c = new HttpCookie("ShoppingCart")
                        //{
                        //    Expires = DateTime.Now.AddDays(-1)
                        //};
                        //Response.Cookies.Add(c);
                    }
            }
        }
    }
}