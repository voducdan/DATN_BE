using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.DAL.Context;
using DATN.DAL.Models;

namespace DATN.DAL.Repository
{
    public class BoTuDienRepository : BaseRepository<BoTuDien>
    {
        public BoTuDienRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
