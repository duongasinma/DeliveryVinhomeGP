﻿using System;
using System.Collections.Generic;

namespace DeliveryVHGP.WebApi.Models
{
    public partial class OrderCache
    {
        public string Id { get; set; } = null!;
        public string OrderId { get; set; } = null!;
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public virtual Order Order { get; set; } = null!;
    }
}
