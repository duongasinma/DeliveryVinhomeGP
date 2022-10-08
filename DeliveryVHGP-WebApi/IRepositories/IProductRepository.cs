﻿using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDetailsModel>> GetAll(string storeId,int pageIndex, int pageSize);
        Task<ProductDetailsModel> GetById(string proId);
        Task<ProductModel> CreatNewProduct(ProductModel pro);
        Task<Object> UpdateProductDetailById(string proId, ProductDetailsModel product);
        Task<Object> DeleteProductById(string id);
        Task<Object> PostFireBase(IFormFile file);
    }
}
