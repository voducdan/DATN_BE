using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DATN.DAL.Models
{
    public class DanhSachCV_KH
    {
        [Key]
        public int Ma_KH { get; set; }
        [Key]
        public int Ma_CV { get; set; }

        public virtual KhachHang khachHang { get; set; }
        public virtual CV CV { get; set; }

    }
}
