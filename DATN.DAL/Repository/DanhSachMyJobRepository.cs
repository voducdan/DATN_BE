using DATN.DAL.Context;
using DATN.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DAL.Repository
{
    public class DanhSachMyJobRepository : BaseRepository<DanhSachMyJob>
    {
        public DanhSachMyJobRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
