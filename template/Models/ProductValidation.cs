using System;
using System.ComponentModel.DataAnnotations;

namespace template.Models.ViewModels
{
    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
    }

    public class ProductMetadata
    {
        public int ProductID { get; set; }

        public Nullable<int> CategoryID { get; set; }

        [StringLength(100, ErrorMessage = "Tên thương hiệu không được vượt quá 100 ký tự.")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm!")]
        [StringLength(200, ErrorMessage = "Tên sản phẩm tối đa 200 ký tự.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá sản phẩm!")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn hoặc bằng 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng tồn kho!")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không được âm.")]
        public Nullable<int> StockQuantity { get; set; }

        public Nullable<decimal> SalePrice { get; set; }
    }
}