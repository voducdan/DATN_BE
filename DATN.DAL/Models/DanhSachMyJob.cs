using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DAL.Models
{
    public class DanhSachMyJob
    {
        [Key]
        public int ma_kh { get; set; }
        [Key]
        public int ma_cong_viec { get; set; }

    }
}
