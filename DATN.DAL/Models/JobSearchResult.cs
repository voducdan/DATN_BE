using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DAL.Models
{
    public class JobSearchResult
    {
        [Key]
        public int ma_cong_viec { get; set; }
        public string ten_cong_viec { get; set; }
        public int? ma_doanh_nghiep { get; set; }
        public string ten_cong_ty { get; set; }
        public string? dia_chi { get; set; }
        public string don_vi_tien_te { get; set; }
        public double luong_toi_thieu { get; set; }
        public double luong_toi_da { get; set; }
        public DateTime? ngay_bat_dau { get; set; }
        public string ten_skill { get; set; }
        public string mo_ta_cong_viec { get; set; }
    }
}
