using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DATN.DAL.Models
{
    public class DoanhNghiep
    {
        [Key]
        public int ma_doanh_nghiep { get; set; }
        public string ten_doanh_nghiep { get; set; }
        public string mo_ta { get; set; }
        public string email { get; set; }
        public string sdt { get; set; }
    }
}
