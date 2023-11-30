using System.ComponentModel.DataAnnotations.Schema;

namespace Pronia.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }

        public List<ProductImageModel>? Images { get; set; }

        public List<ColorModel>? Colors { get; set; }



        [NotMapped]
        public List<int> ColorIds { get; set;}

        [NotMapped]
        public List<IFormFile> ProductImages { get; set; }

        [NotMapped]
        public IFormFile MainImage { get; set; }

        [NotMapped]
        public IFormFile HoverImage { get; set; }

    }
}
