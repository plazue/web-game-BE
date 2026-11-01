using System;
using System.ComponentModel.DataAnnotations;

namespace template.Models.ViewModels
{
    // Khai báo MetadataType liên kết với class User partial được sinh tự động
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        // Bạn có thể viết thêm các logic phụ trợ cho class User ở đây nếu cần
    }

    // Class chứa các Data Annotations (Required, Email, Chiều dài,...)
    public class UserMetadata
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản!")]
        [StringLength(50, ErrorMessage = "Tên tài khoản không được vượt quá 50 ký tự.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ Email!")]
        [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ!")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn vai trò người dùng!")]
        public string Role { get; set; }

        public Nullable<System.DateTime> CreatedAt { get; set; }
    }
}