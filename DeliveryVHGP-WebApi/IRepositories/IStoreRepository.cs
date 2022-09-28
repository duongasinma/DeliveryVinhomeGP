﻿using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IStoreRepository
    {
        Task<IEnumerable<StoreModel>> GetListStore( int pageIndex, int pageSize);
        Task<IEnumerable<StoreModel>> GetListStoreInBrand(string brandName, int pageIndex, int pageSize);
        Task<IEnumerable<StoreModel>> GetListStoreByName(string storeName, int pageIndex, int pageSize);
        Task<ProductModel> CreatNewProduct(ProductModel pro );
    }
}
