using ORM_MiniProject.Enums;

namespace ORM_MiniProject.Models;

public class Orders
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public Users User { get; set; } = null!;

    public ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
}
