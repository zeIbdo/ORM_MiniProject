namespace ORM_MiniProject.Models;

public class Users
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Address { get; set; } = null!;
    public ICollection<Orders> Orders { get; set; }= new List<Orders>();
    public override string ToString()
    {
        return $"{FullName}";
    }

}
