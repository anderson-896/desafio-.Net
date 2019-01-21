using Kurier.Analytics.CrossCutting.Response;
using Kurier.Analytics.Search.Domain.Entities;
using Kurier.Analytics.Search.Domain.Repositories;
using Kurier.Analytics.Search.Domain.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.Analytics.Search.Services
{
    public class QueryQueueService : IQueryQueueService
    {
        protected readonly IQueryQueueRepository queryQueueRepository;
        protected readonly IIndexingRepository indexingRepository;
        protected readonly IQueryRepository queryRepository;

        public QueryQueueService(IQueryQueueRepository queryQueueRepository, IIndexingRepository indexingRepository, IQueryRepository queryRepository)
        {
            this.queryQueueRepository = queryQueueRepository;
            this.indexingRepository = indexingRepository;
            this.queryRepository = queryRepository;
        }

        public async Task<Response> InsertAync()
        {
            // 1 - Verifica as pesquisas disponiveis para indexacao
            List<string> pesquisasAguardandoIndexacaoIds = await indexingRepository.SearchByStatusAsync(1);

            // 2 - Atualiza a tabela de controle de indexação para "indexando"
            foreach(var pesquisaId in pesquisasAguardandoIndexacaoIds)
            {
                Indexing indexacaoAtual = new Indexing
                {
                    PesquisaId = pesquisaId,
                    StatusId = 2,
                    DataAtualizacao = DateTime.Now,
                    DataIndexando = DateTime.Now
                };
                await indexingRepository.UpdateIndexingAsync(indexacaoAtual);
            }
            
            // 3 - Pesquisa as propriedades da pesquisa e compõe o objeto de pesquisa completo
            List<UserSimpleQuery> pesquisas = await queryQueueRepository.SearchAvaliableQueryAsync(pesquisasAguardandoIndexacaoIds);

            // 4 - Executa a indexação no elasticsearch
            foreach (var pesquisa in pesquisas)
            {
                dynamic pesquisaDyn = JsonConvert.DeserializeObject(pesquisa.QueryJsonDescription);
                var pesquisaObj = pesquisaDyn.ToObject<UserSimpleQuery>();

                await queryRepository.InsertAsync(pesquisaObj);

                // 5 - Atualiza a tabela de controle de indexação para "indexado"
                Indexing indexacaoAtual = new Indexing
                {
                    PesquisaId = pesquisa.QueryId,
                    StatusId = 3,
                    DataAtualizacao = DateTime.Now,
                    DataIndexado = DateTime.Now
                };

                await indexingRepository.UpdateIndexingAsync(indexacaoAtual);
            }

            return Response.CreateResponse();
        }
    }
}
