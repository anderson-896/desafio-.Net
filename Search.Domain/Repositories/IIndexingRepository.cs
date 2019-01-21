using Kurier.Analytics.Search.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.Analytics.Search.Domain.Repositories
{
    public interface IIndexingRepository
    {
        /// <summary>
        /// Pesquisa a última pesquisa indexada no controle de indexação
        /// </summary>
        /// <returns></returns>
        Task<Indexing> SearchLastIndexedQueryAsync();

        /// <summary>
        /// Pesquisa as indexações pelo status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        Task<List<string>> SearchByStatusAsync(int statusId);

        /// <summary>
        /// Atualiza os parâmetros do registro no controle de indexação para "indexando"
        /// </summary>
        /// <param name="indexacao"></param>
        /// <returns></returns>
        Task UpdateIndexingAsync(Indexing indexacao);

        /// <summary>
        /// Atualiza os parâmetros do registro no controle de indexação para "indexado"
        /// </summary>
        /// <param name="indexacao"></param>
        /// <returns></returns>
        Task UpdateIndexedAsync(Indexing indexacao);

        /// <summary>
        /// Insere um registro no controle de indexação
        /// </summary>
        /// <param name="indexacao"></param>
        /// <returns></returns>
        Task InsertIndexingAsync(Indexing indexacao);
    }
}
