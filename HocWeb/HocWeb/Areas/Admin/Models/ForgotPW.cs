using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;


namespace HocWeb.Areas.Admin.Models
{
    public class ForgotPW
    {
        [Required(ErrorMessage = "Bạn chưa nhập Email ")]     
        public string Email { get; set; }
    }
}