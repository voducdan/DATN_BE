using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DATN.DAL.Models
{
    public class Skill
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ma_skill { get; set; }
        public string ten_skill { get; set; }
        public string ten_skill_1 { get; set; }
        public string ghi_chu { get; set; }
    }
}
