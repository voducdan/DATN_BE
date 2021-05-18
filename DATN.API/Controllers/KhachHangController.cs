using DATN.API.Commands;
using DATN.DAL.Models;
using DATN.DAL.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN.API.Controllers
{
    [Route("api/v1/customers")]
    public class KhachHangController : Controller
    {
        private KhachHangRepository khachHangRepository;

        public KhachHangController(KhachHangRepository khachHangRepository)
        {
            this.khachHangRepository = khachHangRepository;
        }

        // PUT: api/v1/customers?id
        //[HttpPut]
        //public async Task<IActionResult> Update(int id, [FromBody] UpdateKhachHangCommand command)
        //{
        //    try
        //    {
        //        KhachHang kh = new KhachHang();
        //        kh = await khachHangRepository.Get(w => w.ma_kh == id);
        //        if (kh != null)
        //        {
        //            //KhachHang khUpdate = new KhachHang();

        //            //khUpdate.ma_kh = id;
        //            //khUpdate.email = kh.email;
        //            //khUpdate.password = kh.password;
        //            kh.ten_kh = command.ten_kh;
        //            kh.sdt = command.sdt;
        //            //khUpdate.isadmin = kh.isadmin;
        //            //khUpdate.createtime = kh.createtime;

        //            await this.khachHangRepository.Update(kh);

        //            return Ok(new
        //            {
        //                code = 200,
        //                data = kh,
        //                success = "Update customer success"
        //            });
        //        }
        //        else
        //        {
        //            return NotFound(new
        //            {
        //                code = 400,
        //                error = "Customer is not exist"
        //            });
        //        }


        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(new
        //        {
        //            code = 400,
        //            error = e.Message
        //        });

        //    }

        //}

    }
}
