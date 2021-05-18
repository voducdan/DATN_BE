using DATN.DAL.Context;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DAL.Services
{
    public class AccountService 
    {
        private readonly IHttpContextAccessor _httpContext;

        public AccountService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

       

        //public string GetUserId()
        //{
        //    return _httpContext.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier);
        //}

        public bool IsAuthenticated()
        {
            return _httpContext.HttpContext.User.Identity.IsAuthenticated;
        }

    }
}
