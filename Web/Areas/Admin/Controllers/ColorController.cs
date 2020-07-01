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
    public class ColorController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ColorController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Color.ToList());
        }
        //GET Create Action Method
        public IActionResult Create()
        {
            return View();
        }

        //POST Create action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Color color)
        {
            if (ModelState.IsValid)
            {
                color.Status = true;
                _db.Add(color);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(color);
        }
        //GET Edit Action Method
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var color = await _db.Color.FindAsync(id);
            if (color == null)
            {
                return NotFound();
            }

            return View(color);
        }

        //POST Edit action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Color color)
        {
            if (id != color.ColorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Update(color);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(color);
        }
        //GET Details Action Method
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var color = await _db.Color.FindAsync(id);
            if (color == null)
            {
                return NotFound();
            }

            return View(color);
        }


        //GET Delete Action Method
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var color = await _db.Color.FindAsync(id);
            if (color == null)
            {
                return NotFound();
            }

            return View(color);
        }
        //POST Delete action Method
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var color = await _db.Color.FindAsync(id);
            //_db.Color.Remove(color);
            //await _db.SaveChangesAsync();
            var color = await _db.Color.FindAsync(id);
            color.Status = false;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}