using Kurier.Analytics.Search.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.Analytics.Search.Domain.Repositories
{
    public interface IMonitoringRepository
    {
        Task<List<Monitoring>> SearchMonitoringByQueryAsync(string pesquisaId);

        Task InsertAsync(Monitoring monitoramento);

        Task UpdateAsync(Monitoring monitoramento);
    }
}
