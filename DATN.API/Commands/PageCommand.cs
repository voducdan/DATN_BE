using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN.API.Commands
{
    public class PageCommand
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; } = 5;
    }
}
