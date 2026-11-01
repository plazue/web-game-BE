using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace template.Models
{
    [MetadataType(typeof(OrderMetadata))]
    public partial class Order
    {
    }

    public class OrderMetadata
    {
        public int OrderID { get; set; }

        public Nullable<int> UserID { get; set; }

        [Required(ErrorMessage = "Tổng tiền đơn hàng không được để trống!")]
        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền phải lớn hơn hoặc bằng 0.")]
        [Display(Name = "Tổng tiền")]
        public decimal Subtotal { get; set; }

        [StringLength(500, ErrorMessage = "Ghi chú đơn hàng tối đa 500 ký tự.")]
        [Display(Name = "Ghi chú")]
        public string OrderNote { get; set; }

        [StringLength(50, ErrorMessage = "Trạng thái đơn hàng tối đa 50 ký tự.")]
        [Display(Name = "Trạng thái đơn hàng")]
        public string OrderStatus { get; set; }

        [Display(Name = "Phương thức thanh toán")]
        public string PaymentType { get; set; }

        [StringLength(100, ErrorMessage = "Tên chủ thẻ không được vượt quá 100 ký tự.")]
        [Display(Name = "Tên chủ thẻ")]
        public string CardHolderName { get; set; }

        [StringLength(4, ErrorMessage = "Mã 4 số cuối thẻ chỉ được đúng 4 ký tự.")]
        [Display(Name = "4 số cuối thẻ")]
        public string CardNumberLast4 { get; set; }
    }
}