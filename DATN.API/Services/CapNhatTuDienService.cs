using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DATN.DAL.Models;
using DATN.DAL.Services;

namespace DATN.API.Services
{

    public interface ICapNhatTuDien
    {
        Task CapNhat(AddJob job, CancellationToken cancellationToken = default);
    }


    public class CapNhatTuDienService : ICapNhatTuDien
    {
        private JobService jobService;

        public CapNhatTuDienService(JobService jobService)
        {
            this.jobService = jobService;
        }

        public async Task CapNhat(AddJob job, CancellationToken cancellationToken = default)
        {

            await jobService.CapNhatBoTuDien(job.skills);

        }
    }
}
