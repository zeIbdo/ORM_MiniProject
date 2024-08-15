using ORM_MiniProject.Models;
using ORM_MiniProject.Repositories.Implementations.Generic;
using ORM_MiniProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_MiniProject.Repositories.Implementations
{
    public class OrdersRepository:Repository<Orders>,IOrdersRepository
    {
    }
}
