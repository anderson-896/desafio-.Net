using Kurier.Analytics.Core.Domain.Response;
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
    public class QueryService : IQueryService
    {
        IQueryRepository queryRepository;
        IQueryQueueRepository queryQueueRepository;

        public QueryService(IQueryRepository queryRepository, IQueryQueueRepository queryQueueRepository)
        {
            this.queryRepository = queryRepository;
            this.queryQueueRepository = queryQueueRepository;
        }

        public async Task<IResponse> InsertAsync(UserSimpleQuery query)
        {
            // 1 - Valida se campos estão preenchidos
            if (string.IsNullOrWhiteSpace(query.Name) && query.QueryType == 1)
            {
                return Response
                    .CreateErrorResponse(ValidationMessageHelper
                    .Create(ValidationMessages.REQUIRED_FIELD_NAME));
            }

            // 2 - Pesquisa a existência de pesquisa com o mesmo nome
            var pesquisas = await queryRepository.SearchByUserAsync(query.UserId);

            if (pesquisas.Where(e => e.Name == query.Name).Count() > 0)
            {
                return Response
                    .CreateErrorResponse(ValidationMessageHelper
                    .Create(ValidationMessages.DUPLICATE_NAME_SEARCH));
            }

            // 3 - Caso pesquisa seja automática, enfileira a indexação da pesquisa
            //if (query.QueryType == 2) return Response.CreateResponse(await queryQueueRepository.InserirAsync(query));

            // 4 - Salva as informações da pesquisa
            var response = await queryRepository.InsertAsync(query);

            return Response.CreateResponse(response);
        }
        
        public async Task<IResponse> UpdateAsync(UserSimpleQuery query)
        {
            var userQuery = await queryRepository.SearchByUserAsync(query.UserId);

            if (!userQuery.Any(e => e.QueryId == query.QueryId))
                return Response.CreateErrorResponse(ValidationMessageHelper.Create(ValidationMessages.QUERY_NOT_FOUND));

            await queryRepository.UpdateAsync(query);

            return Response.CreateResponse();
        }

        public async Task<IResponse<IEnumerable<UserSimpleQuery>>> SearchByUserAsync(int userId)
        {
            return Response.CreateResponse(await queryRepository.SearchByUserAsync(userId));
        }

        public async Task<IResponse<IEnumerable<UserSimpleQuery>>> SearchLastAsync(int userId)
        {
            return Response.CreateResponse(await queryRepository.SearchLastAsync(userId));
        }

        public async Task<IResponse<UserSimpleQuery>> SearchByQueryAsync(string queryId)
        {
            return Response.CreateResponse(await queryRepository.SearchAsync(queryId));
        }

        public async Task<IResponse> DeleteAsync(UserSimpleQuery query)
        {
            var userQuery = await queryRepository.SearchByUserAsync(query.UserId);

            if (!userQuery.Any(e => e.QueryId == query.QueryId))
                return Response.CreateErrorResponse(ValidationMessageHelper.Create(ValidationMessages.QUERY_NOT_FOUND));

            await queryRepository.DeleteAsync(query.QueryId);

            return Response.CreateResponse();
        }

       }
}
