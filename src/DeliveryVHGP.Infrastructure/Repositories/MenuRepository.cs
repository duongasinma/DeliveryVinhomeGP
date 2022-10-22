﻿using DeliveryVHGP.Core.Interface.IRepositories;
using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Infrastructure.Repositories.Common;

namespace DeliveryVHGP.WebApi.Repositories
{
    public class MenuRepository : RepositoryBase<Menu>, IMenuRepository
    {
        public MenuRepository(DeliveryVHGP_DBContext context) : base(context)
        {
        }

        public async Task<List<MenuView>> GetListMenuByModeId(string modeId)
        {
            var listMenu = await context.Menus.Where(m => m.ModeId == modeId).Select(x => new MenuView { 
                Id = x.Id, 
                Image = x.Image,
                Name = x.Name,
                StartTime = x.StartHour,
                EndTime = x.EndHour            
            }).ToListAsync();
            //if (!listMenu.Any())
            //{
            //    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, String.Format("Contact {0} not found.", modeId)));
            //}
            return listMenu;
        }
        public async Task<MenuDto> GetMenuDetail(string menuId)
        {
            var menu = await context.Menus.FindAsync(menuId);
            if (menu == null)
            {
                throw new Exception("Not Found");
            }
            List<string> cateId = await context.CategoryInMenus.Where(x => x.MenuId == menuId).Select(x => x.CategoryId).ToListAsync();

            MenuDto menuDto = new MenuDto
            {
                Name = menu.Name,
                Image = menu.Image,
                DayFilter = menu.DayFilter,
                StartDate = menu.StartDate,
                EndDate = menu.EndDate,
                HourFilter = menu.HourFilter,
                StartHour = menu.StartHour,
                EndHour = menu.EndHour,
                listCategory = cateId,
                ModeId = menu.ModeId
            };
            return menuDto;
        }

        //Get a menu by mode id and show list category (in customer web) 
        public async Task<MenuNotProductView> GetMenuByModeAndShowListCategory(string modeId)
        {
            double time = await GetTime();
            var menuView = await context.Menus.Where(x => x.ModeId == modeId).Where(x => x.StartHour <= time).Where(x => x.EndHour > time).Select(x => new MenuNotProductView
            {
                Id = x.Id,
                Name = x.Name,
                Image = x.Image,
                StartTime = x.StartHour,
                EndTime = x.EndHour
            }).FirstOrDefaultAsync();
            if (menuView.Id == null) throw new Exception("Not found menu");

            var listCategory = await (from menu in context.Menus
                                      join cm in context.CategoryInMenus on menu.Id equals cm.MenuId
                                      join category in context.Categories on cm.CategoryId equals category.Id
                                      where menu.Id == menuView.Id
                                      select new CategoryInMenuView
                                      {
                                          Id = category.Id,
                                          Name = category.Name,
                                          Image = category.Image
                                      }).ToListAsync();

            menuView.ListCategoryInMenus = listCategory;
            return menuView;
        }
        //Get list store category in menu by mode
        public async Task<List<StoreCategoryInMenuView>> GetListStoreCateInMenuNow(string modeId, int storeCateSize, int storeSize)
        {
            double time = await GetTime();
            var menuId = await context.Menus.Where(x => x.ModeId == modeId).Where(x => x.StartHour <= time).Where(x => x.EndHour > time).Select(x => x.Id
            ).FirstOrDefaultAsync();
            if (menuId == null) throw new Exception("Not found menu");

            var listStoreCate = await (from menu in context.Menus
                                   join sm in context.StoreInMenus on menu.Id equals sm.MenuId
                                   join store in context.Stores on sm.StoreId equals store.Id
                                   join bu in context.Buildings on store.BuildingId equals bu.Id
                                   join sc in context.StoreCategories on store.StoreCategoryId equals sc.Id
                                   where menu.Id == menuId && store.Status == true
                                   select new StoreCategoryInMenuView
                                   {
                                       Id = sc.Id,
                                       Name = sc.Name                                       
                                   }).GroupBy(x => x.Id).Select(x=> x.First()).Take(storeCateSize).ToListAsync();
            foreach(var storecate in listStoreCate)
            {
                var listStore = await (from menu in context.Menus
                                       join sm in context.StoreInMenus on menu.Id equals sm.MenuId
                                       join store in context.Stores on sm.StoreId equals store.Id
                                       join bu in context.Buildings on store.BuildingId equals bu.Id
                                       join sc in context.StoreCategories on store.StoreCategoryId equals sc.Id
                                       where menu.Id == menuId && store.Status == true && store.StoreCategoryId == storecate.Id
                                       select new StoreInMenuView
                                       {
                                           Id = store.Id,
                                           Name = store.Name,
                                           Image = store.Image,
                                           Building = bu.Name,
                                           StoreCategory = sc.Name
                                       }).Take(storeSize).ToListAsync();
                if(listStore != null) storecate.ListStores = listStore;
            }
            
            return listStoreCate;
        }
        //Get a menu by mode id and show list store (in customer web) 
        public async Task<List<StoreInMenuView>> GetListStoreInMenuNow(string modeId, int page, int pageSize)
        {
            double time = await GetTime();
            var menuId = await context.Menus.Where(x => x.ModeId == modeId).Where(x => x.StartHour <= time).Where(x => x.EndHour > time).Select(x => x.Id
            ).FirstOrDefaultAsync();
            if (menuId == null) throw new Exception("Not found menu");

            var listStore = await (from menu in context.Menus
                                   join sm in context.StoreInMenus on menu.Id equals sm.MenuId
                                   join store in context.Stores on sm.StoreId equals store.Id
                                   join bu in context.Buildings on store.BuildingId equals bu.Id
                                   join sc in context.StoreCategories on store.StoreCategoryId equals sc.Id
                                   where menu.Id == menuId && store.Status == true
                                   select new StoreInMenuView
                                   {
                                       Id = store.Id,
                                       Name = store.Name,
                                       Image = store.Image,
                                       Building = bu.Name,
                                       StoreCategory = sc.Name
                                   }).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return listStore;
        }
        //Get a menu by mode id and group by store (in customer web) 
        public async Task<MenuView> GetMenuByModeAndGroupByStore(string modeId, int page, int pageSize)
        {
            double time = await GetTime();
            var menuView = await context.Menus.Where(x => x.ModeId == modeId).Where(x => x.StartHour <= time).Where(x => x.EndHour > time).Select(x => new MenuView
            {
                Id = x.Id,
                Name = x.Name,
                Image = x.Image,
                StartTime = x.StartHour,
                EndTime = x.EndHour
            }).FirstOrDefaultAsync();
            if (menuView.Id == null) throw new Exception("Not found menu");

            var listStore = await (from menu in context.Menus
                                   join sm in context.StoreInMenus on menu.Id equals sm.MenuId
                                   join store in context.Stores on sm.StoreId equals store.Id
                                   where menu.Id == menuView.Id && store.Status == true
                                   select new CategoryStoreInMenu
                                   {
                                       Id = store.Id,
                                       Name = store.Name,
                                       Image = store.Image

                                   }).ToListAsync();
            foreach (var store in listStore)
            {
                var listProduct = await GetListProductInMenuByStoreId(store.Id, menuView.Id, page, pageSize);
                store.ListProducts = listProduct;
            }
            menuView.ListCategoryStoreInMenus = listStore;
            return menuView;
        }

        //Get a menu by mode id and group by category (in customer web) 
        public async Task<MenuView> GetMenuByModeAndGroupByCategory(string modeId, int page, int pageSize)
        {
            double time = await GetTime();
            var menuView = await context.Menus.Where(x => x.ModeId == modeId).Where(x => x.StartHour <= time).Where(x => x.EndHour > time).Select(x => new MenuView
            {
                Id = x.Id,
                Name = x.Name,
                Image = x.Image,
                StartTime = x.StartHour,
                EndTime = x.EndHour
            }).FirstOrDefaultAsync();
            if (menuView == null) throw new Exception("Not found menu");

            var listCategory = await (from menu in context.Menus
                                  join cm in context.CategoryInMenus on menu.Id equals cm.MenuId
                                  join category in context.Categories on cm.CategoryId equals category.Id
                                  where menu.Id == menuView.Id
                                  select new CategoryStoreInMenu
                                  {
                                      Id = category.Id,
                                      Name = category.Name,
                                      Image = category.Image
                                  }).ToListAsync();
            foreach (var category in listCategory)
            {
                var listProduct = await GetListProductInMenuByCategoryId(category.Id, menuView.Id, page, pageSize);
                if (listProduct != null)
                    category.ListProducts = listProduct;
            }
            listCategory = listCategory.Where(x => x.ListProducts != null).ToList();
            menuView.ListCategoryStoreInMenus = listCategory;
            return menuView;
        }

        // Get a menu in store include list product group by category(in store web)
        public async Task<List<CategoryStoreInMenu>> GetMenuByMenuIdAndStoreIdAndGroupByCategory(string menuId, string storeId, int page, int pageSize)
        {
            //Get list cate by menu id
            var listCategory = await (from menu in context.Menus
                                  join cm in context.CategoryInMenus on menu.Id equals cm.MenuId
                                  join category in context.Categories on cm.CategoryId equals category.Id
                                  where menu.Id == menuId
                                  select new CategoryStoreInMenu
                                  {
                                      Id = category.Id,
                                      Name = category.Name,
                                      Image = category.Image
                                  }).ToListAsync();
            foreach (var category in listCategory)
            {
                var listProduct = await GetListProductInMenuByCategoryIdAndStoreId(storeId, category.Id, menuId, page, pageSize);
                if (listProduct != null)
                    category.ListProducts = listProduct;
            }
            listCategory = listCategory.Where(x => x.ListProducts != null).ToList();
            return listCategory;
        }
        public async Task<List<StoreInMenuView>> GetListStoreInMenuFilerByCategory(string menuId, string categoryId, int page, int pageSize)
        {
            var stores = await (from menu in context.Menus
                                join pm in context.ProductInMenus on menu.Id equals pm.MenuId
                                join product in context.Products on pm.ProductId equals product.Id
                                join cate in context.Categories on product.CategoryId equals cate.Id
                                join store in context.Stores on product.StoreId equals store.Id
                                join bu in context.Buildings on store.BuildingId equals bu.Id
                                join sc in context.StoreCategories on store.StoreCategoryId equals sc.Id
                                where menu.Id == menuId && cate.Id == categoryId
                                select new StoreInMenuView
                                {
                                    Id = store.Id,
                                    Image = store.Image,
                                    Name = store.Name,
                                    Building = bu.Name,
                                    StoreCategory = sc.Name
                                }).DistinctBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return stores;
        }
        
        //--Product in with filed______________________________________________________________________________________________________
        //Get all product when click see all product in menu group by store (customer web)
        public async Task<StoreInProductView> GetAllProductInMenuByStoreId(string storeId,string menuId, int page, int pageSize)
        {
            var storeView = await (from store in context.Stores
                                join bu in context.Buildings on store.BuildingId equals bu.Id
                                join cluster in context.Clusters on bu.ClusterId equals cluster.Id
                                join area in context.Areas on cluster.AreaId equals area.Id
                                where store.Id == storeId && store.Status == true
                                select new StoreInProductView
                                {
                                    Id=store.Id,
                                    Image = store.Image,
                                    Name = store.Name,
                                    OpenTime = store.OpenTime,
                                    CloseTime = store.CloseTime,
                                    Location = bu.Name + ", " + area.Name + ", Vinhomes Grand Park"
                                }).FirstOrDefaultAsync();
            if (storeView == null) throw new Exception("Not found Store Or Store closed");
            storeView.ListProducts = await GetListProductInMenuByStoreId(storeId, menuId, page, pageSize);
            return storeView;
        }

        //Get all product when click see all product in menu group by category (customer web)
        public async Task<CategoryStoreInMenu> GetAllProductInMenuByCategoryId(string categoryId, string menuId, int page, int pageSize)
        {
            var cateView = await context.Categories.Where(x => x.Id == categoryId).Select( x => new CategoryStoreInMenu
                                   {
                                       Id = x.Id,
                                       Name = x.Name,
                                       Image = x.Image
                                   }).FirstOrDefaultAsync();
            cateView.ListProducts = await GetListProductInMenuByCategoryId(categoryId, menuId, page, pageSize);
            return cateView;
        }

        //Get all product when click see all product in menu group by category (store web)
        public async Task<CategoryStoreInMenu> GetAllProductInMenuByCategoryIdAndStoreId(string storeId, string categoryId, string menuId, int page, int pageSize)
        {
            var cateView = await context.Categories.Where(x => x.Id == categoryId).Select(x => new CategoryStoreInMenu
            {
                Id = x.Id,
                Name = x.Name,
                Image = x.Image
            }).FirstOrDefaultAsync();
            cateView.ListProducts = await GetListProductInMenuByCategoryIdAndStoreId(storeId, categoryId, menuId, page, pageSize);
            return cateView;
        }

        //-- Query Product-----------------------------------------------------------------------------------------------------------

        public async Task<List<ProductViewInList>> GetListProductInMenuByStoreId(string storeId, string menuId, int page, int pageSize)
        {
            var listProducts = await (from product in context.Products
                                      join store in context.Stores on product.StoreId equals store.Id
                                      join pm in context.ProductInMenus on product.Id equals pm.ProductId
                                      join menu in context.Menus on pm.MenuId equals menu.Id
                                      where store.Id == storeId && menu.Id == menuId
                                      select new ProductViewInList
                                      {
                                          Id = product.Id,
                                          Image = product.Image,
                                          Name = product.Name,
                                          PricePerPack = pm.Price,
                                          PackDes = product.PackDescription,
                                          StoreId = storeId,
                                          StoreName = store.Name,
                                          Unit = product.Unit,
                                          MinimumDeIn = product.MinimumDeIn,
                                          productMenuId = pm.Id
                                      }).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return listProducts;
        }

        public async Task<List<ProductViewInList>> GetListProductInMenuByCategoryId(string categoryId, string menuId, int page, int pageSize)
        {
            var listProducts = await (from product in context.Products
                                      join store in context.Stores on product.StoreId equals store.Id
                                      join category in context.Categories on product.CategoryId equals category.Id
                                      join pm in context.ProductInMenus on product.Id equals pm.ProductId
                                      join menu in context.Menus on pm.MenuId equals menu.Id
                                      where category.Id == categoryId && menu.Id == menuId && store.Status == true
                                      select new ProductViewInList
                                      {
                                          Id = product.Id,
                                          Image = product.Image,
                                          Name = product.Name,
                                          PricePerPack = pm.Price,
                                          PackDes = product.PackDescription,
                                          StoreId= store.Id,
                                          StoreName = store.Name,
                                          Unit = product.Unit,
                                          MinimumDeIn = product.MinimumDeIn,
                                          productMenuId = pm.Id
                                      }).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return listProducts;
        }
        // Get products in menu of store, filter by category (load list product in menu for a store)
        public async Task<List<ProductViewInList>> GetListProductInMenuByCategoryIdAndStoreId(string storeId, string categoryId, string menuId, int page, int pageSize)
        {
            var listProducts = await (from product in context.Products
                                      join store in context.Stores on product.StoreId equals store.Id
                                      join category in context.Categories on product.CategoryId equals category.Id
                                      join pm in context.ProductInMenus on product.Id equals pm.ProductId
                                      join menu in context.Menus on pm.MenuId equals menu.Id
                                      where store.Id == storeId && category.Id == categoryId && menu.Id == menuId
                                      select new ProductViewInList
                                      {
                                          Id = product.Id,
                                          Image = product.Image,
                                          Name = product.Name,
                                          PricePerPack = pm.Price,
                                          PackDes = product.PackDescription,
                                          StoreId = store.Id,
                                          StoreName = store.Name,
                                          productMenuId = pm.Id
                                      }).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return listProducts;
        }

        //Get list product in store not in menu (when add product to menu in store web)
        public async Task<List<ProductViewInList>> GetListProductNotInMenuByCategoryIdAndStoreId(string storeId, string menuId, int page, int pageSize)
        {
            var listInMenu = await context.ProductInMenus.Where(x => x.MenuId == menuId).Select(x => x.ProductId).ToListAsync();
            //list not in product bi trung khi product o 2 menu
            var listProduct = await (from product in context.Products
                                     join cate in context.Categories on product.CategoryId equals cate.Id
                                     join cm in context.CategoryInMenus on cate.Id equals cm.CategoryId
                                     join store in context.Stores on product.StoreId equals store.Id
                                     join pm in context.ProductInMenus on product.Id equals pm.ProductId into nx
                                     from x in nx.DefaultIfEmpty()
                                     where store.Id == storeId && x.MenuId != menuId && cm.MenuId == menuId
                                     select new ProductViewInList
                                     {
                                         Id = product.Id,
                                         Image = product.Image,
                                         Name = product.Name,
                                         PricePerPack = product.PricePerPack,
                                         PackDes = product.PackDescription
                                     }).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            // Remove product in menu
            foreach(var product in listInMenu)
            {
                listProduct.RemoveAll(x => x.Id == product);
            }
            // Remove duplicate
            return listProduct.GroupBy(x => x.Id).Select(x => x.First()).ToList();
        }


        //Add list product to menu
        public async Task<ProductsInMenuModel> AddProductsToMenu(ProductsInMenuModel listProduct)
        {
            List<ProductInMenu> list = new List<ProductInMenu>();
            foreach(var product in listProduct.products)
            {
                ProductInMenu pro = new ProductInMenu { Id = Guid.NewGuid().ToString(), Price = product.price, MenuId = listProduct.menuId, ProductId = product.id };
                list.Add(pro);
            }
            //Check storeId exist in StoreInMenu table
            var storeId = await context.Products.Where(x => x.Id == listProduct.products.ElementAt(0).id).Select(x => x.StoreId).FirstOrDefaultAsync();
            var storeInMenu = await context.StoreInMenus.Where(x => x.StoreId == storeId && x.MenuId == listProduct.menuId).FirstOrDefaultAsync();
            if( storeInMenu == null)
            {
                await context.StoreInMenus.AddAsync(new StoreInMenu { Id = Guid.NewGuid().ToString(), MenuId = listProduct.menuId, StoreId = storeId });
            }
            try
            {
                await context.ProductInMenus.AddRangeAsync(list);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return listProduct;
        }

        public async Task<MenuDto> CreatNewMenu(MenuDto menu)
        {
            var id = Guid.NewGuid().ToString();
            var newMenu = new Menu
            {
                Id = id,
                Name = menu.Name,
                Image = menu.Image,
                StartHour = menu.StartHour,
                EndHour = menu.EndHour,
                ModeId = menu.ModeId
            };
            foreach (var category in menu.listCategory)
            {
                var cmId = Guid.NewGuid().ToString();
                CategoryInMenu cate = new CategoryInMenu { Id = cmId, CategoryId = category, MenuId = id };
                context.CategoryInMenus.Add(cate);
            }
            try
            {
                await context.Menus.AddAsync(newMenu);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return menu;
        }

        public async Task<MenuDto> UpdateMenu(string menuId, MenuDto menu)
        {
            var menuUpdate = new Menu
            {
                Id = menuId,
                Name = menu.Name,
                Image = menu.Image,
                StartHour = menu.StartHour,
                EndHour = menu.EndHour,
                ModeId = menu.ModeId
            };
            List<String> listCate = (List<String>)await context.CategoryInMenus.Where(x => x.MenuId == menuId).Select(x => x.CategoryId).ToListAsync();
            List<String> listNewCate = (List<String>)menu.listCategory;
            //remove old category in menu
            var listCateInMenu = await context.CategoryInMenus.Where(x => x.MenuId == menuId).ToListAsync();           
            if (listCateInMenu.Any())
                context.CategoryInMenus.RemoveRange(listCateInMenu);
            //add new category in menu
            foreach (var category in menu.listCategory)
            {
                var cmId = Guid.NewGuid().ToString();
                CategoryInMenu cate = new CategoryInMenu { Id = cmId, CategoryId = category, MenuId = menuId };
                await context.CategoryInMenus.AddAsync(cate);
            }
            
            var listIntersection = listNewCate.Intersect(listCate);
            var listRemove = listCate.Except(listIntersection);
            //if (listCateInMenu.Any() && listNewCate != null)
            //{
            //    // get list intersection(giao) beewent old and new list category
            //    //listIntersection = (List<string>)listNewCate.Intersect(listCate);
            //    //listRemove = (List<String>)listCate.Except(listIntersection);
            //}
            if (listRemove.Any())
            {
                foreach (var cate in listRemove)
                {
                    var product = await (from pro in context.Products 
                                         join pm in context.ProductInMenus on pro.Id equals pm.ProductId
                                         where pro.CategoryId == cate 
                                         select pm).ToListAsync();
                    context.ProductInMenus.RemoveRange(product);
                }
            }
            context.Entry(menuUpdate).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return menu;

        }

        public async Task<double> GetTime()
        {
            DateTime utcDateTime = DateTime.UtcNow;
            string vnTimeZoneKey = "SE Asia Standard Time";
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(vnTimeZoneKey);
            string time = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, vnTimeZone).ToString("HH.mm");
            var time2 = Double.Parse(time);
            return time2;
        }
    }
}
