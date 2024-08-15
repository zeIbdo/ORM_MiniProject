using ORM_MiniProject.DTOs.ProductDTOs;
using ORM_MiniProject.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_MiniProject.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductGetDto>> GetAllProductsAsync();
        Task<ProductGetDto> GetProductAsync(int id);
        Task UpdateProductAsync(ProductPutDto user);
        Task CreateProductAsync(ProductPostDto user);
        Task DeleteProductAsync(int id);
        Task<List<ProductGetDto>> SearchProductByNameAsync(string name);
    }
}
