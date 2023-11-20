namespace ProductManagerAPI.Models
{
    public class ProductGroup
    {
        //property definitions
        public int ProductGroupID { get; set; }
        public string? ProductGroupName { get; set; }

        // Main group relationship
        public int? MainGroupID { get; set; }
        public ProductGroup? MainGroup { get; set; }

        // Subgroup relationship
        public ICollection<ProductGroup>? SubGroups { get; set; }

        // Products relationship
        public ICollection<Product>? Products { get; set; }
    }
}
