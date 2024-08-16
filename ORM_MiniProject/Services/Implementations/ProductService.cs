using ORM_MiniProject.DTOs.ProductDTOs;
using ORM_MiniProject.Exceptions;
using ORM_MiniProject.Models;
using ORM_MiniProject.Repositories.Implementations;
using ORM_MiniProject.Repositories.Interfaces;
using ORM_MiniProject.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_MiniProject.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductsRepository _productsRepository;
        public ProductService()
        {
            _productsRepository = new ProductsRepository();
        }
        public async Task CreateProductAsync(ProductPostDto product)
        {
            if (string.IsNullOrWhiteSpace(product.Name)) throw new InvalidProductException("Invalid product name");
            if (string.IsNullOrWhiteSpace(product.Description)) throw new InvalidProductException("Invalid product description");
            var isExist = await _productsRepository.IsExistAsync(x => x.Name.ToLower() == product.Name.ToLower());
            if (isExist)
            {
                throw new InvalidProductException("A product with the same name already exists.");
            }
            if (product.Stock <= 0) throw new InvalidProductException("stock cannot be lower than zero");
            if (product.Price < 0) throw new InvalidProductException("price must be greater than zero");

            Products dbProduct = new Products
            {
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Description = product.Description,
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow
            };

            await _productsRepository.CreateAsync(dbProduct);
            await _productsRepository.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _getProductById(id);
            _productsRepository.Delete(product);
            await _productsRepository.SaveChangesAsync();
        }

        public async Task<List<ProductGetDto>> GetAllProductsAsync()
        {
            var products = await _productsRepository.GetAllAsync();
            List<ProductGetDto> dtos = new List<ProductGetDto>();

            foreach (var item in products)
            {
                ProductGetDto productGetDto = new ProductGetDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    Stock = item.Stock,
                    Description = item.Description,
                    CreatedTime = item.CreatedTime,
                    UpdatedTime = item.UpdatedTime,
                    OrderDetails = item.OrderDetails
                };
                dtos.Add(productGetDto);
            }

            return dtos;
        }
        public async Task<ProductGetDto> GetProductAsync(int id)
        {
            var product = await _getProductById(id);
            ProductGetDto dto = new ProductGetDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Description = product.Description,
                CreatedTime = product.CreatedTime,
                UpdatedTime = product.UpdatedTime,
                OrderDetails = product.OrderDetails
            };

            return dto;
        }

        public async Task<List<ProductGetDto>> SearchProductByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new InvalidProductNameException("cannot be null");
            var products = await _productsRepository.GetFilterAsync(x => x.Name.Contains(name));
            if(products == null||products.Count==0) throw new NotFoundException("No product found with the specified name.");
            var result = products.Select(product => new ProductGetDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Description = product.Description,
                CreatedTime = product.CreatedTime,
                UpdatedTime = product.UpdatedTime,
                OrderDetails = product.OrderDetails
            }).ToList();

            return result;
        }

        public async Task UpdateProductAsync(ProductPutDto product)
        {
            var dbProduct = await _getProductById(product.Id);

            if (await _productsRepository.IsExistAsync(x => x.Name.ToLower() == product.Name.ToLower() && x.Id != product.Id))
            {
                throw new InvalidProductException("A product with the same name already exists.");
            }
            if (string.IsNullOrWhiteSpace(product.Name)) throw new InvalidProductException("Invalid product name");
            if (string.IsNullOrWhiteSpace(product.Description)) throw new InvalidProductException("Invalid product description");
            if (product.Stock <= 0) throw new InvalidProductException("stock cannot be lower than zero");
            if (product.Price < 0) throw new InvalidProductException("price must be greater than zero");

            dbProduct.Name = product.Name;
            dbProduct.Price = product.Price;
            dbProduct.Stock = product.Stock;
            dbProduct.Description = product.Description;
            dbProduct.UpdatedTime = DateTime.UtcNow;

            _productsRepository.Update(dbProduct);
            await _productsRepository.SaveChangesAsync();
        }

        private async Task<Products> _getProductById(int id)
        {
            var product = await _productsRepository.GetAsync(x => x.Id == id);

            if (product is null)
                throw new NotFoundException("Product not found");

            return product;
        }

    }
}
