using AutoMapper;
using Data.Infrastructure;
using Model.Models;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web.Infrastructure.Core;
using Web.Models;

namespace Web.Controllers
{
    public class ProductController : Controller
    {
        IProductCategoryService _productCategoryService;
        IProductService _productService;
        IUnitOfWork _unitOfWork;

        public ProductController(IProductCategoryService productCategoryService, IProductService productService, IUnitOfWork unitOfWork)
        {
            _productCategoryService = productCategoryService;
            _productService = productService;
            _unitOfWork = unitOfWork;
        }

        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(int? productId)
        {
            if (productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = _productService.GetById(productId.Value);

            if (model == null)
            {
                return HttpNotFound();
            }

            ViewBag.moreImages = new JavaScriptSerializer().Deserialize<List<string>>(model.MoreImages).Take(2);

            ViewBag.relatedProducts = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(_productService.GetRelatedProducts(productId.Value, 4));

            ViewBag.categoryName = _productCategoryService.GetById(model.CategoryID).Name;

            ViewBag.Tags = Mapper.Map<IEnumerable<Tag>, IEnumerable<TagViewModel>>(_productService.GetListTagByProductId(productId.Value));

            var productViewModel = Mapper.Map<Product, ProductViewModel>(model);

            return View(productViewModel);
        }

        public ActionResult Category(int? categoryId, int page = 1, string sort = "")
        {
            if (categoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int pageSize = 6;
            int totalRow = 0;

            var productModel = _productService.GetListProductByCategoryIdPaging(categoryId.Value, page, pageSize, sort, out totalRow);

            if (productModel == null)
            {
                return HttpNotFound();
            }

            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);

            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = 5,
                Page = page,
                TotalCount = totalRow,
                TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
            };

            ViewBag.sort = sort;
            ViewBag.categoryName = _productCategoryService.GetById(categoryId.Value).Name;

            return View(paginationSet);
        }

        public ActionResult GetListByTag(string tagId, int page = 1)
        {
            int pageSize = 10;
            int totalRow = 0;

            if (tagId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var lstProductByTagId = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(_productService.GetListProductByTagId(tagId, page, pageSize, out totalRow));

            if (lstProductByTagId.Count() == 0)
            {
                return HttpNotFound();
            }

            ViewBag.Tag = Mapper.Map<Tag, TagViewModel>(_productService.GetTag(tagId));

            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = lstProductByTagId,
                MaxPage = 5,
                Page = page,
                TotalCount = totalRow,
                TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
            };

            return View(paginationSet);
        }

        public ActionResult Search(string keyword, int page = 1, string sort = "")
        {
            int pageSize = 6;
            int totalRow = 0;

            var productModel = _productService.GetListProductByKeywordPaging(keyword, page, pageSize, sort, out totalRow);

            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);

            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = 5,
                Page = page,
                TotalCount = totalRow,
                TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
            };

            ViewBag.sort = sort;
            ViewBag.keyword = keyword;

            return View(paginationSet);
        }
    }
}