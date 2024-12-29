using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Data;
using Api.Dtos;
using Api.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context){
            _context=context;
        }

        // GET:api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(){
            return await _context.Products.ToListAsync();
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id){
            var product=await _context.Products.FindAsync(id);
            if(product==null){
                return NotFound();
            }
            return product;
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(CreateProductDto dto){
            var product=new Product{
                Name=dto.Name,
                Description=dto.Description,
                Category=dto.Category,
                 Price=dto.Price,
                  StockQuantity=dto.StockQuantity,
                   CreatedAt=DateTime.UtcNow,
                    UpdatedAt=DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct),new { id=product.Id},product);
        }
    
        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product){
            if(id!=product.Id){
                return BadRequest();
            }

            product.UpdatedAt=DateTime.UtcNow;
            _context.Entry(product).State=EntityState.Modified;
            _context.Entry(product).Property(x=>x.CreatedAt).IsModified=false;

            try{
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException){
                if(!ProductExist(id)){
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }
        private bool ProductExist(int id){
            return _context.Products.Any(p=>p.Id==id);
        }

        // PATCH: api/products/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchProduct(int id, [FromBody] JsonPatchDocument<Product> patchDoc){
            if(patchDoc==null){
                return BadRequest();
            }

            var product=await _context.Products.FindAsync(id);
            if(product==null){
                return NotFound();
            }

            patchDoc.ApplyTo(product);
            
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            product.UpdatedAt=DateTime.UtcNow;

            try{
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException){

                if(!ProductExist(id)){
                    return NotFound();
                }
                throw;
            }

            return NoContent();
                
            
        }
    
        //DELETE api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id){
            var product=await _context.Products.FindAsync(id);
            if(product==null){
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}