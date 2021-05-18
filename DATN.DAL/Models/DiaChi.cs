using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DATN.DAL.Models
{
    public class DiaChi
    {
        [Key]
        public int ma_dia_chi { get; set; }
        public int ma_thanh_pho { get; set; }
        public int ma_doanh_nghiep { get; set; }
        public string mo_ta { get; set; }
    }
}
