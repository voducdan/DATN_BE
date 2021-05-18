using DATN.DAL.Context;
using DATN.DAL.Models;
using DATN.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using DATN.DAL.Repository;
using MoreLinq;

namespace DATN.DAL.Services
{
    public class JobService : BaseService
    {

        public static int PAGE_SIZE = 20;


        private readonly DatabaseContext dbContext;
        private BoTuDienRepository boTuDienRepository;
        private SkillRepository skillRepository;
        private DanhSachJobSkillRepository danhSachJobSkillRepository;
        public JobService(DatabaseContext context, BoTuDienRepository boTuDienRepository, SkillRepository skillRepository, DanhSachJobSkillRepository danhSachJobSkillRepository) : base(context)
        {
            this.dbContext = context;
            this.boTuDienRepository = boTuDienRepository;
            this.skillRepository = skillRepository;
            this.danhSachJobSkillRepository = danhSachJobSkillRepository;
        }

        public Job Get(string q)
        {
            return context.job.FirstOrDefault(w => w.ten_cong_viec.Contains(q));
        }


        public List<string> GetSuggesstion_JobTiltle(string q)
        {
            var jobs = new List<string>();

            try
            {
                var data = (from p in context.job
                            where p.ten_cong_viec != null
                            select new Job { ten_cong_viec = p.ten_cong_viec }
                            ).Distinct().ToList();

                foreach (Job item in data)
                {
                    if ((jobs.Count() < 5) && (StringUtils.RemoveVietnameseUnicode(item.ten_cong_viec).StartsWith(StringUtils.RemoveVietnameseUnicode(q)) == true))

                        jobs.Add(item.ten_cong_viec);
                }
                jobs.Sort();

                return jobs;

            }
            catch (Exception)
            {

                return null;

            }
        }

        public List<JobSearchDetail> GetDetailJob(int ma_dn, int ma_cv)
        {
            try
            {
                List<JobSearchDetail> jobDetail = (context.danh_sach_skill
                                                   .Join(context.skill, d => d.ma_skill, s => s.ma_skill, (d, s) => new
                                                   {
                                                       d.ma_cong_viec,
                                                       s.ten_skill
                                                   })
                                                   .Join(context.job, d => d.ma_cong_viec, j => j.ma_cong_viec, (d, j) => new
                                                   {
                                                       d.ten_skill,
                                                       j.ma_cong_viec,
                                                       j.mo_ta_cong_viec,
                                                       j.ten_cong_viec,
                                                       j.luong_toi_thieu,
                                                       j.luong_toi_da,
                                                       j.ngay_bat_dau,
                                                       j.ngay_ket_thuc,
                                                       j.don_vi_tien_te

                                                   })
                                                   .Join(dbContext.danh_sach_dang_job, j => j.ma_cong_viec, dnj => dnj.ma_cong_viec, (j, dnj) => new
                                                   {
                                                       j.ten_skill,
                                                       j.ma_cong_viec,
                                                       j.ten_cong_viec,
                                                       j.mo_ta_cong_viec,
                                                       j.luong_toi_da,
                                                       j.luong_toi_thieu,
                                                       j.ngay_bat_dau,
                                                       j.ngay_ket_thuc,
                                                       j.don_vi_tien_te,
                                                       dnj.ma_doanh_nghiep
                                                   })
                                                   .Join(dbContext.doanh_nghiep, x => x.ma_doanh_nghiep, dn => dn.ma_doanh_nghiep, (x, dn) => new
                                                   {
                                                       dn.ma_doanh_nghiep,
                                                       dn.ten_doanh_nghiep,
                                                       dn.mo_ta,
                                                       dn.sdt,
                                                       dn.email,

                                                       x.ten_cong_viec,
                                                       x.mo_ta_cong_viec,
                                                       x.luong_toi_da,
                                                       x.luong_toi_thieu,
                                                       x.ngay_bat_dau,
                                                       x.ngay_ket_thuc,
                                                       x.ten_skill,
                                                       x.ma_cong_viec,
                                                       x.don_vi_tien_te
                                                   })



                                                   .GroupJoin(dbContext.dia_chi, y => y.ma_doanh_nghiep, dc => dc.ma_doanh_nghiep, (y, dc) => new
                                                   {
                                                       y,
                                                       dc
                                                   })

                                                  .SelectMany(x => x.dc.DefaultIfEmpty(), (x, dc) => new JobSearchDetail
                                                  {
                                                      ma_cong_viec = x.y.ma_cong_viec,
                                                      ten_cong_viec = x.y.ten_cong_viec,
                                                      ma_doanh_nghiep = x.y.ma_doanh_nghiep,
                                                      ten_cong_ty = x.y.ten_doanh_nghiep,
                                                      mo_ta_cong_ty = x.y.mo_ta,
                                                      email = x.y.email,
                                                      sdt = x.y.sdt,
                                                      dia_chi = dc.mo_ta,
                                                      luong_toi_thieu = x.y.luong_toi_thieu,
                                                      luong_toi_da = x.y.luong_toi_da,
                                                      ngay_bat_dau = x.y.ngay_bat_dau,
                                                      ngay_ket_thuc = x.y.ngay_ket_thuc,
                                                      ten_skill = x.y.ten_skill,
                                                      mo_ta_cong_viec = x.y.mo_ta_cong_viec,
                                                      don_vi_tien_te = x.y.don_vi_tien_te

                                                  })
                                                  .Where(w => w.ma_cong_viec == ma_cv && w.ma_doanh_nghiep == ma_dn)
                                                  .Select(job => job)).Distinct().ToList();
                return jobDetail;

            }
            catch
            {
                return null;

            }
        }

        public static IEnumerable<List<T>> Combinations<T>(IEnumerable<T> source)
        {
            if (null == source)
                throw new ArgumentNullException(nameof(source));

            List<T> data = source.ToList();

            return Enumerable
              .Range(0, 1 << (data.Count()))
              .Select(index => data
                 .Where((v, i) => (index & (1 << i)) != 0)
                 .ToList());
        }

        public List<string> tokenize(List<string> stuff, List<string> skillName)
        {
            if (stuff.Count() == 0)
            {
                return null;
            }
            List<string> selectedPhase = new List<string>();
            var subset = Combinations(stuff);
            foreach (var i in subset)
            {
                string splited = string.Join(" ", i);
                if (CheckSkill(splited))
                {
                    selectedPhase.Add(splited);
                    foreach (string item in i)
                    {
                        stuff.Remove(item);
                    }

                }
            }
            selectedPhase.AddRange(stuff);
            return selectedPhase;
        }

        public List<string> ParseSearchey(string searchKey)
        {
            List<string> skillName = dbContext.skill.Select(s => s.ten_skill_1).ToList();
            List<string> searchKeyList = searchKey.Split(" ").ToList();
            return tokenize(searchKeyList, skillName);
        }

        public List<JobGroupBySkill> GroupJobBySkill(List<JobSearchResult> list)
        {
            List<JobGroupBySkill> data = new List<JobGroupBySkill>();
            var grouped = list.OrderBy(x => x.ma_cong_viec)
                   .GroupBy(x => x.ma_cong_viec);
            foreach (var iGrouped in grouped)
            {
                JobSearchResult first = iGrouped.First();
                JobGroupBySkill temp = new JobGroupBySkill
                {
                    ma_cong_viec = iGrouped.Key,
                    ten_cong_viec = first.ten_cong_viec,
                    ma_doanh_nghiep = first.ma_doanh_nghiep,
                    ten_cong_ty = first.ten_cong_ty,
                    dia_chi = first.dia_chi,
                    don_vi_tien_te = first.don_vi_tien_te,
                    luong_toi_thieu = first.luong_toi_thieu,
                    luong_toi_da = first.luong_toi_da,
                    ngay_bat_dau = first.ngay_bat_dau,
                    skills = new List<string>(),
                    mo_ta_cong_viec = first.mo_ta_cong_viec,
                };
                temp.skills.AddRange(iGrouped.Select(x => x.ten_skill).ToList());

                data.Add(temp);
            }
            return data;
        }

        public List<JobSearchResult> KetQuaTimKiem(string searchKey)
        {
            try
            {
                string removeUnicodeSkill = StringUtils.RemoveVietnameseUnicode(searchKey);

                IEnumerable<int> skills = dbContext.skill.Where(s => s.ten_skill_1.Equals(removeUnicodeSkill)).Select(s => s.ma_skill);

                IEnumerable<int> jobs = dbContext.job.Where(j => j.ten_cong_viec_1.Contains(removeUnicodeSkill)).Select(s => s.ma_cong_viec);

                IQueryable<DanhSachJob_Skill> jobMapCondition = dbContext.danh_sach_skill.Where(dss => skills.Contains(dss.ma_skill) || jobs.Contains(dss.ma_cong_viec));

                IEnumerable<JobSearchResult> results =
                   (jobMapCondition
                   .Join(dbContext.skill, d => d.ma_skill, s => s.ma_skill, (x, s) => new
                   {
                       x.ma_cong_viec,
                       s.ten_skill
                   })
                   .Join(dbContext.job, d => d.ma_cong_viec, j => j.ma_cong_viec, (d, j) => new
                   {
                       d.ten_skill,
                       j.ma_cong_viec,
                       j.ten_cong_viec,
                       j.luong_toi_da,
                       j.luong_toi_thieu,
                       j.ngay_bat_dau,
                   })
                   .Join(dbContext.danh_sach_dang_job, j => j.ma_cong_viec, dnj => dnj.ma_cong_viec, (j, dnj) => new
                   {
                       j.ten_skill,
                       j.ma_cong_viec,
                       j.ten_cong_viec,
                       j.luong_toi_da,
                       j.luong_toi_thieu,
                       j.ngay_bat_dau,
                       dnj.ma_doanh_nghiep
                   })
                   .Join(dbContext.doanh_nghiep, x => x.ma_doanh_nghiep, dn => dn.ma_doanh_nghiep, (x, dn) => new
                   {
                       dn.ma_doanh_nghiep,
                       dn.ten_doanh_nghiep,
                       x.ten_cong_viec,
                       x.luong_toi_da,
                       x.luong_toi_thieu,
                       x.ngay_bat_dau,
                       x.ten_skill,
                       x.ma_cong_viec,
                   })
                   .GroupJoin(dbContext.dia_chi, y => y.ma_doanh_nghiep, dc => dc.ma_doanh_nghiep, (y, dc) => new
                   {
                       y,
                       dc
                   })
                   .SelectMany(x => x.dc.DefaultIfEmpty(), (x, dc) => new JobSearchResult
                   {
                       ma_cong_viec = x.y.ma_cong_viec,
                       ten_cong_viec = x.y.ten_cong_viec,
                       ma_doanh_nghiep = x.y.ma_doanh_nghiep,
                       ten_cong_ty = x.y.ten_doanh_nghiep,
                       luong_toi_da = x.y.luong_toi_da,
                       luong_toi_thieu = x.y.luong_toi_thieu,
                       ngay_bat_dau = x.y.ngay_bat_dau,
                       dia_chi = dc.mo_ta,
                       ten_skill = x.y.ten_skill,
                   })
                   .AsParallel()
                   .DistinctBy(j => new { j.ma_cong_viec, j.ten_skill })
                   .Select(job => job));
                return results.Count() > 0 ? results.ToList() : null;
            }
            catch
            {
                return null;
            }
        }

        public List<string> FindRelatedSkill(string searchKey)
        {
            try
            {
                searchKey = StringUtils.RemoveVietnameseUnicode(searchKey);
                int searchKeyId = dbContext.skill.Where(s => s.ten_skill_1.Equals(searchKey)).Select(s => s.ma_skill).First();

                List<string> relatedSkills = (context.bo_tu_dien
                                            .Where(btd => btd.ma_tu == searchKeyId)
                                            .Join(context.skill, btd => btd.tu_lien_quan, s => s.ma_skill, (btd, s) => new
                                            {
                                                s.ten_skill,
                                                btd.do_lien_quan
                                            }))
                                            .Select(s => s.ten_skill)
                                            .ToList();
                return relatedSkills.Count() > 0 ? relatedSkills : null;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public List<JobSearchResult> FindJobsOfRelatedSkill(string searchKey)
        {
            List<JobSearchResult> relatedSkillJobs = new List<JobSearchResult>();

            List<string> relatedSkills = FindRelatedSkill(searchKey);
            if (relatedSkills == null)
            {
                return null;
            }

            foreach (string skill in relatedSkills)
            {
                List<JobSearchResult> job = KetQuaTimKiem(skill);
                if (job != null)
                {
                    relatedSkillJobs.AddRange(job);
                }
            };

            return relatedSkillJobs.Count() > 0 ? relatedSkillJobs : null;
        }

        public async Task CalcRelatenessScore(string s1, string s2)
        {
            try
            {
                int s1Cnt = context.skill.Where(s => s.ten_skill == s1).Count();
                int s2Cnt = context.skill.Where(s => s.ten_skill == s2).Count();
                int dataSize = context.job.Count();

                int s1Id = context.skill.Where(s => s.ten_skill == s1).Select(s => s.ma_skill).First();
                int s2Id = context.skill.Where(s => s.ten_skill == s2).Select(s => s.ma_skill).First();


                string query = @$"
                    select *
                    from 
                    (
	                    select array_agg(ma_skill) aa
	                    from danh_sach_skill dss 
	                    group by ma_cong_viec 
                    ) a
                    where (select ma_skill from skill where ten_skill = '{s1}') = any(aa) and (select ma_skill from skill where ten_skill = '{s2}') = any(aa)
                ";

                int s1S2Cnt = context.danh_sach_skill.FromSqlRaw(query).Count();

                float score = ScoreCalculator.findRelatedScore(s1Cnt, s2Cnt, s1S2Cnt, dataSize);

                BoTuDien boTuDien = new BoTuDien
                {
                    ma_tu = s1Id,
                    tu_lien_quan = s2Id,
                    loai_tu = "Skill",
                    do_lien_quan = score
                };

                await boTuDienRepository.Update(boTuDien);

            }

            catch
            {

            }
        }

        public async Task<bool> CapNhatJobSkill(List<string> skills, int jobId)
        {
            try
            {
                List<string> allSkills = dbContext.skill.Select(s => s.ten_skill).ToList();

                foreach (string skill in skills)
                {
                    try
                    {
                        int skillId = dbContext.skill.Where(s => s.ten_skill == skill).Select(s => s.ma_skill).First();
                        DanhSachJob_Skill danhSachJob_Skill = new DanhSachJob_Skill { ma_skill = skillId, ma_cong_viec = jobId };
                        await danhSachJobSkillRepository.Create(danhSachJob_Skill);
                    }
                    catch (Exception e)
                    {
                        Skill newSkill = new Skill { ten_skill = skill, ghi_chu = "" };
                        Skill createdSkill = await skillRepository.Create(newSkill);
                        DanhSachJob_Skill danhSachJob_Skill = new DanhSachJob_Skill { ma_skill = createdSkill.ma_skill, ma_cong_viec = jobId };
                        await danhSachJobSkillRepository.Create(danhSachJob_Skill);
                    }
                }
                return true;
            }

            catch
            {
                return false;
            }
        }

        public async Task CapNhatBoTuDien(List<string> skills)
        {
            List<string> allSkills = skillRepository.GetAll().Select(s => s.ten_skill).ToList();

            foreach (string skill in skills)
            {
                foreach (string x in allSkills)
                {
                    await CalcRelatenessScore(skill, x);
                }
            }
        }

        public bool CheckSkill(string skill)
        {
            skill = StringUtils.RemoveVietnameseUnicode(skill);
            List<string> foundSkill = dbContext.skill.Where(s => s.ten_skill_1.Equals(skill)).Select(s => s.ten_skill).ToList();

            return foundSkill.Count() > 0 ? true : false;
        }
        public async Task<List<JobFilterResult>> GetFilterResults(DateTime? date, int? pageIndex, double? salary, string? city, string? skill)
        {
            try
            {
                List<JobFilterResult> jobs =
                   (dbContext.danh_sach_skill
                   .Join(dbContext.skill, d => d.ma_skill, s => s.ma_skill, (d, s) => new
                   {
                       d.ma_cong_viec,
                       s.ten_skill
                   })
                   .Join(dbContext.job, d => d.ma_cong_viec, j => j.ma_cong_viec, (d, j) => new
                   {
                       d.ten_skill,
                       j.mo_ta_cong_viec,
                       j.ma_cong_viec,
                       j.ten_cong_viec,
                       j.luong_toi_da,
                       j.luong_toi_thieu,
                       j.ngay_bat_dau,
                       j.ngay_ket_thuc
                   })
                   .Join(dbContext.danh_sach_dang_job, j => j.ma_cong_viec, dnj => dnj.ma_cong_viec, (j, dnj) => new
                   {
                       j.ten_skill,
                       j.ma_cong_viec,
                       j.mo_ta_cong_viec,
                       j.ten_cong_viec,
                       j.luong_toi_da,
                       j.luong_toi_thieu,
                       j.ngay_bat_dau,
                       j.ngay_ket_thuc,
                       dnj.ma_doanh_nghiep
                   })
                   .Join(dbContext.doanh_nghiep, x => x.ma_doanh_nghiep, dn => dn.ma_doanh_nghiep, (x, dn) => new
                   {
                       dn.ma_doanh_nghiep,
                       dn.ten_doanh_nghiep,
                       x.ten_cong_viec,
                       x.luong_toi_da,
                       x.luong_toi_thieu,
                       x.ngay_bat_dau,
                       x.ngay_ket_thuc,
                       x.ten_skill,
                       x.ma_cong_viec,
                       x.mo_ta_cong_viec
                   })
                   .GroupJoin(dbContext.dia_chi, y => y.ma_doanh_nghiep, dc => dc.ma_doanh_nghiep, (y, dc) => new
                   {
                       y,
                       dc
                   })
                   .SelectMany(x => x.dc.DefaultIfEmpty(), (x, dc) => new
                   {
                       x.y.ma_cong_viec,
                       x.y.ten_cong_viec,
                       x.y.ten_doanh_nghiep,
                       x.y.luong_toi_da,
                       x.y.luong_toi_thieu,
                       x.y.ngay_bat_dau,
                       dc.mo_ta,
                       x.y.ten_skill,
                       x.y.ma_doanh_nghiep,
                       x.y.ngay_ket_thuc,
                       dc.ma_thanh_pho
                   })
                   .Join(dbContext.thanh_pho, z => z.ma_thanh_pho, tp => tp.ma_thanh_pho, (z, tp) => new JobFilterResult
                   {

                       ten_cong_ty = z.ten_doanh_nghiep,
                       ten_cong_viec = z.ten_cong_viec,
                       luong_toi_da = z.luong_toi_da,
                       luong_toi_thieu = z.luong_toi_thieu,
                       ngay_bat_dau = z.ngay_bat_dau,
                       ngay_ket_thuc = z.ngay_ket_thuc,
                       ten_skill = z.ten_skill,
                       ma_cong_viec = z.ma_cong_viec,
                       ma_thanh_pho = z.ma_thanh_pho,
                       ten_thanh_pho = tp.ten_thanh_pho,
                       dia_chi = z.mo_ta,
                       ma_doanh_nghiep = z.ma_doanh_nghiep
                   })
                   .Where(z => ((z.ngay_bat_dau <= date || date == null) && (z.ngay_ket_thuc >= date || date == null) && (z.luong_toi_thieu <= salary || salary == null) && (z.ten_thanh_pho == city || city == null) && (z.ten_skill == skill || skill == null)))
                   .Skip(pageIndex.Value * PAGE_SIZE).Take(PAGE_SIZE)
                   .Select(job => job)).ToList();
                return jobs.Count() > 0 ? jobs : null;

            }
            catch
            {
                return null;
            }
        }
    }
}
