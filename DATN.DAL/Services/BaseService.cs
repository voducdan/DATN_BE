using DATN.DAL.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATN.DAL.Services
{
    public class BaseService
    {
        protected DatabaseContext context;

        public BaseService(DatabaseContext context)
        {
            this.context = context;
        }
    }
}
