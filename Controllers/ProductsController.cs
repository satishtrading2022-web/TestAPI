using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApi.Data;
using TestApi.Models;

namespace TestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProductsController(AppDbContext db) => _db = db;

    /// <summary>GET - list products (optional search + paging).</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll(
        [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _db.Products.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Name.Contains(search));

        var total = await query.CountAsync();
        var items = await query.OrderBy(p => p.Id)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

        Response.Headers["X-Total-Count"] = total.ToString();
        return Ok(items);
    }

    /// <summary>GET - single product by id.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        return product is null ? NotFound() : Ok(product);
    }

    /// <summary>HEAD - check if a product exists (no body returned).</summary>
    [HttpHead("{id:int}")]
    public async Task<IActionResult> Head(int id)
    {
        var exists = await _db.Products.AnyAsync(p => p.Id == id);
        return exists ? Ok() : NotFound();
    }

    /// <summary>POST - create a product.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    public async Task<ActionResult<Product>> Create([FromBody] ProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Quantity = dto.Quantity,
            CreatedAt = DateTime.UtcNow
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    /// <summary>PUT - replace the whole product.</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> Replace(int id, [FromBody] ProductDto dto)
    {
        var product = await _db.Products.FindAsync(id);
        if (product is null) return NotFound();

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Quantity = dto.Quantity;
        product.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(product);
    }

    /// <summary>PATCH - update only the fields you send.</summary>
    [HttpPatch("{id:int}")]
    public async Task<ActionResult<Product>> Patch(int id, [FromBody] ProductPatchDto dto)
    {
        var product = await _db.Products.FindAsync(id);
        if (product is null) return NotFound();

        if (dto.Name is not null) product.Name = dto.Name;
        if (dto.Description is not null) product.Description = dto.Description;
        if (dto.Price.HasValue) product.Price = dto.Price.Value;
        if (dto.Quantity.HasValue) product.Quantity = dto.Quantity.Value;
        product.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(product);
    }

    /// <summary>DELETE - remove a product.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product is null) return NotFound();

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>OPTIONS - list the HTTP methods supported by this resource.</summary>
    [HttpOptions]
    [HttpOptions("{id:int}")]
    public IActionResult Options()
    {
        Response.Headers["Allow"] = "GET, HEAD, POST, PUT, PATCH, DELETE, OPTIONS";
        return Ok();
    }
}
