namespace ProductManagerAPI.Models
{
    public class Store
    {
        // property definitions
        public int StoreID { get; set; }
        public string StoreName { get; set; }

        // M2M Product realationship collection
        public ICollection<StoreProduct> StoreProducts { get; set; }
    }
}
