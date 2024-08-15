using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_MiniProject.DTOs.PaymentDTOs
{
    public record PaymentPostDto
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
    }
}
