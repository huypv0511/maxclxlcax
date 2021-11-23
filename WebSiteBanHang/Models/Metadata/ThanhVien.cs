using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebSiteBanHang.Models
{
    [MetadataTypeAttribute(typeof(ThanhVienMetadata))]
    public partial class ThanhVien
    {
        internal sealed class ThanhVienMetadata
        {
            public int MaThanhVien { get; set; }
            [StringLength(12, ErrorMessage = "No more than 12 characters")]
            [Required(ErrorMessage = "Do not leave blank")]
            public string TaiKhoan { get; set; }

            [Required(ErrorMessage = "Do not leave blank")]
            public string MatKhau { get; set; }


            [Required(ErrorMessage = "Do not leave blank")]
            public string NhapLaiMatKhau { get; set; }

            [Required(ErrorMessage = "Do not leave blank")]
            public string HoTen { get; set; }

            [Required(ErrorMessage = "Do not leave blank")]
            public string DiaChi { get; set; }

            [Required(ErrorMessage = "Field can't be empty")]
            [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
            [EmailAddress]
            public string Email { get; set; }



            //[Required(ErrorMessage = "Do not leave blank")]
            //[RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$", ErrorMessage = "Your email address is not in a valid format. Example of correct format: joe.example@example.org")]
            //public string Email { get; set; }
            [Required(ErrorMessage = "Do not leave blank")]
            public string SoDienThoai { get; set; }
            public string CauHoi { get; set; }
            [Required(ErrorMessage = "Do not leave blank")]
            public string CauTraLoi { get; set; }

        }
    }
}