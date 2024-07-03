﻿namespace BackendForFrontend.Models.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ProductStatus { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public int Stock { get; set; }

    }
}