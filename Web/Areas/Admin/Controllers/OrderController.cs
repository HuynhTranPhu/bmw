using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models.ViewModel;
using Web.Utility;
using Microsoft.EntityFrameworkCore;
using Web.Models;
using System.Text;

namespace Web.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.AdminEndUser + "," + SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        private int PageSize = 3;

        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index(int productPage = 1, string searchName = null, string searchDate = null)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            AppointmentViewModel appointmentVM = new AppointmentViewModel()
            {
                Order = new List<Models.Order>()
            };

            StringBuilder param = new StringBuilder();
            param.Append("/Admin/Order?productPage=:");
            param.Append("&searchName=");
            if (searchName != null)
            {
                param.Append(searchName);
            }
            param.Append("&searchDate=");
            if (searchDate != null)
            {
                param.Append(searchDate);
            }

            appointmentVM.Order = _db.Order.Include(a=>a.SalesPerson).ToList();
            if (User.IsInRole(SD.AdminEndUser))
            {
                appointmentVM.Order = appointmentVM.Order.Where(a => a.SalesPersonId == claim.Value).ToList();
            }

            if (searchName != null)
            {
                appointmentVM.Order = appointmentVM.Order.Where(a => a.CustomerName.ToLower().Contains(searchName.ToLower())).ToList();
            }
            
            if (searchDate != null)
            {
                try
                {
                    DateTime appDate = Convert.ToDateTime(searchDate);
                    appointmentVM.Order = appointmentVM.Order.Where(a => a.AppointmentDate.ToShortDateString().Equals(appDate.ToShortDateString())).ToList();
                }
                catch (Exception ex)
                {

                }

            }
            var count = appointmentVM.Order.Count;
            appointmentVM.Order = appointmentVM.Order.OrderBy(p => p.AppointmentDate)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize).ToList();


            appointmentVM.Page = new Page
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = count,
                urlParam = param.ToString()
            };
            return View(appointmentVM);
        }
        //GET Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productList = (IEnumerable<Products>)(from p in _db.Products
                                                      join a in _db.OrderDetail
                                                      on p.ProductID equals a.ProductId
                                                      where a.AppointmentId == id
                                                      select p).Include("Category").Include("ProductCategory").Include("Size").Include("Color");

            AppointmentDetailsViewModel objAppointmentVM = new AppointmentDetailsViewModel()
            {
                Order = _db.Order.Include(a => a.SalesPerson).Where(a => a.OrderID == id).FirstOrDefault(),
                SalesPerson = _db.User.ToList(),
                Products = productList.ToList()
            };

            return View(objAppointmentVM);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AppointmentDetailsViewModel objAppointmentVM)
        {
            if (ModelState.IsValid)
            {
                objAppointmentVM.Order.AppointmentDate = objAppointmentVM.Order.AppointmentDate
                                    .AddHours(objAppointmentVM.Order.AppointmentTime.Hour)
                                    .AddMinutes(objAppointmentVM.Order.AppointmentTime.Minute);

                var appointmentFromDb = _db.Order.Where(a => a.OrderID == objAppointmentVM.Order.OrderID).FirstOrDefault();

                appointmentFromDb.CustomerName = objAppointmentVM.Order.CustomerName;
                appointmentFromDb.CustomerEmail = objAppointmentVM.Order.CustomerEmail;
                appointmentFromDb.CustomerPhoneNumber = objAppointmentVM.Order.CustomerPhoneNumber;
                appointmentFromDb.Address = objAppointmentVM.Order.Address;
                appointmentFromDb.AppointmentDate = objAppointmentVM.Order.AppointmentDate;
                appointmentFromDb.isConfirmed = objAppointmentVM.Order.isConfirmed;
                if (User.IsInRole(SD.SuperAdminEndUser))
                {
                    appointmentFromDb.SalesPersonId = objAppointmentVM.Order.SalesPersonId;
                }
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));


            }

            return View(objAppointmentVM);
        }


        //GET Detials
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productList = (IEnumerable<Products>)(from p in _db.Products
                                                      join a in _db.OrderDetail
                                                      on p.ProductID equals a.ProductId
                                                      where a.AppointmentId == id
                                                      select p).Include("Category").Include("ProductCategory").Include("Size").Include("Color");

            AppointmentDetailsViewModel objAppointmentVM = new AppointmentDetailsViewModel()
            {
                Order = _db.Order.Include(a => a.SalesPerson).Where(a => a.OrderID == id).FirstOrDefault(),
                SalesPerson = _db.User.ToList(),
                Products = productList.ToList()
            };

            return View(objAppointmentVM);

        }


        //GET Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productList = (IEnumerable<Products>)(from p in _db.Products
                                                      join a in _db.OrderDetail
                                                      on p.ProductID equals a.ProductId
                                                      where a.AppointmentId == id
                                                      select p).Include("Category").Include("ProductCategory").Include("Size").Include("Color");

            AppointmentDetailsViewModel objAppointmentVM = new AppointmentDetailsViewModel()
            {
                Order = _db.Order.Include(a => a.SalesPerson).Where(a => a.OrderID == id).FirstOrDefault(),
                SalesPerson = _db.User.ToList(),
                Products = productList.ToList()
            };

            return View(objAppointmentVM);

        }


        //POST Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _db.Order.FindAsync(id);
            _db.Order.Remove(appointment);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}