using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DATN.Infrastructure.Helpers;
using DATN.DAL.Services;
using System.IO;
using DATN.DAL.Models;
using DATN.DAL.Context;

namespace DATN.DAL.Repository
{
    public class RequestJobRepository : BaseRepository<RequestJob>
    {
        public RequestJobRepository(DatabaseContext context) : base(context)
        {

        }
    }
}
