using DATN.DAL.Context;
using DATN.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DAL.Repository
{
    public class KhachHangRepository : BaseRepository<KhachHang>
    {
        public KhachHangRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
