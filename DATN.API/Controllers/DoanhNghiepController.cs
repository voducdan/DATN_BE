using DATN.API.Commands;
using DATN.DAL.Models;
using DATN.DAL.Repository;
using DATN.DAL.Services;
using DATN.Infrastructure.Helpers;
using DATN.Infrastructure.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DATN.API.Controllers
{
    [Route("api/v1/companies")]
    public class DoanhNghiepController : Controller
    {
        private DoanhNghiepRepository doanhNghiepRepository;
        private DoanhNghiepService doanhNghiepService;
        private AccountRepository accountRepository;
        private KhachHangRepository khachHangRepository;
        private ThanhPhoService thanhPhoService;
        private DiaChiRepository diaChiRepository;

        public DoanhNghiepController(DoanhNghiepRepository doanhNghiepRepository, DoanhNghiepService doanhNghiepService, AccountRepository accountRepository, KhachHangRepository khachHangRepository, ThanhPhoService thanhPhoService, DiaChiRepository diaChiRepository)
        {
            this.doanhNghiepRepository = doanhNghiepRepository;
            this.doanhNghiepService = doanhNghiepService;
            this.accountRepository = accountRepository;
            this.khachHangRepository = khachHangRepository;
            this.thanhPhoService = thanhPhoService;
            this.diaChiRepository = diaChiRepository;
        }


        // create new account of company
        [Route("sign-up")]
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCompanyCommand command)
        {
            try
            {
                if (command.password != command.password_confirm)
                {
                    return BadRequest(new
                    {
                        error = "The password comfirm is not the same as The password"
                    });
                }
                else
                {
                    // password Length of password is >= 8 and any character is number and any character is upper and case better to have a special then Symbol
                    var expectedPasswordPattern = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");

                    var isValidPassword = expectedPasswordPattern.IsMatch(command.password);


                    if (isValidPassword == true)
                    {
                        // Get account by email
                        Account account = new Account();
                        account = await accountRepository.Get(w => w.email == command.email);


                        if (account == null)
                        {
                            
                            KhachHang kh = new KhachHang();
                            
                            // Add new customer 
                            kh.ten_kh = command.ten_kh;
                            kh.sdt = command.sdt;
                            await khachHangRepository.Create(kh);

                            int ma=0;
                            // Add Company
                            DoanhNghiep dn = new DoanhNghiep();

                            
                            Account account_new = new Account();
                            account_new.ma_kh = kh.ma_kh;
                            DoanhNghiep ct = new DoanhNghiep();
                            ct = await doanhNghiepRepository.Get(w => w.ten_doanh_nghiep == command.ten_doanh_nghiep);
                            if (ct != null)
                            {
                                account_new.ma_dn = ct.ma_doanh_nghiep;
                                ma = ct.ma_doanh_nghiep;
                                dn.ma_doanh_nghiep = ct.ma_doanh_nghiep;
                            }
                            else
                            {
                                //await doanhNghiepService.Sort();
                                // Add company
                                dn.ten_doanh_nghiep = command.ten_doanh_nghiep;
                                dn.mo_ta = command.mo_ta;
                                dn.ma_doanh_nghiep =  doanhNghiepService.Count() + 1;

                                await doanhNghiepRepository.Create(dn); 

                                account_new.ma_dn = dn.ma_doanh_nghiep;
                                ma = dn.ma_doanh_nghiep;
                            }

                            // get ma_thanh_pho

                            var ma_tp = await thanhPhoService.GetId(command.ten_thanh_pho);

                            // Add Dia chi

                            DiaChi address = new DiaChi();

                            address.ma_doanh_nghiep = ma;
                            address.ma_thanh_pho = ma_tp;
                            address.mo_ta = command.dia_chi_cu_the;

                            await diaChiRepository.Create(address);

                            // Add account
                            account_new.email = command.email;
                            account_new.password = HashHelper.Hash(command.password);
                            account_new.rolecode = 2;
                            account_new.createtime = DateTime.Now;

                            try
                            {
                                await accountRepository.Create(account_new);

                                return Ok(new
                                {
                                    code = 200,
                                    success = "register is success"

                                });
                            }
                            catch (Exception e)
                            {
                                return BadRequest(new
                                {
                                    error = "Add account fail -  " + e.InnerException.Message
                                });
                            }

                        }
                        else
                        {
                            return BadRequest(new
                            {
                                code = 400,
                                error = "Email is exist"
                            });
                        }
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            code = 404,
                            error = "password Length of password is >= 8 and any character is number and any character is upper and case better to have a special then Symbol"
                        });
                    }
                }

            }
            catch(Exception e)
            {
                return BadRequest(new
                {
                    code =400,
                    error = "Create account of Company fail + " + e.InnerException.Message
                });
            }

        }


        // POST: api/v1/companies
        [HttpPost]
        async public Task<IActionResult> SearchByKeyWord([FromQuery] string searchKey, int pageIndex)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Expression<Func<DoanhNghiep, bool>> companyExp = j => j.ten_doanh_nghiep.Contains(searchKey);
                // Expression<Func<DanhSachJob_Skill,Skill,Job,bool>> jobInSkillExp = j=>j.ten_cong_viec.Contains(searchKey);
               

                ListResponse<DoanhNghiep> copanies = await this.doanhNghiepRepository.GetList(searchKey, pageIndex, companyExp);

                if (copanies == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        error = "Could not find any company"
                    });
                }
                return Ok(new
                {
                    success = true,
                    data = copanies
                }) ;
            }
            catch(Exception e)
            {
                return NotFound(new
                {
                    success = false,
                    error = "faile error: " + e.InnerException.Message
                    
                });
            }
        }


        // GET: api/v1/companies/5
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
                DoanhNghiep doanhNghiep = await doanhNghiepRepository.Get(w => w.ma_doanh_nghiep == id);
                if (doanhNghiep == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        error = "Company is not found"
                    });
                }
                return Ok(new
                {
                    success = true,
                    data = doanhNghiep
                });
            }
            catch(Exception e)
            {
                return NotFound(new
                {
                    success = false,
                    error = "Company is not found + " + e.InnerException.Message
                });
            }
        }
    }
}
