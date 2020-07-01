using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Extensions;
using Web.Models;
using Web.Models.ViewModel;

namespace Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public ShoppingCartViewModel ShoppingCartVM { get; set; }

        public ShoppingCartController(ApplicationDbContext db)
        {
            _db = db;
            ShoppingCartVM = new ShoppingCartViewModel()
            {
                Products = new List<Models.Products>()
            };
        }

        //Get Index Shopping Cart
        public async Task<IActionResult> Index()
        {

           
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");


            if (lstShoppingCart!=null )
            {
                foreach (int cartItem in lstShoppingCart)
                {
                    Products prod = _db.Products.Include(p => p.Category).Include(p => p.ProductCategory).Include(p => p.Size).Include(p => p.Color).Where(p => p.ProductID == cartItem).FirstOrDefault();
                    ShoppingCartVM.Products.Add(prod);
                }
                return View(ShoppingCartVM);
            }
            else
            {
                return View(ShoppingCartVM);
            }
           // return View(ShoppingCartVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            List<int> lstCartItems = HttpContext.Session.Get<List<int>>("ssShoppingCart");

            ShoppingCartVM.Order.AppointmentDate = ShoppingCartVM.Order.AppointmentDate
                                                            .AddHours(ShoppingCartVM.Order.AppointmentTime.Hour)
                                                            .AddMinutes(ShoppingCartVM.Order.AppointmentTime.Minute);

            Order order = ShoppingCartVM.Order;
            _db.Order.Add(order);
            _db.SaveChanges();

            int orderID = order.OrderID;

            foreach (int productId in lstCartItems)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    AppointmentId =orderID ,
                    ProductId = productId
                };
                _db.OrderDetail.Add(orderDetail);

            }
            _db.SaveChanges();
            lstCartItems = new List<int>();
            HttpContext.Session.Set("ssShoppingCart", lstCartItems);

            return RedirectToAction("OrderConfirmation", "ShoppingCart", new { Id = orderID });

        }
        public IActionResult Remove(int id)
        {
            List<int> lstCartItems = HttpContext.Session.Get<List<int>>("ssShoppingCart");

            if (lstCartItems.Count > 0)
            {
                if (lstCartItems.Contains(id))
                {
                    lstCartItems.Remove(id);
                }
            }

            HttpContext.Session.Set("ssShoppingCart", lstCartItems);

            return RedirectToAction(nameof(Index));
        }
        public IActionResult OrderConfirmation(int id)
        {
            ShoppingCartVM.Order = _db.Order.Where(a => a.OrderID == id).FirstOrDefault();
            List<OrderDetail> objProdList = _db.OrderDetail.Where(p => p.AppointmentId == id).ToList();

            foreach (OrderDetail prodAptObj in objProdList)
            {
                ShoppingCartVM.Products.Add(_db.Products.Include(p => p.Category).Include(p => p.ProductCategory).Include(p => p.Color).Include(p => p.Size).Where(p => p.ProductID == prodAptObj.ProductId).FirstOrDefault());
            }

            return View(ShoppingCartVM);
        }
    }
}