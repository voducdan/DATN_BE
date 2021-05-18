using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DAL.Models
{
    public class RequestJob
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int request_id { get; set; } 
        public string ten_cong_viec { get; set; }
        public double luong_toi_thieu { get; set; }
        public double luong_toi_da { get; set; }
        public string mo_ta_cong_viec { get; set; }
        public string don_vi_tien_te { get; set; }
        public DateTime? ngay_bat_dau { get; set; }
        public DateTime? ngay_ket_thuc { get; set; }
        public int? ma_cong_ty { get; set; }
        public string danh_sach_skill { get; set; }
    }
}
