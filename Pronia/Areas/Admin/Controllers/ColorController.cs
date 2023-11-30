using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ColorController : Controller
    {
        private readonly AppDbContext DB;
        public ColorController(AppDbContext context)
        {
            DB = context;
        }
        public IActionResult Show()
        {
            return View(DB.Colors.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ColorModel newColor) {

            if (!ModelState.IsValid) return BadRequest();


            DB.Colors.Add(newColor);
            DB.SaveChanges();


            return RedirectToAction("Show");
        
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest();

            ColorModel deleted = DB.Colors.FirstOrDefault(x => x.Id == id);

            if(deleted is null) return BadRequest();

            DB.Colors.Remove(deleted);
            DB.SaveChanges();

            return RedirectToAction("Show");
        }

        public IActionResult Update(int? id)
        {
            if(id is null) return BadRequest();

            ColorModel updated = DB.Colors.Include(x=>x.Products).FirstOrDefault(x => x.Id == id);

            if(updated is null) return BadRequest();

            return View(model: updated) ;
        }

        [HttpPost]
        public IActionResult Update(ColorModel updatedColor)
        {

            if(updatedColor is null) return BadRequest("DOIN HACKING? NIG**A");

            
            if(! ModelState.IsValid) return BadRequest("NT...");


            ColorModel old = DB.Colors.FirstOrDefault(x=>x.Id == updatedColor.Id);

            if(old is null) return BadRequest("STOP");

            old.Name = updatedColor.Name;

            DB.SaveChanges();


            return RedirectToAction("Show");
        }
    }
}
