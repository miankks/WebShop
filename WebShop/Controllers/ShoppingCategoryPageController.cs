﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using WebShop.Models.Pages;
using WebShop.Models.ViewModels;
using WebShop.Business;
using System.Web;
using System;
using System.Globalization;
using EPiServer.Filters;

namespace WebShop.Controllers
{
    public class ShoppingCategoryPageController : PageControllerBase<ShoppingCategoryPage>
    {
        private readonly IContentRepository _contentRepository;
        private readonly IPublishedStateAssessor _publishedStateAssessor;
        public ShoppingCategoryPageController(IContentRepository contentRepository, IPublishedStateAssessor publishedStateAssessor)
        {
            this._contentRepository = contentRepository;
            this._publishedStateAssessor = publishedStateAssessor;
        }
        public ActionResult Index(ShoppingCategoryPage currentPage)
        {
            //done by IPublishedStateAssessor
            //var categoryPages = _contentRepository.GetChildren<ShoppingCategoryPage>(currentPage.ContentLink).Where(_publishedStateAssessor.IsPublished).ToList(); 
            var categoryPages = FilterForVisitor.Filter(_contentRepository.GetChildren<ShoppingCategoryPage>(currentPage.ContentLink)).Cast<ShoppingCategoryPage>().ToList();
            var shopplinks = _contentRepository.GetChildren<ShoppingPage>(currentPage.ContentLink).ToList();
            var shoppingLinks = FilterForVisitor.Filter(shopplinks).Cast<ShoppingPage>().ToList();
            var criterias = new PropertyCriteriaCollection();
            var criteria = new PropertyCriteria { Condition = CompareCondition.GreaterThan, Name = "PageCreated", Type = PropertyDataType.Date,
                Value = DateTime.Now.AddDays(-7).ToString(CultureInfo.InvariantCulture), Required = true };
            criterias.Add(criteria);
            var criteriaQueryService = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<IPageCriteriaQueryService>();
            var weekOldPages = criteriaQueryService.FindPagesWithCriteria(ContentReference.StartPage, criterias);

            var model = new ShoppingCategoryPageViewModel(currentPage)
            {
               ShoppingCategoryPages = categoryPages,
               ShoppingPages = shoppingLinks
            };
            return View(model);
        }
    }
}
