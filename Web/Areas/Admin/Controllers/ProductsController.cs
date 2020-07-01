using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;
using Web.Models.ViewModel;
using Web.Utility;

namespace Web.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly HostingEnvironment _hostingEnvironment;

        [BindProperty]
        public ProductsViewModel ProductsVM { get; set; }


        public ProductsController(ApplicationDbContext db, HostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            ProductsVM = new ProductsViewModel()
            {
                Category = _db.Category.ToList(),
                ProductCategory = _db.ProductCategory.ToList(),
                Size=_db.Size.ToList(),
                Color = _db.Color.ToList(),
                Products = new Models.Products(),
              
            };

        }

        public async Task<IActionResult> Index()
        {
            var products = _db.Products.Include(m => m.Category).Include(m => m.ProductCategory).Include(m => m.Size).Include(m => m.Color);
            return View(await products.ToListAsync());
        }
        //Get : Products Create
        public IActionResult Create()
        {
            return View(ProductsVM);
        }

        //Post : Products Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            if (!ModelState.IsValid)
            {
                return View(ProductsVM);
            }

            _db.Products.Add(ProductsVM.Products);
      
            await _db.SaveChangesAsync();

            //Image being saved

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            //var nhieuFile = 

            var productsFromDb = _db.Products.Find(ProductsVM.Products.ProductID);
           
            if (files.Count != 0)
            {
                //Image has been uploaded
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(files[0].FileName);

                using (var filestream = new FileStream(Path.Combine(uploads, ProductsVM.Products.ProductID + extension), FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }
                productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.ProductID + extension;
              
                //a++;
            }
            else
            {
                //when user does not upload image
                var uploads = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultProductImage);
                System.IO.File.Copy(uploads, webRootPath + @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.ProductID + ".png");
                productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.ProductID + ".png";
                
            }
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        //GET : Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductsVM.Products = await _db.Products.Include(m => m.ProductCategory).Include(m => m.Category).Include(m => m.Size).Include(m => m.Color).SingleOrDefaultAsync(m => m.ProductID == id);

            if (ProductsVM.Products == null)
            {
                return NotFound();
            }

            return View(ProductsVM);
        }

       // Post : Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                var productFromDb = _db.Products.Where(m => m.ProductID == ProductsVM.Products.ProductID).FirstOrDefault();
       

                if (files.Count > 0 && files[0] != null)
                {
                    //if user uploads a new image
                    var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                    var extension_new = Path.GetExtension(files[0].FileName);
                    var extension_old = Path.GetExtension(productFromDb.Image);

                    if (System.IO.File.Exists(Path.Combine(uploads, ProductsVM.Products.ProductID + extension_old)))
                    {
                        System.IO.File.Delete(Path.Combine(uploads, ProductsVM.Products.ProductID + extension_old));
                    }
                    using (var filestream = new FileStream(Path.Combine(uploads, ProductsVM.Products.ProductID + extension_new), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }
                    ProductsVM.Products.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.ProductID + extension_new;
                }

                if (ProductsVM.Products.Image != null)
                {
                    productFromDb.Image = ProductsVM.Products.Image;
                }

                productFromDb.Name = ProductsVM.Products.Name;
                productFromDb.Quantity = ProductsVM.Products.Quantity;
                productFromDb.Price = ProductsVM.Products.Price;
                productFromDb.PromotionPrice = ProductsVM.Products.PromotionPrice;
                productFromDb.Detail = ProductsVM.Products.Detail;
                productFromDb.Status = ProductsVM.Products.Status;
                productFromDb.CategoryID = ProductsVM.Products.CategoryID;
                productFromDb.ProductCategoryID = ProductsVM.Products.ProductCategoryID;
                productFromDb.SizeID = ProductsVM.Products.SizeID;
                productFromDb.ColorID = ProductsVM.Products.ColorID;


                productFromDb.Warranty = ProductsVM.Products.Warranty;
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(ProductsVM);
        }
        //GET : Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductsVM.Products = await _db.Products.Include(m => m.ProductCategory).Include(m => m.Category).Include(m => m.Size).Include(m => m.Color).SingleOrDefaultAsync(m => m.ProductID == id);

            if (ProductsVM.Products == null)
            {
                return NotFound();
            }

            return View(ProductsVM);
        }

        //GET : Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductsVM.Products = await _db.Products.Include(m => m.ProductCategory).Include(m => m.Category).Include(m => m.Size).Include(m => m.Color).SingleOrDefaultAsync(m => m.ProductID == id);

            if (ProductsVM.Products == null)
            {
                return NotFound();
            }

            return View(ProductsVM);
        }

        //POST : Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            Products products = await _db.Products.FindAsync(id);

            if (products == null)
            {
                return NotFound();
            }
            else
            {
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(products.Image);

                if (System.IO.File.Exists(Path.Combine(uploads, products.ProductID + extension)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, products.ProductID + extension));
                }
                //_db.Products.Remove(products);
                products.Status = false;// Khi xóa chỉ chuyển thuộc tính status true thành false
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
        }
    }
}