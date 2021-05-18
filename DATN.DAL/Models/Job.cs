using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DATN.DAL.Models
{
    public class Job
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ma_cong_viec { get; set; }
        public string ten_cong_viec { get; set; }
        public string ten_cong_viec_1 { get; set; }
        public double luong_toi_thieu { get; set; }
        public double luong_toi_da { get; set; }
        public string mo_ta_cong_viec { get; set; }
        public string don_vi_tien_te { get; set; }
        public DateTime? ngay_bat_dau { get; set; }
        public DateTime? ngay_ket_thuc { get; set; }
     }
}
