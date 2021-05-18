using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DATN.DAL.Models
{
    public class DanhSachJob_Skill
    {
        [Key]
        public int ma_skill { get; set; }
        [Key]
        public int ma_cong_viec { get; set; }
    }
}
