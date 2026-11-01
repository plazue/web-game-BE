using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace template.Models
{
    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
        [Display(Name = "Ảnh đại diện sản phẩm")]
        public HttpPostedFileBase ImageFile { get; set; }

        [Display(Name = "Ảnh phụ bổ sung")]
        public HttpPostedFileBase[] SubImageFiles { get; set; }
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

        // BẮT BUỘC SỐ LƯỢNG TỒN KHO
        [Required(ErrorMessage = "Vui lòng nhập số lượng tồn kho!")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không được âm.")]
        public Nullable<int> StockQuantity { get; set; }

        public Nullable<decimal> SalePrice { get; set; }

        
        public string ProductImage { get; set; }

        public string ProductImages1 { get; set; }
    }
}