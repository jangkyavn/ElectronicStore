using AutoMapper;
using Common;
using Model.Models;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        IProductCategoryService _productCategoryService;
        ICommonService _commonService;
        IProductService _productService;

        public HomeController(IProductCategoryService productCategoryService, ICommonService commonService, IProductService productService)
        {
            _productCategoryService = productCategoryService;
            _commonService = commonService;
            _productService = productService;
        }

        public ActionResult Index()
        {
            var homeViewModel = new HomeViewModel();

            var lstNewProduct = _productService.GetNewProducts(4);
            var lstMobileProduct = _productService.GetListProductByType(CommonConstants.MobileProductType, 3);
            var lstTabletProduct = _productService.GetListProductByType(CommonConstants.TabletProductType, 3);
            var lstLaptopProduct = _productService.GetListProductByType(CommonConstants.LaptopProductType, 3);

            homeViewModel.NewProducts = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lstNewProduct);
            homeViewModel.MobileProducts = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lstMobileProduct);
            homeViewModel.TabletProducts = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lstTabletProduct);
            homeViewModel.LaptopProducts = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lstLaptopProduct);

            return View(homeViewModel);
        }

        [ChildActionOnly]
        public ActionResult _Navigation()
        {
            var model = _productCategoryService.GetAll();

            var lstProductCategory = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);

            return PartialView(lstProductCategory);
        }

        [ChildActionOnly]
        public ActionResult _Header()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult _Footer()
        {
            var model = _commonService.GetFooter();

            var footerViewModel = Mapper.Map<Footer, FooterViewModel>(model);

            return PartialView(footerViewModel);
        }

        [HttpGet]
        public ActionResult ProductDetail(int id)
        {
            var model = _productService.GetById(id);

            var productViewModel = Mapper.Map<Product, ProductViewModel>(model);

            return Json(new
            {
                data = productViewModel
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetListProductByKeyword(string keyword)
        {
            var model = _productService.GetListProductByKeyword(keyword, 10);

            var viewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);

            return Json(new
            {
                data = viewModel
            }, JsonRequestBehavior.AllowGet);
        }
    }
}