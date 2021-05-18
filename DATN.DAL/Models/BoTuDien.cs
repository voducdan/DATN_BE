using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DAL.Models
{
    public class BoTuDien
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id { get; set; }
        public int ma_tu { get; set; }
        public int tu_lien_quan { get; set; }
        public string loai_tu { get; set; }
        public float do_lien_quan { get; set; }
    }
}
