using DATN.DAL.Context;
using DATN.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DATN.Infrastructure.Helpers;
using DATN.DAL.Services;
using System.IO;

namespace DATN.DAL.Repository
{
    public class JobRepository : BaseRepository<Job>
    {
        public JobRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
