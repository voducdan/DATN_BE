using DATN.DAL.Models;
using DATN.DAL.Repository;
using DATN.DAL.Services;
using DATN.Infrastructure.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN.API.Controllers
{
    [Route("api/v1/admin")]
    public class AdminController : Controller
    {
        private DanhSachDNJobRepository danhSachDNJobRepository;
        private DoanhNghiepRepository doanhNghiepRepository;
        private JobService jobService;


        public AdminController(DanhSachDNJobRepository danhSachDNJobRepository, DoanhNghiepRepository doanhNghiepRepository, JobService jobService)
        {
            this.danhSachDNJobRepository = danhSachDNJobRepository;
            this.doanhNghiepRepository = doanhNghiepRepository;
            this.jobService = jobService;
        }


      

        // GET: api/v1/admin
        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] string? ten_doanh_nghiep, DateTime? startTime ,int? pageIndex)
        {
            try
            {
                string tempt = null;

                ListResponse<DanhSachDN_Job> list = new ListResponse<DanhSachDN_Job>();
                DoanhNghiep doanhNghiep = new DoanhNghiep();



                int key = 0;
                while (key == 0)
                {
                    if ((ten_doanh_nghiep == null) && (startTime == null)) { key = 1; break; }
                    if ((ten_doanh_nghiep == null) && (startTime != null)) { key = 2; break; }
                    if ((ten_doanh_nghiep != null) && (startTime == null)) { key = 3; break; }
                    if ((ten_doanh_nghiep != null) && (startTime != null)) { key = 4; break; }
                } 

                switch (key)
                {
                    case 1:
                        list = await danhSachDNJobRepository.GetList(tempt, pageIndex, w => w.trang_thai == false);
                        break;
                    case 2:
                        list = await danhSachDNJobRepository.GetList(tempt, pageIndex, w => w.trang_thai == false && w.ngay_dang >= startTime);
                        break;
                    case 3:
                        doanhNghiep = await doanhNghiepRepository.Get(w => w.ten_doanh_nghiep == ten_doanh_nghiep);

                        if (doanhNghiep == null)
                        {
                            return NotFound(new
                            {
                                data = "No results"
                            });
                        }
                        else
                        {
                            list = await danhSachDNJobRepository.GetList(tempt, pageIndex, w => w.trang_thai == false && w.ma_doanh_nghiep == doanhNghiep.ma_doanh_nghiep);
                        }
                        break;
                    case 4:
                        doanhNghiep = await doanhNghiepRepository.Get(w => w.ten_doanh_nghiep == ten_doanh_nghiep);

                        if (doanhNghiep == null)
                        {
                            return NotFound(new
                            {
                                data = "No results"
                            });
                        }
                        else
                        {
                            list = await danhSachDNJobRepository.GetList(tempt, pageIndex, w => w.trang_thai == false && w.ma_doanh_nghiep == doanhNghiep.ma_doanh_nghiep && w.ngay_dang >= startTime);
                        }
                        break;      
                }
                if (list != null)
                {
                    return Ok(new
                    {
                        code = 200,
                        data = list,
                        success = "sucesss"
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        data = "Null"
                    });
                }                   
            }
            catch(NullReferenceException e)
            {
                return BadRequest(new
                {
                    code = 400,
                    error = "Get list job of company Fail : Error" + e.InnerException.Message
                });

            }
        }


        // POST: api/v1/admin
        [HttpPost]
        public async Task<IActionResult> ManagePostJob([FromQuery] int ma_dn, int ma_cv)
        {
            try
            {
                DanhSachDN_Job jobDN = new DanhSachDN_Job();
                jobDN = await danhSachDNJobRepository.Get(w => w.ma_doanh_nghiep == ma_dn && w.ma_cong_viec == ma_cv);

                jobDN.trang_thai = true;

                await danhSachDNJobRepository.Update(jobDN);

                return Ok(new
                {
                    code = 200,
                    success = "Update status post job is success"
                });

            }
            catch(Exception e)
            {
                return BadRequest(new
                {
                    error = "Manage Post Job fail Error : " + e.InnerException.Message
                });
            }
        }

        // DELETE: api/v1/admin
        [HttpDelete]
        public async Task<IActionResult> DeletePostJob([FromQuery] int ma_dn, int ma_cv)
        {
            try
            {
                DanhSachDN_Job jobDN = await danhSachDNJobRepository.Get(w => w.ma_doanh_nghiep == ma_dn && w.ma_cong_viec == ma_cv);


                await danhSachDNJobRepository.Delete(jobDN);

                return Ok(new
                {
                    code = 200,
                    success = "Delete post job is success"
                });

            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    error = "Delete Post Job fail Error : " + e.InnerException.Message
                });
            }
        }
    }
}
