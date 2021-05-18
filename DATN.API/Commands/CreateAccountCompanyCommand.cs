using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN.API.Commands
{
    public class CreateAccountCompanyCommand
    {
        public string email { get; set; }
        public string password { get; set; }
        public string password_confirm { get; set; }
        public string ten_kh { get; set; }
        public string sdt { get; set; }
        // Thông tin nhà tuyển dụng company
        public string ten_doanh_nghiep { get; set; }
        public string mo_ta { get; set; }
        public string dia_chi_cu_the { get; set; }
        public string ten_thanh_pho { get; set; }

    }
}
