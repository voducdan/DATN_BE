using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DAL.Models
{
    public class AddJob
    {
        public Job job { get; set; }
        public List<string> skills { get; set; }
        public string city { get; set; }
        public string address { get; set; }
    }
}
