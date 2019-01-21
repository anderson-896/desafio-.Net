using Kurier.Analytics.Acoes.Domain.Entities;
using Kurier.Analytics.Search.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.Analytics.Search.Domain.Repositories
{
    public interface IQueryRepository
    {
        /// <summary>
        /// Executa a indexação da pesquisa na base do ElasticSearch
        /// </summary>
        /// <param name="pesquisa"></param>
        /// <returns></returns>
        Task<UserSimpleQuery> InsertAsync(UserSimpleQuery pesquisa);

        Task<List<UserSimpleQuery>> SearchMonitoredByUserAsync();

        Task<UserSimpleQuery> SearchAsync(string queryId);

        Task<IEnumerable<UserSimpleQuery>> SearchByUserAsync(int userId);

        Task UpdateAsync(UserSimpleQuery query);

        Task DeleteAsync(string queryId);

        Task<IEnumerable<UserSimpleQuery>> SearchLastAsync(int usuarioId);
    }
}
