using ORM_MiniProject.Models;
using ORM_MiniProject.Repositories.Interfaces.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_MiniProject.Repositories.Interfaces
{
    public interface IOrdersRepository:IRepository<Orders>
    {
    }
}
