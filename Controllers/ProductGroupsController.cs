using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagerAPI.Models;

namespace ProductManagerAPI.Controllers
{
    public class ProductGroupDTO
    {
        public int ProductGroupID { get; set; }
        public string? ProductGroupName { get; set; }
        public int? MainGroupID { get; set; }
        public List<ProductGroupDTO>? SubGroups { get; set; } = new List<ProductGroupDTO>();
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ProductGroupsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductGroupsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductGroupDTO>>> GetProductsGroups()
        {
            if (_context.ProductGroups == null)
            {
                return NotFound();
            }

            var allGroups = await _context.ProductGroups.ToListAsync();

            ProductGroupDTO MapToDTO(ProductGroup group)
            {
                return new ProductGroupDTO
                {
                    ProductGroupID = group.ProductGroupID,
                    ProductGroupName = group.ProductGroupName,
                    MainGroupID = group.MainGroupID,
                    SubGroups = allGroups
                        .Where(sub => sub.MainGroupID == group.ProductGroupID)
                        .Select(sub => MapToDTO(sub)) // recursive :'(
                        .ToList()
                };
            }

            var productGroupDTOs = allGroups
                .Where(pg => pg.MainGroupID == null) // main groups first
                .Select(pg => MapToDTO(pg)) //subgroups then
                .ToList();

            return productGroupDTOs;
        }
        /*
        // GET: api/ProductGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductGroup>> GetProductGroup(int id)
        {
            if (_context.ProductGroups == null)
            {
                return NotFound();
            }
            var productGroup = await _context.ProductGroups.FindAsync(id);

            if (productGroup == null)
            {
                return NotFound();
            }

            return productGroup;
        }
        */
        // PUT: api/ProductGroups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutProductGroup(int id, ProductGroup productGroup)
        {
            if (id != productGroup.ProductGroupID)
            {
                return BadRequest();
            }

            _context.Entry(productGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductGroupExists(id))
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

        // POST: api/ProductGroups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductGroup>> PostProductGroup(ProductGroup productGroup)
        {
            if (_context.ProductGroups == null)
            {
                return Problem("Entity set 'AppDbContext.ProductsGroups'  is null.");
            }
            _context.ProductGroups.Add(productGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductGroup", new { id = productGroup.ProductGroupID }, productGroup);
        }

        // DELETE: api/ProductGroups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductGroup(int id)
        {
            if (_context.ProductGroups == null)
            {
                return NotFound();
            }
            var productGroup = await _context.ProductGroups.FindAsync(id);
            if (productGroup == null)
            {
                return NotFound();
            }

            _context.ProductGroups.Remove(productGroup);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        private bool ProductGroupExists(int id)
        {
            return (_context.ProductGroups?.Any(e => e.ProductGroupID == id)).GetValueOrDefault();
        }
        */
    }
}
