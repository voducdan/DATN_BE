using DATN.DAL.Context;
using DATN.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DAL.Services
{
    public class MyJobService : BaseService
    {
        public MyJobService(DatabaseContext context) : base(context)
        {
        }
        //public List<Job> GetListMyJob(int ma_kh)
        //{
        //    List<DanhSachMyJob> myJobs = (context.danh_sach_my_job
        //        .Join(context.job, d => d.ma_cong_viec, s => s.ma_cong_viec, (d, s) => new
        //        { d.ma_kh,
        //          d.ma_cong_viec,
        //          s.ten_cong_viec,
        //        s.don_vi_tien_te,
        //        s.luong_toi_da,
        //        s.luong_toi_thieu,
        //        s.mo_ta_cong_viec,
        //        s.ngay_bat_dau,
        //        s.ngay_ket_thuc,
               
        //        s.ten_cong_viec_1,
        //        s.ten_cong_viec
        //        })
        //        .Where(w => w.ma_kh == ma_kh)
        //        .Select(job => job)).Distinct().ToList();



        //    return myJobs;

        //}
    }
       
    
}
