using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DATN.DAL.Models
{
    public class LoaiLinhVuc
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Ma_Loai_Skill { get; set; }
        public string Ten_Loai_Skill   { get; set; }
        public string Ghi_Chu { get; set; }

    }
}
