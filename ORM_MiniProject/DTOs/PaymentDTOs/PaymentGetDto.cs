using ORM_MiniProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_MiniProject.DTOs.PaymentDTOs
{
    public record PaymentGetDto
    {
        
        
            public int Id { get; set; } 
            public int OrderId { get; set; } 
            public decimal Amount { get; set; } 
            public DateTime PaymentDate { get; set; }
            public Orders Order { get; set; } = null!;


    }
}
