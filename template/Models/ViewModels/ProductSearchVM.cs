using System.Collections.Generic;

namespace template.Models.ViewModels
{
    public class ProductSearchVM
    {
        public string SearchTerm { get; set; }
        public List<Product> Products { get; set; }
    }
}