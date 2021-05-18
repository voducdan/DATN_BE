using DATN.API.Commands;
using DATN.DAL.Models;
using DATN.DAL.Repository;
using DATN.DAL.Services;
using DATN.Infrastructure.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DATN.API.Controllers
{
    [Route("api/v1/myjobs")]
    public class MyJobController : Controller
    {
        private DanhSachMyJobRepository danhSachMyJobRepository;
        private JobRepository jobRepository;

        public MyJobController(DanhSachMyJobRepository danhSachMyJobRepository, JobRepository jobRepository)
        {
            this.danhSachMyJobRepository = danhSachMyJobRepository;
            this.jobRepository = jobRepository;
        }


        // GET: api/v1/myjob
        //[HttpGet]
        //public async Task<IActionResult> GetList([FromQuery] int ma_kh, int? pageIndex)
        //{
        //    try
        //    {
        //        ListResponse<DanhSachMyJob> myJobs = new ListResponse<DanhSachMyJob>();
        //        string searchKey = null;

        //        myJobs = await danhSachMyJobRepository.GetList(searchKey, pageIndex, w => w.ma_kh == ma_kh);


        //        ListResponse<Job> jobs = new ListResponse<Job>();
        //        for (int i = 0; i < myJobs.TotalRecord; i++)
        //        {
        //            jobs = await jobRepository.GetList(searchKey, pageIndex, w => w.ma_cong_viec == myJobs)

        //                }
        //        return Ok(new
        //        {
        //            code = 200,
        //            data = myJobs
                   
        //        });

        //    }
        //    catch(NullReferenceException e)
        //    {
        //        return BadRequest(new
        //        {
        //            code = 400,
        //            error = "Fail : error" + e.InnerException.Message
        //        });
        //    }
        //}

        // POST: api/v1/myjobs
        [HttpPost]
        public async Task<IActionResult> AddMyJob([FromBody] CreateMyJobCommand command)
        {
            try
            {
                DanhSachMyJob myJob = new DanhSachMyJob();
                myJob.ma_kh = command.ma_kh;
                myJob.ma_cong_viec = command.ma_cong_viec;

                await danhSachMyJobRepository.Create(myJob);
                return Ok(new
                {
                    code = 200,
                    success = "Add My Job is success"
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    code = 400,
                    error = "Fail: error" + e.InnerException.Message
                });
            }
        }


        //DELTE: api/v1/myjobs?ma_kh=&ma_cv=
        [HttpDelete]
        public async Task<IActionResult> DeleteMyJob([FromQuery]int ma_kh, int ma_cv)
        {
            try
            {
                DanhSachMyJob myJob = new DanhSachMyJob();
                                
                myJob = await danhSachMyJobRepository.Get(w => w.ma_kh == ma_kh && w.ma_cong_viec == ma_cv);
                await danhSachMyJobRepository.Delete(myJob);
                return Ok(new
                {
                    code = 200,
                    success = "Remove Job from My Job success"
                });

            }
            catch (NullReferenceException e)
            {
                return BadRequest(new
                {
                    code = 400,
                    error = "Delete my job fail : Error " + e.InnerException.Message
                });
            }
        }
 
    }
}
