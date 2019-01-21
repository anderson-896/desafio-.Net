using Kurier.Analytics.CrossCutting.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kurier.Analytics.Search.Domain.Entities;
using System.Threading.Tasks;

namespace Kurier.Analytics.Search.Domain.Services
{
    public interface IIndexingService
    {
        /// <summary>
        /// Monitora as pesquisas disponíveis para indexação e atualiza o status no controle de indexação
        /// </summary>
        /// <returns></returns>
        Task<Response<IndexingResult>> InsertAync();
    }
}

