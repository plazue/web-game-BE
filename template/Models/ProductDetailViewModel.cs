using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace template.Models
{
    public class ProductDetailViewModel
    {
        public int ProductID { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; }
        public List<string> ImageURLs { get; set; } // List of all image paths
    }
}