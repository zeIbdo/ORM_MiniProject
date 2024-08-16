using ClosedXML.Excel;
using ORM_MiniProject.DTOs.UserDTOs;
using ORM_MiniProject.Exceptions;
using ORM_MiniProject.Models;
using ORM_MiniProject.Repositories.Implementations;
using ORM_MiniProject.Repositories.Interfaces;
using ORM_MiniProject.Services.Interfaces;
using System.Text.RegularExpressions;

namespace ORM_MiniProject.Services.Implementations
{
    public class UserService:IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IOrdersRepository _orderRepository;
        public UserService()
        {
            _usersRepository = new UsersRepository();
            _orderRepository = new OrdersRepository();
        }
        public async Task CreateUserAsync(UserPostDto user)
        {
            if(string.IsNullOrWhiteSpace(user.FullName)) throw new InvalidUserInformationException("fullname cannot be null");
            if (string.IsNullOrWhiteSpace(user.Address)) throw new InvalidUserInformationException("address cannot be null");
            if (string.IsNullOrWhiteSpace(user.Email)) throw new InvalidUserInformationException("email cannot be null");
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!regex.IsMatch(user.Email)) throw new InvalidUserInformationException("email formati sehvdir");
            if (string.IsNullOrWhiteSpace(user.Password)) { throw new InvalidUserInformationException("password cannot be null"); }
            if (!user.Password.Any(char.IsDigit)) throw new InvalidUserInformationException("password must contain at least one digit");
            if (user.Password.Length < 8) throw new InvalidUserInformationException("password uzunlugu minimum 8 olmalidir");
            var isExist = await _usersRepository.IsExistAsync(x=>x.Email.ToLower() == user.Email.ToLower());
            if (isExist == true) { throw new SameEmailException("Users cannot have same email"); }
            Users dbUser = new Users()
            {
               FullName = user.FullName,
               Password = user.Password,
               Email = user.Email,
               Address = user.Address,
            };
            await _usersRepository.CreateAsync(dbUser);
            await _usersRepository.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _getUserById(id);
            _usersRepository.Delete(user);
            await _usersRepository.SaveChangesAsync();
        }

        //public async Task<byte[]> ExportUserOrdersToExcelAsync(int userId)
        //{
        //    var user = await _getUserById(userId);
        //    var orders = user.Orders;

        //    using (var workbook = new XLWorkbook())
        //    {
        //        var worksheet = workbook.Worksheets.Add("Orders");

        //        worksheet.Cell(1, 1).Value = "Order ID";
        //        worksheet.Cell(1, 2).Value = "Order Date";
        //        worksheet.Cell(1, 3).Value = "Total Amount";
        //        worksheet.Cell(1, 4).Value = "Status";

        //        int row = 2; 

        //        foreach (var order in orders)
        //        {
        //            worksheet.Cell(row, 1).Value = order.Id;
        //            worksheet.Cell(row, 2).Value = order.OrderDate.ToString("yyyy-MM-dd");
        //            worksheet.Cell(row, 3).Value = order.TotalAmount;
        //            worksheet.Cell(row, 4).Value = order.Status.ToString();
        //            row++;

                   
        //        }

               
        //        worksheet.Columns().AdjustToContents();

        //        using (var stream = new MemoryStream())
        //        {
        //            workbook.SaveAs(stream);
        //            return stream.ToArray();
        //        }
        //    }
        //}

        public async Task<List<UserGetDto>> GetAllUsersAsync()
        {
            var users = await _usersRepository.GetAllAsync("Orders");
            List<  UserGetDto > dtos = new List< UserGetDto >();
            foreach (var item in users)
            {
                UserGetDto userGetDto = new UserGetDto()
                {
                    Id = item.Id,
                    Address = item.Address,
                    Email = item.Email,
                    FullName = item.FullName,
                    Orders = item.Orders
                };
                dtos.Add(userGetDto);
            }
            return dtos;
        }

        public async Task<UserGetDto> GetUserAsync(int id)
        {

            var user = await _getUserById(id);
            UserGetDto dto = new UserGetDto()
            {
                FullName=user.FullName,
                Id=user.Id,
                Email=user.Email,
                Address=user.Address,
                Orders = user.Orders
            };
            return dto;
        }

        public async Task<List<Orders>>? GetUserOrdersAsync(int id)
        {
            var user =await _getUserById(id);
            var orders = user.Orders.ToList();
            return orders;
        }

        public async Task<UserGetDto> LoginAsync(UserLoginDto login)
        {
            var user = await _usersRepository.GetAsync(x => x.Email.ToLower() == login.Email.ToLower(),"Orders");
            if (user == null) throw new UserAuthenticationException("wrong email or password");
            if(user.Password!=login.Password) throw new UserAuthenticationException("wrong email or password");
            UserGetDto dto = new UserGetDto()
            {
                FullName = user.FullName,
                Address = user.Address,
                Email = user.Email,
                Id = user.Id,
                Orders = user.Orders
            };
            return dto;
        }

        public async Task UpdateUserAsync(UserPutDto user)
        {
            var dbUser =await _getUserById(user.Id);           
            if (await _usersRepository.IsExistAsync(x => x.Email.ToLower() == user.Email.ToLower())) { throw new SameEmailException("eyni mail le user ola bilmez"); }
            
            dbUser.Email = user.Email;
            dbUser.FullName = user.FullName;
            dbUser.Address = user.Address;
            dbUser.Password = user.Password;

            
            _usersRepository.Update(dbUser);
            await _usersRepository.SaveChangesAsync();
        }
        private async Task<Users> _getUserById(int id)
        {
            var user = await _usersRepository.GetAsync(x => x.Id == id,"Orders");

            if (user is null)
                throw new NotFoundException("User is not found");


            return user;
        }

    }
}
