using DATN.DAL.Context;
using DATN.DAL.Models;
using DATN.DAL.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DAL.Repository
{
    public class AccountRepository : BaseRepository<Account>
    {
        public AccountRepository(DatabaseContext context) : base(context)
        {
        }

    }
}
