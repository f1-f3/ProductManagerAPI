namespace ProductManagerAPI.Models
{
    public class Product
    {
        //property definitions
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public double ProductPrice { get; set; }
        public double ProductPriceWithVAT { get; set; }
        public double ProductVAT { get; set; }
        public DateTime ProductAdded { get; set; }
        
        //FK definitions
        public int ProductGroupID { get; set; }
        public ProductGroup? ProductGroup { get; set; }

        //M2M store realationship collection
        public ICollection<StoreProduct> StoreProducts { get; set; } = new List<StoreProduct>();
    }
}
