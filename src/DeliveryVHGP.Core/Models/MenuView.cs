﻿namespace DeliveryVHGP.Core.Models
{
    public class MenuView
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Image { get; set; }
        public double? StartTime { get; set; }
        public double? EndTime { get; set; }
        public List<CategoryStoreInMenu> ListCategoryStoreInMenus { get; set; }
    }
    public class MenuNotProductView
    {
        public String Id { get; set; }
        public String Name { get; set; }    
        public String Image { get; set; }
        public double? StartTime { get; set; }
        public double? EndTime { get; set; }
        public List<CategoryInMenuView> ListCategoryInMenus { get; set; }
    }
    public class MenuViewModel
    {
        //public int? Count { get; set; }

        public List<ProductInMenuView> Product { get; set; }
        public List<StoreInMenuVieww> Store { get; set; }
    }
}
