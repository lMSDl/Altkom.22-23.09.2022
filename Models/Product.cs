﻿using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Product : Entity
    {
        public string Name { get; set; } = string.Empty;
        public float Price { get; set; }
        public IEnumerable<Order>? Orders { get; set; }
    }
}