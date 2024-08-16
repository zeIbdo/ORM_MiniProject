﻿using ORM_MiniProject.Enums;
using ORM_MiniProject.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_MiniProject.DTOs.OrderDTOs
{
    public class OrderExcelDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public Users User { get; set; } = null!;
    }
}
