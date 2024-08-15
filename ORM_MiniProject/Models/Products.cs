namespace ORM_MiniProject.Models;

public class Products
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Description { get; set; } = null!;
    public DateTime CreatedTime { get; set; }
    public DateTime UpdatedTime { get; set; }
    public ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();

}
