using DATN.DAL.Context;
using DATN.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DAL.Repository
{
    public class DanhSachDNJobRepository : BaseRepository<DanhSachDN_Job>
    {
        public DanhSachDNJobRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
