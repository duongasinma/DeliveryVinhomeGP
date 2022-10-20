﻿namespace DeliveryVHGP_WebApi.ViewModels
{
    public class StoreModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? BrandStoreId { get; set; }
        public string? BrandStoreName { get; set; }
        public string? Phone { get; set; }
        public string? BuildingId { get; set; }

        public string? BuildingStore { get; set; }

        public string? StoreCateId { get; set; }
        public string? StoreCateName { get; set; }
        public bool? Status { get; set; }
        public string? CreateAt { get; set; }
        public string? UpdateAt { get; set; }
    }
}
