namespace ProductManagerAPI.Models
{
    public class StoreProduct
    {
        // property definitions
        public int StoreID { get; set; }
        public Store? Store { get; set; }
        public int ProductID { get; set; }
        public Product? Product { get; set; }
        public double Quantity { get; set; }
    }
}
