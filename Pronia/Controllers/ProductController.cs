using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;

namespace Pronia.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext DB;
        public ProductController(AppDbContext context)
        {
            DB = context;
        }

        public IActionResult ShowSingle(int? id)
        {
            if (id is null) return NotFound();

            ProductModel model = DB.Products.Include(x => x.Images).Include(x=>x.Colors).FirstOrDefault(x => x.Id == id);

            if (model is null) return NotFound();


            return View(model);
        }
    }
}
