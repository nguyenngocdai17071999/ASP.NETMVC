using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HocWeb.Areas.Admin.Models
{
    public class RegisterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string CMND { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập Email ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập Địa Chỉ ")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập Tài Khoản ")]
        public string UserName { get; set; }

      
        [Required(ErrorMessage = "Bạn chưa nhập Password ")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 ký tự ")]
        public string Passswords { get; set; }


        [Required(ErrorMessage = "Bạn chưa nhập ConfirmPasssword ")]
        [Compare("Passswords", ErrorMessage = "Xác nhận mật khẩu không đúng")]
        public string ConfirmPasssword { get; set; }         
     
    }
}