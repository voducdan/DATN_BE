using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DATN.DAL.Models
{
   public class JobFilterResult
    {
        [Key]
        public int ma_cong_viec { get; set; }
        public string ten_cong_viec { get; set; }
        public string ten_cong_ty { get; set; }
        public string dia_chi { get; set; }
        public double luong_toi_thieu { get; set; }
        public double luong_toi_da { get; set; }
        public DateTime? ngay_bat_dau { get; set; }
        public DateTime? ngay_ket_thuc { get; set; }
        public string ten_skill { get; set; }
     
        public int ma_doanh_nghiep { get; set; }
        public int ma_thanh_pho { get; set; }
        public string ten_thanh_pho { get; set; }
      
    }
}
