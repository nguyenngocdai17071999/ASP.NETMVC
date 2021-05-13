using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace HocWeb.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Bạn chưa nhập Tài Khoản ") ]
        public string TenTK { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập Mật Khẩu ")]
        public string MatKhau { get; set; }
        public bool RememberMe { get; set; }
    }
}