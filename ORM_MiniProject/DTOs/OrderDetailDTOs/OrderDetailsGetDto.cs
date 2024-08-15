using ORM_MiniProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_MiniProject.DTOs.OrderDetailDTOs
{
    public record OrderDetailsGetDto
    {
        public int OrderId { get; set; }
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerItem { get; set; }
        public Orders Order { get; set; } = null!;
        public Products Product { get; set; } = null!;
    }
}
