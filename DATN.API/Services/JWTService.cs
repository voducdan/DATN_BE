using DATN.API.Settings;
using DATN.DAL.Models;
using DATN.DAL.Repository;
using DATN.DAL.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DATN.API.Services
{
    public class JWTService
    {
        private AccountRepository accountRepository;
       

        public JWTService(AccountRepository accountRepository )
        {
            this.accountRepository = accountRepository;         
        }

        public string GenerateAccessToken(string authSecret, Account account, DateTime accessTokenExpiration)
        {

            var claims = new[] {
                new Claim(Key.JWTUserIdKey, account.id.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(claims: claims,
                                              expires: accessTokenExpiration,
                                              signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Account> GetAccount(string token)
        {            
            try
            {
                Account account = new Account();

                var validator = new JwtSecurityTokenHandler();
                var jwtToken = validator.ReadJwtToken(token);
                var userClaim = jwtToken.Claims.FirstOrDefault(ww => ww.Type == Key.JWTUserIdKey);
                if (userClaim != null)
                {
                    int accountId = Convert.ToInt32(userClaim.Value);
                    account = await accountRepository.Get(w=>w.id == accountId);
                    
                }
                return account;
            }
            catch 
            {
                return null;
            }
        }
    }
}
