using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DATN.DAL.Models
{
    public class KhachHang
    {
        [Key]
        public int  ma_kh   { get; set; }
        public string ten_kh { get; set; }

        public string sdt { get; set; }
  
    }
}
