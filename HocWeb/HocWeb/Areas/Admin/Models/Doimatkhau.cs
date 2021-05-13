using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HocWeb.Areas.Admin.Models
{
    public class Doimatkhau
    {
        

        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu mới ")]

        [StringLength(50, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 ký tự ")]
        public string NewPassswords { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu xác nhận ")]

        [Compare("NewPassswords", ErrorMessage = "Xác nhận mật khẩu không đúng")]
        public string ConfirmPasssword { get; set; }


        [Required(ErrorMessage = "Bạn chưa nhập Code ")]      
        public string Code { get; set; }


    }
}