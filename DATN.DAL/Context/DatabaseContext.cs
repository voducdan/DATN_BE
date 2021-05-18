using DATN.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATN.DAL.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DanhSachDN_Job>().HasKey(e => new {e.ma_cong_viec, e.ma_doanh_nghiep });
            modelBuilder.Entity<DanhSachJob_Skill>().HasKey(e => new { e.ma_skill, e.ma_cong_viec });
            modelBuilder.Entity<DanhSachMyJob>().HasKey(e => new { e.ma_kh, e.ma_cong_viec });
        }

        public DbSet<Job> job { get; set; }
        public DbSet<DoanhNghiep> doanh_nghiep { get; set; }
        public DbSet<ThanhPho> thanh_pho { get; set; }
        public DbSet<DiaChi> dia_chi { get; set; }

        public DbSet<Skill> skill { get; set; }
        public DbSet<KhachHang> khach_hang { get; set; }
        public DbSet<Account> account { get; set; }
        public DbSet<BoTuDien> bo_tu_dien { get; set; }
        public DbSet<DanhSachJob_Skill> danh_sach_skill { get; set; }
        public DbSet<DanhSachDN_Job> danh_sach_dang_job { get; set; }
        public DbSet<DanhSachMyJob> danh_sach_my_job { get; set; }
        public DbSet<RequestJob> temp_job { get; set; }
    }
}
