using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using DATN.DAL.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using DATN.API.Settings;
using System.Text;
using DATN.DAL.Services;
using DATN.DAL.Repository;
using DATN.API.Services;
using DATN.DAL.Models;

namespace DATN.API
{
    public class Startup
    {
        private const string AllowAllOriginsPolicy = "AllowAllOriginsPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //"AppConn": "Server=arjuna.db.elephantsql.com;Database=ppcoymqc;User ID=ppcoymqc;Password=nPNk-4ZwWFwJ3sIiZTYj981pW-Ijw0w3;Port=5432;"
            services.AddControllers();
            //services.AddDbContext<DatabaseContext>(opt =>
            //{
            //    opt.UseNpgsql(Configuration.GetConnectionString("AppConn"));
            //});
            services.AddEntityFrameworkNpgsql().AddDbContext<DatabaseContext>(opt =>
            opt.UseNpgsql(Configuration.GetConnectionString("AppConn")));

            services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });
            services.AddHostedService<BackgroundWorker>();
            services.AddSingleton<IBackgroundQueue<AddJob>, BackgroundQueue<AddJob>>();


            // Khai báo các service và các Repository đc dùng

            services.AddScoped<ICapNhatTuDien, CapNhatTuDienService>();


            services.AddScoped<AccountService, AccountService>();
            services.AddScoped<AccountRepository, AccountRepository>();

            services.AddScoped<KhachHangService, KhachHangService>();
            services.AddScoped<KhachHangRepository, KhachHangRepository>();


            services.AddScoped<JWTService, JWTService>();
            services.AddScoped<CacheService, CacheService>();


            services.AddScoped<JobService, JobService>();
            services.AddScoped<JobRepository, JobRepository>();

            services.AddScoped<DoanhNghiepRepository, DoanhNghiepRepository>();
            services.AddScoped<DoanhNghiepService, DoanhNghiepService>();

            services.AddScoped<ThanhPhoService, ThanhPhoService>();
            services.AddScoped<DiaChiRepository, DiaChiRepository>();

            services.AddScoped<LevenshteinService, LevenshteinService>();

            services.AddScoped<SkillService, SkillService>();
            services.AddScoped<SkillRepository, SkillRepository>();

            services.AddScoped<RequestJobRepository, RequestJobRepository>();




            services.AddScoped<BoTuDienRepository, BoTuDienRepository>();

            services.AddScoped<DanhSachJobSkillRepository, DanhSachJobSkillRepository>();


            services.AddScoped<DanhSachDNJobRepository, DanhSachDNJobRepository>();

            services.AddScoped<DanhSachMyJobRepository, DanhSachMyJobRepository>();

            services.AddHttpContextAccessor();

            //services.AddScoped<EmailService, EmailService>();




            ////configure strongly typed settings object
            var authSettingsSection = Configuration.GetSection("AuthSettings");
            services.Configure<AuthSettings>(authSettingsSection);

            var appSettings = authSettingsSection.Get<AuthSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.AuthSecret);


            services.Configure<SMTPConfigModel>(Configuration.GetSection("SMTPConfig"));
            services.AddCors(options =>
            {
                options.AddPolicy(AllowAllOriginsPolicy,
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();

                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CheckToken", policy =>
                {
                    policy.RequireAssertion(httpctx =>
                    {
                        if (true)
                        {
                            return true;
                        }
                    });
                });
            });

            services.AddMemoryCache();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(AllowAllOriginsPolicy);
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
