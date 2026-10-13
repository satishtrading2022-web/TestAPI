using System.ComponentModel.DataAnnotations;

namespace TestApi.Models;

public class Product
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>Body for POST and PUT (full object).</summary>
public class ProductDto
{
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}

/// <summary>Body for PATCH (only send the fields you want to change).</summary>
public class ProductPatchDto
{
    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? Price { get; set; }

    [Range(0, int.MaxValue)]
    public int? Quantity { get; set; }
}
