using DATN.DAL.Context;
using DATN.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATN.DAL.Services
{
    public class KhachHangService : BaseService
    {
        public KhachHangService(DatabaseContext context) : base(context)
        {
        }

        public KhachHang GetUser_id(int id)
        {
            return context.khach_hang.FirstOrDefault(w => w.ma_kh == id);
        }
        
    }
}
