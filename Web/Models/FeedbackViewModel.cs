using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class FeedbackViewModel
    {
        public int ID { set; get; }

        [StringLength(250, ErrorMessage = "Họ tên không nhập quá 250 ký tự")]
        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string Name { set; get; }

        [StringLength(250, ErrorMessage = "Email không nhập quá 250 ký tự")]
        public string Email { set; get; }

        [StringLength(500, ErrorMessage = "Tin nhắn không nhập quá 500 ký tự")]
        public string Message { set; get; }

        public DateTime CreatedDate { set; get; }

        public bool Status { set; get; }

        public ContactDetailViewModel ContactDetail { get; set; }
    }
}