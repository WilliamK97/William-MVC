﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace William.ViewModels
{
    public class ProductIndexViewModel
    {
        public ProductIndexViewModel()
        {
            ProductList = new List<ProductListViewModel>();
        }

        public class ProductListViewModel
        {
            public int ProductId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public byte[] Photo { get; set; }
            public int CategoryId { get; set; }
        }

        public string Search { get; set; }
        public string SortOrderName { get; set; } = "nameAsc";
        public string SortOrderPrice { get; set; } = "priceAsc";
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public List<ProductListViewModel> ProductList { get; set; }
    }
}