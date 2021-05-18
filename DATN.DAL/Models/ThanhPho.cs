using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DAL.Models
{
    public class ThanhPho
    {
        [Key]
        public int ma_thanh_pho { get; set; }
        public string ten_thanh_pho { get; set; }
    }
}
