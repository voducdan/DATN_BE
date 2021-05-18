using DATN.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DAL.Services
{
    public class ThanhPhoService : BaseService
    {
        public ThanhPhoService(DatabaseContext context) : base(context)
        {
        }

        public async  Task<int> GetId(string ten_tp)
        {
            return  (from p in context.thanh_pho 
                    where p.ten_thanh_pho == ten_tp
                    select p.ma_thanh_pho).FirstOrDefault();
        }
    }
}
