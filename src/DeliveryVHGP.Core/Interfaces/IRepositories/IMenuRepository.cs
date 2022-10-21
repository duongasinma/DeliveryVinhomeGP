﻿using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.Core.Interface.IRepositories
{
    public interface IMenuRepository : IRepositoryBase<Menu>
    {
        Task<MenuDto> GetMenuDetail(string menuId);
        Task<List<MenuView>> GetListMenuByModeId(string modeId);
        Task<MenuView> GetMenuByModeAndGroupByStore(string modeId, int page, int pageSize);
        Task<MenuView> GetMenuByModeAndGroupByCategory(string modeId, int page, int pageSize) ;
        Task<List<CategoryStoreInMenu>> GetMenuByMenuIdAndStoreIdAndGroupByCategory(string menuId, string storeId, int page, int pageSize);
        Task<CategoryStoreInMenu> GetAllProductInMenuByStoreId(string storeId, string menuId, int page, int pageSize);
        Task<CategoryStoreInMenu> GetAllProductInMenuByCategoryId(string categoryId, string menuId, int page, int pageSize);
        Task<CategoryStoreInMenu> GetAllProductInMenuByCategoryIdAndStoreId(string storeId, string categoryId, string menuId, int page, int pageSize);
        Task<List<ProductViewInList>> GetListProductInMenuByStoreId(string storeId, string menuId, int page, int pageSize);
        Task<List<ProductViewInList>> GetListProductInMenuByCategoryId(string categoryId, string menuId, int page, int pageSize);
        Task<List<ProductViewInList>> GetListProductNotInMenuByCategoryIdAndStoreId(string storeId, string menuId, int page, int pageSize);
        Task<ProductsInMenuModel> AddProductsToMenu(ProductsInMenuModel listProduct);
        Task<MenuDto> CreatNewMenu(MenuDto menu);
        Task<MenuDto> UpdateMenu(string menuId, MenuDto menu);

    }
}
