using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DAL.Models
{
    public class Account
    {
        [Key]
        public int id { get; set; }
        public string  email { get; set; }
        public string password { get; set; }
        public int? ma_kh { get; set; }
        public int? ma_dn { get; set; }
        public int rolecode { get; set; }
        public DateTime createtime { get; set; }
    }
}
