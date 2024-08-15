using ORM_MiniProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_MiniProject.DTOs.UserDTOs
{
    public record UserGetDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public ICollection<Orders> Orders { get; set; } = new List<Orders>();

    }
}
