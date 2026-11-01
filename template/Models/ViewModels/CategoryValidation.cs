using System;
using System.ComponentModel.DataAnnotations;

namespace template.Models.ViewModels
{
    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category
    {
    }

    public class CategoryMetadata
    {
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên danh mục!")]
        [StringLength(100, ErrorMessage = "Tên danh mục không được vượt quá 100 ký tự.")]
        [Display(Name = "Tên danh mục")]
        public string CategoryName { get; set; }

        [StringLength(100, ErrorMessage = "Slug không được vượt quá 100 ký tự.")]
        [Display(Name = "Đường dẫn thân thiện (Slug)")]
        public string Slug { get; set; }
    }
}