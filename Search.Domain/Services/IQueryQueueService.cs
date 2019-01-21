using Kurier.Analytics.CrossCutting.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.Analytics.Search.Domain.Services
{
    public interface IQueryQueueService
    {
        /// <summary>
        /// Verifica as pesquisas disponíveis e realiza a indexação na base do ElasticSearch
        /// </summary>
        /// <returns></returns>
        Task<Response> InsertAync();
    }
}
