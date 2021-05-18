using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DATN.DAL.Models
{
    public class CV
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Ma_CV { get; set; }
        public string Ten_CV { get; set; }
        public string Vi_tri_ung_tuyen { get; set; }
        public float So_nam_kinh_nghiem { get; set; }
        public string Link_cv_chi_tiet { get; set; }
        public string Ghi_chu { get; set; }
    }
}
