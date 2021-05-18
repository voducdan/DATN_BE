using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DATN.DAL.Models
{
    public class DanhSachDN_Job
    {
        [Key]
        public int ma_cong_viec { get; set; }
        [Key]
        public int? ma_doanh_nghiep { get; set; }
        public DateTime ngay_dang { get; set; }
        public bool trang_thai { get; set; }
        public virtual DoanhNghiep DoanhNghiep { get; set; }
        public virtual  Job Job { get; set; }
    }
}
