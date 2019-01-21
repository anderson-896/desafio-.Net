using Dapper;
using Kurier.Analytics.DatabaseHelper;
using Kurier.Analytics.Search.Domain.Entities;
using Kurier.Analytics.Search.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.Analytics.Search.Repositories
{
    public class IndexingRepository : IIndexingRepository
    {
        protected readonly IConnectionFactory connectionFactory;

        public IndexingRepository(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task InsertIndexingAsync(Indexing indexacao)
        {
            using (SqlConnection conn = connectionFactory.GetConnection(Connections.CONTROLEINDEX))
            {
                string query = @"INSERT INTO [TB_CONTROLE_INDEXACAO]
                                    ([CD_PESQUISA]
                                    ,[DT_CADASTRO]
                                    ,[CD_STATUS_INDEXACAO]
                                    ,[DT_ATUALIZACAO])
                                VALUES
                                    (@PesquisaId
                                    ,GETDATE()
                                    ,@StatusId
                                    ,GETDATE())";

                await conn.ExecuteAsync(query, indexacao);
            }
        }

        public async Task<List<string>> SearchByStatusAsync(int statusId)
        {
            using (SqlConnection conn = connectionFactory.GetConnection(Connections.CONTROLEINDEX))
            {
                string query = @"SELECT CINDEX.[CD_PESQUISA] as PesquisaId
                            FROM [dbo].[TB_CONTROLE_INDEXACAO] CINDEX
                            WHERE [CD_STATUS_INDEXACAO] = @statusId";

                var dapperResult = await conn.QueryAsync<string>(query, new { statusId = statusId });

                return dapperResult.ToList();
            }
        }

        public async Task<Indexing> SearchLastIndexedQueryAsync()
        {
            using (SqlConnection conn = connectionFactory.GetConnection(Connections.CONTROLEINDEX))
            {
                string query = @"SELECT TOP(1)
   	                             CINDEX.[CD_CONTROLE_INDEXACAO] as IndexacaoId
                                ,CINDEX.[CD_PESQUISA] as PesquisaId
                                ,CINDEX.[CD_STATUS_INDEXACAO] as StatusId
                                ,CINDEX.[DT_CADASTRO] as DataCadastro
                                ,CINDEX.[DT_ATUALIZACAO] as DataAtualizacao
                                ,CINDEX.[DT_INDEXANDO] as DataIndexando
                                ,CINDEX.[DT_INDEXADO] as DataIndexado
                                ,CINDEX.[DT_ERRO] as DataErro
	                            ,SINDEX.[DS_STATUS_INDEXACAO] as StatusDescricao
                            FROM [dbo].[TB_CONTROLE_INDEXACAO] CINDEX
                            LEFT JOIN TB_STATUS_INDEXACAO SINDEX ON CINDEX.[CD_STATUS_INDEXACAO] = SINDEX.[CD_STATUS_INDEXACAO]
                            ORDER BY CINDEX.[DT_INDEXADO] DESC";

                var dapperResult = await conn.QueryAsync(query);

                var mappedResult = Slapper.AutoMapper.MapDynamic<Indexing>(dapperResult).ToList().FirstOrDefault();

                return mappedResult;
            }
        }

        public async Task UpdateIndexedAsync(Indexing indexacao)
        {
            using (SqlConnection conn = connectionFactory.GetConnection(Connections.CONTROLEINDEX))
            {
                string query = @"UPDATE TB_CONTROLE_INDEXACAO
                                SET [CD_STATUS_INDEXACAO] = @StatusId
                                    ,[DT_ATUALIZACAO] = @DataAtualizacao
                                    ,[DT_INDEXADO] = @DataIndexado
                                WHERE [CD_PESQUISA] = @PesquisaId";

                await conn.ExecuteAsync(query, indexacao);
            }
        }

        public async Task UpdateIndexingAsync(Indexing indexacao)
        {
            using (SqlConnection conn = connectionFactory.GetConnection(Connections.CONTROLEINDEX))
            {
                string query = @"UPDATE TB_CONTROLE_INDEXACAO
                                SET [CD_STATUS_INDEXACAO] = @StatusId
                                    ,[DT_ATUALIZACAO] = @DataAtualizacao
                                    ,[DT_INDEXADO] = @DataIndexado
                                WHERE [CD_PESQUISA] = @PesquisaId";

                await conn.ExecuteAsync(query, indexacao);
            }
        }
    }
}
