using DATN.API.Commands;
using DATN.DAL.Context;
using DATN.DAL.Models;
using DATN.DAL.Repository;
using DATN.DAL.Services;
using DATN.API.Services;
using DATN.Infrastructure.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DATN.Infrastructure.Helpers;
using System.Linq.Expressions;
using MoreLinq;

namespace DATN.API.Controllers
{
    [Route("api/v1/jobs")]
    [ApiController]
    public class JobController : Controller
    {
        private JobRepository jobRepository;
        private RequestJobRepository requestJobRepository;
        private JobService jobService;
        private JWTService jWTService;
        private LevenshteinService levenshteinService;
        private IBackgroundQueue<AddJob> queue;
        private DanhSachDNJobRepository danhSachDNJobRepository;
        private CacheService cacheService;

        public JobController(JobRepository jobRepository, RequestJobRepository requestJobRepository, JobService jobService, JWTService jWTService, LevenshteinService levenshteinService, IBackgroundQueue<AddJob> queue, DanhSachDNJobRepository danhSachDNJobRepository, CacheService cacheService)
        {
            this.jobRepository = jobRepository;
            this.requestJobRepository = requestJobRepository;
            this.jobService = jobService;
            this.jWTService = jWTService;
            this.levenshteinService = levenshteinService;
            this.queue = queue;
            this.danhSachDNJobRepository = danhSachDNJobRepository;
            this.cacheService = cacheService;
        }


        // GET: api/v1/jobs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id.ToString() == null)
            {
                return BadRequest(new
                {
                    success = false,
                    error = "id is not null"
                });
            }
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Job job = await jobRepository.Get(w => w.ma_cong_viec == id);
                if (job == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        error = "Job not found"
                    });
                }
                return Ok(new
                {
                    success = true,
                    data = job
                });
            }
            catch
            {
                return NotFound(new
                {
                    success = false,
                    error = "Job not found"
                });
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetByCondition([FromQuery] DateTime? date, int pageIndex, double? salary, string? city, string? skill)

        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //Expression<Func<Job, bool>> jobByCondittion = j => j.ngay_bat_dau <= date && j.ngay_ket_thuc >= date && j.luong_toi_thieu <= salary && j.luong_toi_da >= salary;
                List<JobFilterResult> jobsByCon = await this.jobService.GetFilterResults(date, pageIndex, salary, city, skill);
                if (jobsByCon == null)
                {
                    return NotFound(new

                    {
                        success = false,
                        error = "Could not find any jobs"
                    });
                }
                return Ok(new
                {
                    success = true,
                    data = jobsByCon

                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    code = 400,
                    error = e.Message
                });
            }


        }

        [Route("search")]
        [HttpGet]
        public IActionResult SearchJobByKeyWord([FromQuery] string searchKey, int pageIndex = 0, int skipRelatedRecords = 0, int skipNormalizeRecords = 0)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                int totalJobFound = 0;
                List<string> parseSearchKey = jobService.ParseSearchey(searchKey).Distinct().ToList();

                List<JobSearchResult> jobWithoutDictionary = new List<JobSearchResult>();
                List<JobGroupBySkill> jobWithoutDictionaryGroup = new List<JobGroupBySkill>();
                foreach (string key in parseSearchKey)
                {
                    string jobWithoutDictionaryCacheKey = $"{key}_jobWithoutDictionary";
                    List<JobSearchResult> cacheResponse = cacheService.GetJobResultCache(jobWithoutDictionaryCacheKey);
                    if (cacheResponse.Count() > 0)
                    {
                        jobWithoutDictionary.AddRange(cacheResponse);
                    }
                    else
                    {
                        List<JobSearchResult> temp = jobService.KetQuaTimKiem(key);

                        if (temp != null)
                        {
                            jobWithoutDictionary.AddRange(temp);
                            cacheService.SetJobResultCache(jobWithoutDictionaryCacheKey, temp);

                        }
                    }
                }

                jobWithoutDictionaryGroup.AddRange(jobService.GroupJobBySkill(jobWithoutDictionary));
                int skipRecords = pageIndex * JobService.PAGE_SIZE;
                jobWithoutDictionaryGroup = jobWithoutDictionaryGroup.Skip(skipRecords).Take(JobService.PAGE_SIZE).ToList();
                if (jobWithoutDictionaryGroup.Count() >= JobService.PAGE_SIZE)
                {
                    var results = new
                    {
                        success = true,
                        data = new
                        {
                            Data = new
                            {
                                jobsSearchResult = jobWithoutDictionaryGroup,
                                jobsInRelatedSkill = new List<JobSearchResult>(),
                                jobsAfterNormalize = new List<JobSearchResult>(),
                            },
                            PageIndex = pageIndex,
                            skipRelatedRecords = skipRelatedRecords,
                            skipNormalizeRecords = skipNormalizeRecords,
                            TotalRecord = JobService.PAGE_SIZE
                        }
                    };
                    return Ok(results);
                }

                List<JobSearchResult> jobsOfRelatedSkill = new List<JobSearchResult>();
                List<JobGroupBySkill> jobsOfRelatedSkillGroup = new List<JobGroupBySkill>();

                foreach (string key in parseSearchKey)
                {
                    if (jobService.CheckSkill(key))
                    {

                        string jobsOfRelatedSkillCacheKey = $"{key}_jobsOfRelatedSkill";
                        List<JobSearchResult> cacheResponse = cacheService.GetJobResultCache(jobsOfRelatedSkillCacheKey);
                        if (cacheResponse.Count() > 0)
                        {
                            jobsOfRelatedSkill.AddRange(cacheResponse);
                        }

                        else
                        {
                            List<JobSearchResult> temp = jobService.FindJobsOfRelatedSkill(key);
                            if (temp != null)
                            {
                                jobsOfRelatedSkill.AddRange(temp);
                                cacheService.SetJobResultCache(jobsOfRelatedSkillCacheKey, temp);

                            }
                        }
                    }
                }

                jobsOfRelatedSkillGroup.AddRange(jobService.GroupJobBySkill(jobsOfRelatedSkill));
                jobsOfRelatedSkillGroup = jobsOfRelatedSkillGroup.Skip(skipRelatedRecords).Take(JobService.PAGE_SIZE - jobWithoutDictionaryGroup.Count()).ToList();


                if (jobsOfRelatedSkillGroup.Count() >= JobService.PAGE_SIZE)
                {
                    var results = new
                    {
                        success = true,
                        data = new
                        {
                            Data = new
                            {
                                jobsSearchResult = jobWithoutDictionaryGroup,
                                jobsInRelatedSkill = jobsOfRelatedSkillGroup,
                                jobsAfterNormalize = new List<JobSearchResult>(),
                            },
                            PageIndex = pageIndex,
                            skipRelatedRecords = skipRelatedRecords + jobsOfRelatedSkillGroup.Count(),
                            skipNormalizeRecords = skipNormalizeRecords,
                            TotalRecord = JobService.PAGE_SIZE
                        }
                    };
                    return Ok(results);
                }

                totalJobFound = jobWithoutDictionaryGroup.Count() + jobsOfRelatedSkillGroup.Count();

                List<JobSearchResult> jobsAfterNormalize = new List<JobSearchResult>();
                List<JobGroupBySkill> jobsAfterNormalizeGroup = new List<JobGroupBySkill>();

                foreach (string key in parseSearchKey)
                {
                    string mayCorrectSkill = levenshteinService.calcDictDistance(key, 1)[0];
                    if (StringUtils.RemoveVietnameseUnicode(mayCorrectSkill) == StringUtils.RemoveVietnameseUnicode(key))
                    {
                        continue;
                    }

                    string jobsAfterNormalizeCacheKey = $"{key}_jobsAfterNormalize";
                    List<JobSearchResult> cacheResponse = cacheService.GetJobResultCache(jobsAfterNormalizeCacheKey);
                    if (cacheResponse.Count() > 0)
                    {
                        jobsAfterNormalize.AddRange(cacheResponse);
                    }

                    else
                    {
                        List<JobSearchResult> temp = jobService.KetQuaTimKiem(mayCorrectSkill);

                        if (temp != null)
                        {
                            jobsAfterNormalize.AddRange(temp);
                            cacheService.SetJobResultCache(jobsAfterNormalizeCacheKey, temp);
                        }

                    }

                    string jobsOfRelatedNormalizeSkillCacheKey = $"{key}_jobsOfRelatedNormalizeSkill";
                    cacheResponse = cacheService.GetJobResultCache(jobsOfRelatedNormalizeSkillCacheKey);
                    if (cacheResponse.Count() > 0)
                    {
                        jobsAfterNormalize.AddRange(cacheResponse);
                    }
                    else
                    {
                        List<JobSearchResult> jobsOfRelatedNormalizeSkill = jobService.FindJobsOfRelatedSkill(mayCorrectSkill);
                        if (jobsOfRelatedNormalizeSkill != null)
                        {
                            jobsAfterNormalize.AddRange(jobsOfRelatedNormalizeSkill);

                            cacheService.SetJobResultCache(jobsOfRelatedNormalizeSkillCacheKey, jobsOfRelatedNormalizeSkill);
                        }
                    }
                }

                jobsAfterNormalizeGroup.AddRange(jobService.GroupJobBySkill(jobsAfterNormalize));
                jobsAfterNormalizeGroup = jobsAfterNormalizeGroup.Skip(skipNormalizeRecords).Take(JobService.PAGE_SIZE - totalJobFound).ToList();
                totalJobFound += jobsAfterNormalizeGroup.Count();
                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        Data = new
                        {
                            jobsSearchResult = jobWithoutDictionaryGroup,
                            jobsInRelatedSkill = jobsOfRelatedSkillGroup,
                            jobsAfterNormalize = jobsAfterNormalizeGroup,
                        },
                        PageIndex = pageIndex,
                        skipRelatedRecords = skipRelatedRecords + jobsOfRelatedSkillGroup.Count(),
                        skipNormalizeRecords = skipNormalizeRecords + jobsAfterNormalizeGroup.Count(),
                        TotalRecord = totalJobFound
                    }
                });

            }
            catch (NullReferenceException e)
            {
                return NotFound(new
                {
                    success = false,
                    error = e.Message
                });
            }
        }

        [Route("description")]
        [HttpGet]
        public async Task<IActionResult> GetDesription([FromQuery] int ma_cv)
        {
            try
            {
                Job job = await jobRepository.Get(w => w.ma_cong_viec == ma_cv);

                if (job != null)
                {
                    return Ok(new
                    {
                        data = job.ma_cong_viec
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        data = "Job null"
                    });
                }


            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    error = "Get description fail error" + e.InnerException.Message
                });
            }
        }

        [HttpPost]
        [Route("request")]
        async public Task<IActionResult> RequestJob([FromBody] RequestJob job, [FromHeader] string authorization)
        {
            Account org = await jWTService.GetAccount(authorization);

            if (org == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    code = 401,
                    error = "Unauthorized"
                });
            }

            if (org.ma_dn == null)
            {
                return Forbid("Forbidden");
            }

            job.ma_cong_ty = org.ma_dn;

            RequestJob requestJob = await requestJobRepository.Create(job);
            if (requestJob == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(new
            {
                success = true
            });
        }

        [HttpPost]
        async public Task<IActionResult> AddJob([FromBody] AddJob job, int cpId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (job == null)
            {
                return BadRequest(new
                {
                    success = false,
                    error = "Body can not be null"
                });
            }

            try
            {

                Job createdJob = await jobRepository.Create(job.job);
                if (createdJob == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                DanhSachDN_Job temp = new DanhSachDN_Job { ma_cong_viec = createdJob.ma_cong_viec, ma_doanh_nghiep = cpId, ngay_dang = DateTime.Now, trang_thai = false };

                DanhSachDN_Job createdDN_Job = await danhSachDNJobRepository.Create(temp);

                if (createdDN_Job == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }


                bool createdJobSkills = await jobService.CapNhatJobSkill(job.skills, createdJob.ma_cong_viec);
                if (createdJobSkills == false)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                queue.Enqueue(job);

                return Ok(new
                {
                    success = true,
                });

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("detail")]
        [HttpGet]
        public IActionResult GetDetail([FromQuery] int ma_dn, int ma_cv)
        {
            try
            {
                List<JobSearchDetail> result = jobService.GetDetailJob(ma_dn, ma_cv);

                if (result.Count() != 0)
                {

                    return Ok(new
                    {
                        success = "success",
                        data = result
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        code = 404
                    });
                }

            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    error = "Get detail fail Error" + e.InnerException.Message
                });
            }
        }
    }
}