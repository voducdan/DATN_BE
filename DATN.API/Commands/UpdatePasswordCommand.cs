using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN.API.Commands
{
    public class UpdatePasswordCommand
    {
        public string email { get; set; }
        public string password_old  { get; set; }
        public string password_new { get; set; }
        public string  password_confirm { get; set; }
    }
}
