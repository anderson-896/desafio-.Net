using Kurier.Analytics.CrossCutting.Response;
using Kurier.Analytics.Search.Domain.Entities;
using Kurier.Analytics.Search.Domain.Repositories;
using Kurier.Analytics.Search.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.Analytics.Search.Services
{
    public class IndexingService : IIndexingService
    {
        protected readonly IQueryQueueRepository queryQueueRepository;
        protected readonly IIndexingRepository indexingRepository;
        protected readonly IQueryQueueService queryQueueService;

        public IndexingService(IQueryQueueRepository queryQueueRepository, IIndexingRepository indexingRepository, IQueryQueueService queryQueueService)
        {
            this.queryQueueRepository = queryQueueRepository;
            this.indexingRepository = indexingRepository;
            this.queryQueueService = queryQueueService;
        }

        public async Task<Response<IndexingResult>> InsertAync()
        {
            // 1 - Pesquisa no controle de indexação o id da última pesquisa indexada
            Indexing ultimaIndexacao = await indexingRepository.SearchLastIndexedQueryAsync();

            // 2 - Pesquisa no bases de pesquisas salvas todas as entradas novas ou as que sofreram alteração
            List<string> pesquisasDisponiveisIds = await queryQueueRepository.SearchAvaliableQueryIdsAsync(ultimaIndexacao.DataIndexado);

            // 3 - Havendo pesquisas disponíveis para indexação, realiza o cadastro da indexação com o status "Aguardando indexação"
            foreach (var item in pesquisasDisponiveisIds)
            {
                Indexing indexacao = new Indexing
                {
                    PesquisaId = item,
                    StatusId = 1,
                };
                await indexingRepository.InsertIndexingAsync(indexacao);
            }

            await queryQueueService.InsertAync();

            IndexingResult resultado = new IndexingResult { TotalIndexedDocuments = pesquisasDisponiveisIds.Count };

            return Response.CreateResponse(resultado);
        }
    }
}