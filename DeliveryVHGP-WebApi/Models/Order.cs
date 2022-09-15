﻿using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Payments = new HashSet<Payment>();
        }

        public string Id { get; set; } = null!;
        public string? CustomerId { get; set; }
        public string? Total { get; set; }
        public string? Type { get; set; }
        public string? HubId { get; set; }
        public string? StoreId { get; set; }
        public string? MenuId { get; set; }
        public string? BuildingId { get; set; }

        public virtual Building? Building { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual Hub? Hub { get; set; }
        public virtual Menu? Menu { get; set; }
        public virtual Store? Store { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
