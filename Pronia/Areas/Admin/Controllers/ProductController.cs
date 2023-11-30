using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Helpers;
using Pronia.Models;
using Pronia.Services;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext DB;
        
        
        public ProductController(AppDbContext context)
        {
            DB = context;
        }


   

        public IActionResult Show()
        {
            return View(model: DB.Products.Include(x=>x.Images).ToList());
        }


        public IActionResult Create()
        {
            ViewBag.Colors = DB.Colors.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductModel newProduct)
        {



            List<ColorModel> allColors = DB.Colors.ToList();
         



            if (!ModelState.IsValid)
            {
                ViewBag.Colors = allColors;
                return View();
            }


            //TODO MAKE THIS ENUM
            var errors = new Dictionary<string, string>(){


                {"product-images", "Product images are required."},
                {"product-main-image", "Main product image is required."},
                {"product-hover-image", "Hover product image is required."},
                 {"product-", "Color is required."}


            };

            bool productImagesError = false;

            bool mainProductImageError = false;

            bool hoverProductImageError = false;

            bool colorError = false;




            if (newProduct.ColorIds is null || newProduct.ColorIds.Count == 0)
            {
                ModelState.AddModelError("ColorIds", "Invalid colors");
                colorError = true;
            }


            if (newProduct.ProductImages is null) {

                ModelState.AddModelError("ProductImages", errors["product-images"]);
                productImagesError = true;

            }

            if(newProduct.MainImage is null)
            {
                ModelState.AddModelError("MainImage", errors["product-main-image"]);
                mainProductImageError = true;
            }

            if(newProduct.HoverImage is null)
            {
                ModelState.AddModelError("HoverImage", errors["product-hover-image"]);
                hoverProductImageError = true;
            }
            

            if(productImagesError || mainProductImageError || hoverProductImageError || colorError) {


                ViewBag.Colors = allColors;

                return View();
            }



            List<ColorModel> colors = new List<ColorModel>();   


            //TODO DEBUG THIS
            foreach(int colorID in newProduct.ColorIds)
            {
                ColorModel _ = new()
                {

                    Name = allColors.Where(x => x.Id == colorID).First().Name,
                };

                colors.Add(_);
            }



            List<ProductImageModel> images = new List<ProductImageModel>();


            foreach (IFormFile pro in newProduct.ProductImages) {

               string filename =  FileManager.Save(pro);


                ProductImageModel imageModel = new ProductImageModel()
                {
                    Filename = filename,
                    Type = (int) EProductImageTypes.REGULAR,
                };

                images.Add(imageModel);
            }


            string mainFilename = FileManager.Save(newProduct.MainImage);

            ProductImageModel mainImageModel = new ProductImageModel()
            {
                Filename = mainFilename,
                Type = (int) EProductImageTypes.MAIN
            };

            images.Add(mainImageModel);



            string hoverFilename = FileManager.Save(newProduct.HoverImage);

            ProductImageModel hoverImageModel = new ProductImageModel() {
                Filename = hoverFilename,
                Type = (int)EProductImageTypes.HOVER
            
            };

            images.Add(hoverImageModel);

            newProduct.Images = images;

            newProduct.Colors = colors;

            DB.Products.Add(newProduct);    

            DB.SaveChanges();
            

            return RedirectToAction("Show");
        }

        [HttpDelete]
        public IActionResult Delete(int? id) {

            if (id is null) return BadRequest();

            ProductModel deleted = DB.Products.Include(x=>x.Images).FirstOrDefault(x => x.Id == id);

            if(deleted is null) return BadRequest();

            DB.Products.Remove(deleted);


            foreach(var image in deleted.Images)
            {
                FileManager.Delete(image.Filename);
            }



            DB.SaveChanges();

            return NoContent();
            
        
        }


        public IActionResult Update(int? id)
        {


            if(id is null) return BadRequest();


            ProductModel updated = DB.Products.Include(x=>x.Images).FirstOrDefault(x => x.Id == id);
            
            if(updated is null) return BadRequest();

             ViewBag.Colors = DB.Colors.ToList();

            return View(updated);

        }

        [HttpPost]
        public IActionResult Update(ProductModel updatedProduct) {


            if (!ModelState.IsValid)
            {
                return View();
            }


            //TODO MAKE THIS ENUM
            var errors = new Dictionary<string, string>(){


                {"product-images", "Product images are required."},
                {"product-main-image", "Main product image is required."},
                {"product-hover-image", "Hover product image is required."}


            };

            bool productImagesError = false;

            bool mainProductImageError = false;

            bool hoverProductImageError = false;

            if (updatedProduct.ProductImages is null)
            {

                ModelState.AddModelError("ProductImages", errors["product-images"]);
                productImagesError = true;

            }

            if (updatedProduct.MainImage is null)
            {
                ModelState.AddModelError("MainImage", errors["product-main-image"]);
                mainProductImageError = true;
            }

            if (updatedProduct.HoverImage is null)
            {
                ModelState.AddModelError("HoverImage", errors["product-hover-image"]);
                hoverProductImageError = true;
            }

            //just uncomment this line if you do not wanna srict update
            if (productImagesError || mainProductImageError || hoverProductImageError)
            {

                return View();
            }



            List<ProductImageModel> images = new List<ProductImageModel>();



            ProductModel old = DB.Products.Include(x => x.Images).FirstOrDefault();

            old.Images.Clear();


            foreach (IFormFile image in updatedProduct.ProductImages ) //not mapped ones
            {
                string filename = FileManager.Save(image);

                ProductImageModel newImage = new ProductImageModel() { 
                    
                    Filename = filename,
                    Type = (int) EProductImageTypes.REGULAR
                
                };
            }

            string mainImageFilename = FileManager.Save(updatedProduct.MainImage);

            ProductImageModel mainImage = new ProductImageModel() { 
            
            
                Filename = mainImageFilename,
                Type = (int) EProductImageTypes.MAIN
            
            };


            string hoverImageFilename = FileManager.Save(updatedProduct.HoverImage);

            ProductImageModel hoverImage = new ProductImageModel()
            {
                Filename = hoverImageFilename,
                Type = (int) EProductImageTypes.HOVER
            };




            old.Images = images;
            old.Price = updatedProduct.Price;
            old.Description = updatedProduct.Description;
            old.Name = updatedProduct.Name;


            DB.SaveChanges();


            return RedirectToAction("Show");




        }
    }
}
