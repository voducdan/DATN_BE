using DATN.DAL.Context;
using DATN.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DAL.Services
{
    public class DoanhNghiepService : BaseService
    {
        public DoanhNghiepService(DatabaseContext context) : base(context)
        {
        }

        public int Count()
        {
            return (from p in context.doanh_nghiep  select p.ma_doanh_nghiep).Max();            
        }

        public async Task  Sort()
        {
            context.doanh_nghiep.OrderBy(m => m.ma_doanh_nghiep);
            await context.SaveChangesAsync();            
        }
    }
}
