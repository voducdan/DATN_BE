using DATN.API.Commands;
using DATN.API.Services;
using DATN.API.Settings;
using DATN.DAL.Models;
using DATN.DAL.Repository;
using DATN.DAL.Services;
using DATN.Infrastructure.Helpers;
using DATN.Infrastructure.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DATN.API.Controllers
{
    [Route("api/v1/auths")]
    public class AuthController : Controller
    {
        // Khai bao cac service va repository su dung 
      
        private KhachHangRepository khachHangRepository;
        private JWTService jWTService;
        private AuthSettings authSettings;
        private AccountRepository accountRepository;
        
       

        public AuthController(KhachHangRepository khachHangRepository, JWTService jWTService, IOptions<AuthSettings> authSettings, AccountRepository accountRepository) : base()
        {
            this.khachHangRepository = khachHangRepository;
            this.jWTService = jWTService;
            this.authSettings = authSettings.Value;
            this.accountRepository = accountRepository;
            
        }

        // create new account
        [Route("sign-up")]
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCustomerCommand command)
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


                        if (account != null)
                        {
                            return BadRequest(new
                            {
                                code = 400,
                                error = "Email is exist"
                            });
                        }
                        else
                        {
                           
                            Account account_new = new Account();
                            KhachHang kh = new KhachHang();
                            // Add new customer 

                            kh.ten_kh = command.ten_kh;
                            kh.sdt = command.sdt;
                            await khachHangRepository.Create_v(kh);

                            // Add account
                            account_new.email = command.email;
                            account_new.password = HashHelper.Hash(command.password);
                            account_new.ma_kh = kh.ma_kh;
                            account_new.rolecode = 1;
                            account_new.createtime = DateTime.Now;

                            //try
                            //{
                                await accountRepository.Create_v(account_new);

                                return Ok(new
                                {
                                    code = 200,
                                    success = "register is success"

                                });
                            //}
                            //catch (Exception e)
                            //{
                            //    return BadRequest(new
                            //    {
                            //        error = "Add account fail -  " + e.InnerException.Message
                            //    });
                            //}
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
            catch (NullReferenceException e)
            {

                return BadRequest(new
                {
                    code = 400,
                    error = e.InnerException.Message
                });
            }
        }

        // send email confirm
        //[Route("send")]
        //[HttpGet]
        //public async Task<IActionResult> SendEmail()
        //{
        //    try
        //    {
        //        UserEmailOptions options = new UserEmailOptions
        //        {
        //            ToEmails = new List<string>() { "ngtranhtuan99@gmail.com" },
        //            PlaceHolders = new List<KeyValuePair<string, string>>()
        //            {
        //                new KeyValuePair<string, string>("{{UserName}}", "ptson")
        //            }
        //        };

        //        await emailService.SendTestEmail(options);
        //        return Ok(new
        //        {
        //            code = 200
        //        });
        //    }
        //    catch (NullReferenceException e)
        //    {
        //        return BadRequest(new
        //        {
        //            error = "Fail" + e.InnerException.Message
        //        });
        //    }

        //}


        //// GET: api/v1/auths/email
        //public async Task<IActionResult> Get(string email)
        //{
        //    KhachHang kh = new KhachHang();
        //    kh = await khachHangRepository.Get(w => w.email == email);
        //    try
        //    {
        //        if (kh != null)
        //        {
        //            return Ok(new
        //            {
        //                code = 200,
        //                data = kh,
        //                success = "Get success"
        //            });
        //        }
        //        else
        //        {
        //            return BadRequest(new
        //            {
        //                code = 400,
        //                error = "account is not exist"
        //            });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(new
        //        {
        //            error = e.Message
        //        });

        //    }

        //}




        private string ProcessLogin(Account account)
        {
            // Time died
            var accessTokenExpiration = DateTime.Now.AddMinutes(ExpiredTime.AccessTokenExpirationTime);
            // ceate accessToken
            var accessToken = jWTService.GenerateAccessToken(authSettings.AuthSecret, account, accessTokenExpiration);

            return accessToken;
        }


        // POST: api/v1/auths/login
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            try
            {
                var account = await accountRepository.Get(w => w.email == command.email);

                if (account != null && HashHelper.Verify(command.password, account.password))
                {

                    return Ok(new
                    {
                        code = 200,
                        success = "Login success",
                        // token
                        data = ProcessLogin(account)
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        code = 203,
                        error = "Account does not exist"
                    });
                }


            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    code = 400,
                    error = e.InnerException.Message
                });
            }

        }

        // POST: api/v1/auths/logintoken
        [Route("logintoken")]
        [HttpPost]
        public async Task<IActionResult> LoginToken([FromHeader] string authorization)
        {
            try
            {
                var account = await jWTService.GetAccount(authorization);

                if (account != null)
                {
                    return Ok(new
                    {
                        code = 200,
                        ma_kh = account.ma_kh,
                        ma_dn = account.ma_dn,
                        success = "Login token success"
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        code = 400,
                        error = "Account is not exist"
                    });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    code = 400,
                    error = "Login token fail" + e.InnerException.Message
                });
            }
        }

        //DELETE: api/v1/auths?email=
        [HttpDelete]
        public async Task<IActionResult> DeleteAccount(string email)
        {
            Account account = new Account();
            account = await accountRepository.Get(w => w.email == email);
            try
            {
                if (account != null)
                {
                    await accountRepository.Delete(account);
                    return Ok(new
                    {
                        code = 200,
                        success = "Delete success"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        code = 400,
                        error = "account is not exits "
                    });
                }


            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    code = 400,
                    error = "Delete account fail: error " + e.InnerException.Message
                });
            }

        }

        //PUT: api/v1/auths
        [HttpPut]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordCommand command)
        {
            try
            {
                Account account = new Account();
                account = await accountRepository.Get(w => w.email == command.email);
                if ((account != null) && (HashHelper.Verify(command.password_old, account.password)))
                {
                    if (command.password_new != command.password_confirm)
                    {
                        return BadRequest(new
                        {
                            error = "the password confirm is not the same as password new"
                        });
                    }
                    else
                    {
                        // password Length of password is >= 8 and any character is number and any character is upper and case better to have a special then Symbol
                        var expectedPasswordPattern = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
                        var isValidPassword = expectedPasswordPattern.IsMatch(command.password_new);

                        if (command.password_old == command.password_new)
                        {
                            return BadRequest(new
                            {
                                error = "The new password is the same as the old password "
                            });
                        }
                        else
                        {
                            if (isValidPassword == true)
                            {
                                account.password = HashHelper.Hash(command.password_new);
                                account.createtime = DateTime.Now;
                                await accountRepository.Update(account);

                                return Ok(new
                                {
                                    code = 200,
                                    success = "Update password success"
                                });
                            }
                            else
                            {
                                return BadRequest(new
                                {
                                    error = "password Length of password is >= 8 and any character is number and any character is upper and case better to have a special then Symbol  "
                                });
                            }
                        }
                    }
                }
                else
                {
                    return BadRequest(new
                    {
                        error = "The password old or Email is incorrect"
                    });
                }


            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    code = 400,
                    error = "Update Password of account fail: error " + e.InnerException.Message
                });
            }
        }

        // GET: api/v1/auths/confirm-email
        //[HttpGet("confirm-email")]
        //public async Task<IActionResult> ConfirmEmail(string uid, string token, string email)
        //{
        //    EmailConfirmModel model = new EmailConfirmModel
        //    {
        //        Email = email
        //    };

        //    if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
        //    {
        //        token = token.Replace(' ', '+');
        //        var result = await _accountRepository.ConfirmEmailAsync(uid, token);
        //        if (result.Succeeded)
        //        {
        //            model.EmailVerified = true;
        //        }
        //    }

        //    return View(model);
        //}

        // POST: api/v1/auths/confirm-email
        //[HttpPost("confirm-email")]
        //public async Task<IActionResult> ConfirmEmail(EmailConfirmModel model)
        //{
        //    var user = await _accountRepository.GetUserByEmailAsync(model.Email);
        //    if (user != null)
        //    {
        //        if (user.EmailConfirmed)
        //        {
        //            model.EmailVerified = true;
        //            return View(model);
        //        }

        //        await _accountRepository.GenerateEmailConfirmationTokenAsync(user);
        //        model.EmailSent = true;
        //        ModelState.Clear();
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Something went wrong.");
        //    }
        //    return View(model);
        //}
    }
}