using Kurier.Analytics.DatabaseHelper;
using Kurier.Analytics.Search.Domain.Entities;
using Kurier.Analytics.Search.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;

namespace Kurier.Analytics.Search.Repositories
{
    public class QueryQueueRepository : IQueryQueueRepository
    {
        protected readonly IConnectionFactory connectionFactory;

        public QueryQueueRepository(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<UserSimpleQuery> InserirAsync(UserSimpleQuery query)
        {
            query.QueryId = Guid.NewGuid().ToString();
            query.CreateDate = DateTime.Now;

            string jsonStr = JsonConvert.SerializeObject(query);

            using (SqlConnection conn = connectionFactory.GetConnection(Connections.PESQUISAANALYTICS))
            {
                string queryQueue = @"INSERT INTO [TB_PESQUISA_ANALYTICS]
                                            ([CD_PESQUISA_ANALYTICS]
                                            ,[JSON_PESQUISA]
                                            ,[DT_CADASTRO]
                                            ,[ST_INATIVO]
                                            ,[DT_ALTERACAO])
                                            VALUES
                                            (@QueryId
                                            ,@jsonStr
                                            ,GETDATE()
                                            ,0
                                            ,GETDATE())";

                await conn.ExecuteAsync(queryQueue, new { QueryId = query.QueryId, jsonStr = jsonStr });
            }

            return query;
        }

        public async Task<List<UserSimpleQuery>> SearchAvaliableQueryAsync(List<string> idsPesquisa)
        {
            using (SqlConnection conn = connectionFactory.GetConnection(Connections.PESQUISAANALYTICS))
            {
                string query = @"SELECT TOP (1000) 
                                   [CD_PESQUISA_ANALYTICS] as QueryId
                                  ,[JSON_PESQUISA] as QueryJsonDescription
                                  ,[DT_CADASTRO] as CreateDate
                                  ,[DT_ALTERACAO] as UpdateDate
                                  ,[ST_INATIVO] as Inativo
                              FROM [TB_PESQUISA_ANALYTICS]
                              WHERE CD_PESQUISA_ANALYTICS IN @idsPesquisa";

                IEnumerable<dynamic> dapperResult = await conn.QueryAsync(query, new { idsPesquisa = idsPesquisa });

                List<UserSimpleQuery> mappedResult = Slapper.AutoMapper.MapDynamic<UserSimpleQuery>(dapperResult).ToList();

                return mappedResult;
            }
        }

        public async Task<List<string>> SearchAvaliableQueryIdsAsync(DateTime? ultimaIndexacao)
        {
            using (SqlConnection conn = connectionFactory.GetConnection(Connections.PESQUISAANALYTICS))
            {
                string query = @"SELECT TOP(1000) [CD_PESQUISA_ANALYTICS]
                                    FROM [dbo].[TB_PESQUISA_ANALYTICS]
                                    WHERE [DT_ALTERACAO] > @ultimaIndexacao
                                    ORDER BY [DT_ALTERACAO]";

                var dapperResult = await conn.QueryAsync<string>(query, new { ultimaIndexacao = ultimaIndexacao });

                return dapperResult.ToList();
            }
        }


    }
}
