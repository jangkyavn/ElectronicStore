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
    public class CartItemController : Controller
    {
        IProductService _productService;

        public CartItemController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: CartItem
        public ActionResult Index()
        {
            if (Session[CommonConstants.SESSIONCART] == null)
            {
                Session[CommonConstants.SESSIONCART] = new List<CartItemViewModel>();
            }

            return View();
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            if (Session[CommonConstants.SESSIONCART] == null)
            {
                Session[CommonConstants.SESSIONCART] = new List<CartItemViewModel>();
            }

            var cart = Session[CommonConstants.SESSIONCART] as List<CartItemViewModel>;

            var totalCount = cart.Sum(x => x.Quantity);

            var totalMoney = cart.Sum(x => x.Product.Price * x.Quantity);

            return Json(new
            {
                data = cart,
                totalCount = totalCount,
                totalMoney = totalMoney
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddCartItem(int productId)
        {
            var cart = Session[CommonConstants.SESSIONCART] as List<CartItemViewModel>;

            if (cart.Any(x => x.ProductId == productId))
            {
                foreach (var item in cart)
                {
                    if (item.ProductId == productId)
                    {
                        item.Quantity += 1;
                    }
                }
            }
            else
            {
                var newCartItem = new CartItemViewModel();

                newCartItem.ProductId = productId;
                newCartItem.Product = Mapper.Map<Product, ProductViewModel>(_productService.GetById(productId));
                newCartItem.Quantity = 1;

                cart.Add(newCartItem);
            }

            Session[CommonConstants.SESSIONCART] = cart;

            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult UpdateCartItem(int productId, int quantity)
        {
            var cart = Session[CommonConstants.SESSIONCART] as List<CartItemViewModel>;

            foreach (var item in cart)
            {
                if (item.ProductId == productId)
                {
                    item.Quantity = quantity;
                }
            }

            var totalCount = cart.Sum(x => x.Quantity);

            var totalMoney = cart.Sum(x => x.Quantity * x.Product.Price);

            Session[CommonConstants.SESSIONCART] = cart;

            return Json(new
            {
                totalCount = totalCount,
                totalMoney = totalMoney,
                status = true
            });
        }

        public ActionResult Checkout()
        {
            var cart = Session[CommonConstants.SESSIONCART] as List<CartItemViewModel>;

            if (cart == null || cart.Count == 0)
            {
                return Redirect("/gio-hang");
            }

            return View(cart);
        }

        [HttpPost]
        public JsonResult DeleteCartItem(int productId)
        {
            var cart = Session[CommonConstants.SESSIONCART] as List<CartItemViewModel>;

            cart.RemoveAll(x => x.ProductId == productId);

            Session[CommonConstants.SESSIONCART] = cart;

            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteAll()
        {
            Session[CommonConstants.SESSIONCART] = new List<CartItemViewModel>();

            return Json(new
            {
                status = true
            });
        }
    }
}