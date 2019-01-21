using Kurier.Analytics.Search.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.Analytics.Search.Domain.Repositories
{
    public interface IQueryQueueRepository
    {
        /// <summary>
        /// Pesquisa na base do Track ETL os ids de pesquisas disponíveis para indexação
        /// </summary>
        /// <param name="idUltimaPesquisaIndexada"></param>
        /// <returns></returns>
        Task<List<string>> SearchAvaliableQueryIdsAsync(DateTime? ultimaIndexacao);

        /// <summary>
        /// Pesquisa os dados completos da pesquisas disponíveis para indexação
        /// </summary>
        /// <param name="idsPesquisa"></param>
        /// <returns></returns>
        Task<List<UserSimpleQuery>> SearchAvaliableQueryAsync(List<string> idsPesquisa);

        Task<UserSimpleQuery> InserirAsync(UserSimpleQuery query);
    }
}
