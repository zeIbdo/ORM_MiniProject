namespace ORM_MiniProject.Models;

public class OrderDetails
{
    public int OrderId { get; set; }
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal PricePerItem { get; set; }
    public Orders Order { get; set; } = null!;
    public Products Product { get; set; } = null!;

}
