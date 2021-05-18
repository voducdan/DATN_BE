using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN.API.Commands
{
    public class CreateAccountCustomerCommand
    {
        public string email { get; set; }
        public string password { get; set; }
        public string password_confirm { get; set; }
        public string  ten_kh { get; set; }
        public string sdt { get; set; }

    }
}
