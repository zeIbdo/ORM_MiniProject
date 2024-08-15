using ORM_MiniProject.Models;
using ORM_MiniProject.Repositories.Implementations.Generic;
using ORM_MiniProject.Repositories.Interfaces;

namespace ORM_MiniProject.Repositories.Implementations
{
    public class UsersRepository:Repository<Users>,IUsersRepository
    {
    }
}
