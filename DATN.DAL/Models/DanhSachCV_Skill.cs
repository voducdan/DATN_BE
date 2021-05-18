using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DATN.DAL.Models
{
    public class DanhSachCV_Skill
    {
        [Key]
        public int Ma_CV { get; set; }
        [Key]
        public int Ma_Skill { get; set; }
        public virtual CV CV { get; set; }
        public virtual Skill Skill { get; set; }
    }
}
