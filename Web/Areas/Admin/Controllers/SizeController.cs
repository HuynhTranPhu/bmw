using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models;
using Web.Utility;

namespace Web.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class SizeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SizeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Size.ToList());
        }
        //GET Create Action Method
        public IActionResult Create()
        {
            return View();
        }

        //POST Create action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Size size)
        {
            if (ModelState.IsValid)
            {
                size.Status = true;
                _db.Add(size);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(size);
        }
        //GET Edit Action Method
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _db.Size.FindAsync(id);
            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }

        //POST Edit action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Size size)
        {
            if (id != size.SizeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Update(size);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(size);
        }
        //GET Details Action Method
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _db.Size.FindAsync(id);
            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }


        //GET Delete Action Method
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _db.Size.FindAsync(id);
            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }
        //POST Delete action Method
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed( int id)
        {
            //var size = await _db.Size.FindAsync(id);
            //_db.Size.Remove(size);
            //await _db.SaveChangesAsync();
            var size = await _db.Size.FindAsync(id);
            size.Status = false;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}