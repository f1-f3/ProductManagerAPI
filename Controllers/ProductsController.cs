using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagerAPI.Controllers;
using ProductManagerAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductManagerAPI.Controllers
{
    public class ProductDTO
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public double ProductPrice { get; set; }
        public double ProductPriceWithVAT { get; set; }
        public double ProductVAT { get; set; }
        public DateTime ProductAdded { get; set; }

        public int ProductGroupID { get; set; }
        public string? ProductGroupName { get; set; }

        public List<StoreDTO>? Stores { get; set; }
    }

    public class ProductCreationDTO
    {
        [Required(ErrorMessage = "Product name is required")]
        public string? ProductName { get; set; }
        public double ProductPrice { get; set; }
        public double ProductVAT { get; set; }
        public double ProductPriceWithVAT { get; set; }
        public DateTime ProductAdded { get; set; }

        // these fields are required also
        public int ProductGroupID { get; set; }
        public List<StoreQuantityDTO> Stores { get; set; } = new List<StoreQuantityDTO>();

        public class StoreQuantityDTO
        {
            public int StoreID { get; set; }
            public double Quantity { get; set; }
        }
    }

    public class StoreDTO
    {
        public int StoreID { get; set; }
        public string? StoreName { get; set; }
        public double Quantity { get; set; } // Assuming you want to include this
    }




[Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
   
            var product = await _context.Products.Include(p => p.ProductGroup)
                                         .Include(p => p.StoreProducts)
                                         .ThenInclude(sp => sp.Store)
                                         .ToListAsync();

            var productDTOs = product.Select(product => new ProductDTO
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                ProductVAT = product.ProductVAT,
                ProductPriceWithVAT = product.ProductPriceWithVAT,
                ProductAdded = product.ProductAdded,
                ProductGroupID = product.ProductGroupID,
                ProductGroupName = product.ProductGroup?.ProductGroupName,
                Stores = product.StoreProducts?.Select(sp => new StoreDTO
                {
                    StoreID = sp.Store?.StoreID ?? 0,
                    StoreName = sp.Store?.StoreName,
                    Quantity = sp.Quantity
                }).ToList() ?? new List<StoreDTO>()
            }).ToList();



            return productDTOs;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {

            var product = await _context.Products.Include(p => p.ProductGroup)
                                                 .Include(p => p.StoreProducts)
                                                 .ThenInclude(sp => sp.Store)
                                                 .FirstOrDefaultAsync(p => p.ProductID == id);

            if (product == null)
            {
                return NotFound();
            }

        var productDTO = new ProductDTO
        {
            ProductID = product.ProductID,
            ProductName = product.ProductName,
            ProductPrice = product.ProductPrice,
            ProductVAT = product.ProductVAT,
            ProductPriceWithVAT = product.ProductPriceWithVAT,
            ProductAdded = product.ProductAdded,
            ProductGroupID = product.ProductGroupID,
            ProductGroupName = product.ProductGroup?.ProductGroupName,
            Stores = product.StoreProducts?.Select(sp => new StoreDTO
            {
                StoreID = sp.Store?.StoreID??0,
                StoreName = sp.Store?.StoreName,
                Quantity = sp.Quantity
            }).ToList()
        };

        return productDTO;
        }
        /*
        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductID)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        */
        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> PostProduct(ProductCreationDTO productCreationDTO)
        {
            if(productCreationDTO == null) { return NotFound(); }
            // Validate required fields
            if (productCreationDTO.ProductGroupID == 0 || !productCreationDTO.Stores.Any())
            {
                return BadRequest(" Product group and at least one store are required.");
            }

            int priceError = 0;
            priceError += productCreationDTO.ProductPrice >= 0 ? 1 : 0;
            priceError += productCreationDTO.ProductPriceWithVAT > 0 ? 1 : 0;
            priceError += productCreationDTO.ProductVAT > 0 ? 1 : 0;

            if(priceError < 2)
            {
                return BadRequest(" At least two of the following must be provided: Product price, Product price with VAT, VAT %");
            }
            if(priceError == 2)
            {
                if (productCreationDTO.ProductPriceWithVAT > 0 && productCreationDTO.ProductPrice > 0)
                {
                    //VAT % not provided
                    productCreationDTO.ProductVAT = (productCreationDTO.ProductPriceWithVAT - productCreationDTO.ProductPrice) / productCreationDTO.ProductPrice * 100;
                }
                else if (productCreationDTO.ProductPrice > 0 && productCreationDTO.ProductVAT > 0)
                {
                    // Price with VAT not provided
                    productCreationDTO.ProductPriceWithVAT = productCreationDTO.ProductPrice + (productCreationDTO.ProductPrice * productCreationDTO.ProductVAT / 100);
                }
                else if (productCreationDTO.ProductPriceWithVAT > 0 && productCreationDTO.ProductVAT > 0)
                {
                    // Price without VAT not provided
                    productCreationDTO.ProductPrice = productCreationDTO.ProductPriceWithVAT / (1 + productCreationDTO.ProductVAT / 100);
                }
            }

            if (productCreationDTO.ProductPrice <= 0)
            {
                
            }

            var product = new Product
            {
                ProductName = productCreationDTO.ProductName,
                ProductPrice = productCreationDTO.ProductPrice,
                ProductPriceWithVAT = productCreationDTO.ProductPriceWithVAT,
                ProductVAT = productCreationDTO.ProductVAT,
                ProductAdded = productCreationDTO.ProductAdded,
                ProductGroupID = productCreationDTO.ProductGroupID
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            foreach (var storeQuantity in productCreationDTO.Stores)
            {
                var storeProduct = new StoreProduct
                {
                    StoreID = storeQuantity.StoreID,
                    ProductID = product.ProductID,
                    Quantity = storeQuantity.Quantity // Set the quantity for each store
                };
                _context.StoreProducts.Add(storeProduct);
            }

            if (productCreationDTO.Stores.Any())
            {
                await _context.SaveChangesAsync();
            }
            // Map to ProductDTO
            var productDTO = new ProductDTO
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                ProductVAT = product.ProductVAT,
                ProductPriceWithVAT = product.ProductPriceWithVAT,
                ProductAdded = product.ProductAdded,
                ProductGroupID = product.ProductGroupID,
                ProductGroupName = _context.ProductGroups
                                          .Where(pg => pg.ProductGroupID == product.ProductGroupID)
                                          .Select(pg => pg.ProductGroupName)
                                          .FirstOrDefault(),
                Stores = productCreationDTO.Stores
                            .Select(storeQuantity => _context.Stores.Where(s => s.StoreID == storeQuantity.StoreID)
                            .Select(s => new StoreDTO
                            {
                                StoreID = s.StoreID,
                                StoreName = s.StoreName
                            })
                            .FirstOrDefault() ?? new StoreDTO())
                            .ToList()
            };

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductID }, productDTO);
        }
        /*
        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        */
        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductID == id)).GetValueOrDefault();
        }


    }
        
}
