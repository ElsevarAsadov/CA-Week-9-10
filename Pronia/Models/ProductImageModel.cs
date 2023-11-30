namespace Pronia.Models
{
    public class ProductImageModel
    {
        public int Id { get; set; }

        public string Filename { get; set; }

        public int Type { get; set; }

        public int ProductId { get; set; }

        public ProductModel Product { get; set; }
    }
}
