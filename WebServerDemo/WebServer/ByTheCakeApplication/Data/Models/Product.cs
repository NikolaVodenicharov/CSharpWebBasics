﻿namespace WebServer.ByTheCakeApplication.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        public Product(string name, decimal price, string imageUrl)
        {
            this.Name = name;
            this.Price = price;
            this.ImageUrl = imageUrl;
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [Required]
        [MaxLength(2000)]
        public string ImageUrl { get; set; }

        public List<OrderProduct> OrdersProducts { get; set; } = new List<OrderProduct>();
    }
}
