﻿using System;
using System.Collections.Generic;

namespace DeliveryVHGP.Core.Entities
{
    public partial class Brand
    {
        public Brand()
        {
            Stores = new HashSet<Store>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Image { get; set; }

        public virtual ICollection<Store> Stores { get; set; }
    }
}
