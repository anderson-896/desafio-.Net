using Kurier.Analytics.Acoes.Domain.Entities;
using Kurier.Analytics.DatabaseHelper;
using Kurier.Analytics.Search.Domain.Entities;
using Kurier.Analytics.Search.Domain.Repositories;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.Analytics.Search.Repositories
{
    public class QueryRepository : IQueryRepository
    {
        const string PESQUISA_INDICE = "kurier_pesquisa_v2";
        const string PESQUISA_TIPO = "pesquisa";

        const string PESQUISA_INDICE_TESTE = "pesquisa_teste";
        const string PESQUISA_TESTE = "pesquisateste";

        protected readonly IElasticClientFactory factory;

        public QueryRepository(IElasticClientFactory factory)
        {
            this.factory = factory;
        }

        public async Task<UserSimpleQuery> InsertAsync(UserSimpleQuery pesquisa)
        {
            ElasticClient client = factory.NewClient();

            pesquisa.QueryId = Guid.NewGuid().ToString();
            pesquisa.CreateDate = DateTime.Now;

            var response = await client.CreateAsync(pesquisa, i => i
                .Id(pesquisa.QueryId)
                .Index(PESQUISA_INDICE)
                .Type(PESQUISA_TIPO));

            return pesquisa;
        }

        public async Task<List<UserSimpleQuery>> SearchMonitoredByUserAsync()
        {
            ElasticClient client = factory.NewClient();

            var response = await client
            .SearchAsync<UserSimpleQuery>(s => s
            .Size(10)
            .Index(PESQUISA_INDICE_TESTE)
            .Type(PESQUISA_TESTE)
            .Query(q => q
                .Bool(b => b
                    .Must(
                        mon => mon.Term(t => t.Field(f => f.IsMonitored).Value(true)),
                        st => st.Term(t => t.Field(f => f.IsActive).Value(true))))));

            return response.Documents.ToList();
        }

        public async Task UpdateAsync(UserSimpleQuery entidade)
        {
            ElasticClient client = factory.NewClient();

            entidade.ModifyDate = DateTime.Now;

            await client.IndexAsync(entidade, i => i.Id(entidade.QueryId).Index(PESQUISA_INDICE).Type(PESQUISA_TIPO));
        }

        public async Task<UserSimpleQuery> SearchAsync(string chave)
        {
            ElasticClient client = factory.NewClient();

            var response = await client
                .SearchAsync<UserSimpleQuery>(s => s
                .Index(PESQUISA_INDICE)
                .Type(PESQUISA_TIPO)
                .Query(q => q.Ids(i => i.Values(chave))));

            return response.Documents.SingleOrDefault();
        }

        public async Task<IEnumerable<UserSimpleQuery>> SearchByUserAsync(int usuarioId)
        {
            ElasticClient client = factory.NewClient();

            var response = await client
                .SearchAsync<UserSimpleQuery>(s => s
                .Size(100)
                .Index(PESQUISA_INDICE)
                .Type(PESQUISA_TIPO)
                .Query(q => q.Term(t => t.Field(f => f.UserId).Value(usuarioId))));

            return response.Documents;
        }

        public async Task<IEnumerable<UserSimpleQuery>> SearchLastAsync(int usuarioId)
        {
            ElasticClient client = factory.NewClient();

            var response = await client
                .SearchAsync<UserSimpleQuery>(s => s
                .Size(10)
                .Index(PESQUISA_INDICE_TESTE)
                .Type(PESQUISA_TESTE)
                .Query(q => q.Term(t => t.Field(f => f.UserId).Value(usuarioId)))
                .Sort(q => q.Descending(u => u.CreateDate)));

            return response.Documents;
        }

        public async Task DeleteAsync(string chave)
        {
            ElasticClient client = factory.NewClient();

            await client.DeleteAsync<UserSimpleQuery>(chave, (config) => config.Index(PESQUISA_INDICE).Type(PESQUISA_TIPO));
        }
    }
}
