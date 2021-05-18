using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DAL.Models
{
    
    public class RelatedSkill
    {
        public string name { get; set; }
        public float relateness { get; set; }
    }

    public class SkillDictionary
    {
        public string name { get; set; }
        public string type { get; set; }
        public List<RelatedSkill> values { get; set; }
    }
}
