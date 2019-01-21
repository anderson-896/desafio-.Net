using Dapper;
using Kurier.Analytics.DatabaseHelper;
using Kurier.Analytics.Search.Domain.Entities;
using Kurier.Analytics.Search.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.Analytics.Search.Repositories
{
    public class MonitoringRepository : IMonitoringRepository
    {
        protected IConnectionFactory connectionFactory;

        public MonitoringRepository(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task InsertAsync(Monitoring monitoramento)
        {
            using (var conn = connectionFactory.GetConnection(Connections.PESQUISAANALYTICS))
            {
                string query = @"INSERT INTO TB_MONITORAMENTO_PESQUISA 
                                    (DT_CADASTRO
                                    ,CD_PESQUISA_ANALYTICS
                                    ,NUM_QUANTIDADE_NOVOS_PROCESSOS)
                                    VALUES
                                    (GETDATE()
                                    ,@QueryId
                                    ,@QuantityNewProcesses)";

                await conn.ExecuteAsync(query, monitoramento);
            }
        }

        public async Task<List<Monitoring>> SearchMonitoringByQueryAsync(string pesquisaId)
        {
            using (var conn = connectionFactory.GetConnection(Connections.PESQUISAANALYTICS))
            {
                string query = @"SELECT MON.[CD_MONITORAMENTO_PESQUISA] as MonitoringId
                                      ,MON.[CD_PESQUISA_ANALYTICS] as PesquisaId
                                      ,MON.[DT_CADASTRO] as CreateDate
                                      ,MON.[DT_ALTERACAO] as AlterDate
                                      ,MON.[NUM_QUANTIDADE_NOVOS_PROCESSOS] as QuantityNewProcesses
                                  FROM [TB_MONITORAMENTO_PESQUISA] MON
                                  WHERE MON.CD_PESQUISA_ANALYTICS = @pesquisaId";

                var dapperResult = await conn.QueryAsync(query, new { pesquisaId = pesquisaId });

                var mappedResult = Slapper.AutoMapper.MapDynamic<Monitoring>(dapperResult);

                return mappedResult.ToList();
            }
        }

        public async Task UpdateAsync(Monitoring monitoramento)
        {
            using (var conn = connectionFactory.GetConnection(Connections.PESQUISAANALYTICS))
            {
                string query = @"UPDATE TB_MONITORAMENTO_PESQUISA
                                 SET [CD_STATUS_MONITORAMENTO] = @StatusId
                                ,[DT_ALTERACAO] = GETDATE()
                                ,[DT_CONCLUSAO] = GETDATE()
                                ,[NUM_QUANTIDADE_NOVOS_PROCESSOS] = @QuantityNewProcesses
                            WHERE [CD_MONITORAMENTO_PESQUISA] = @MonitoringId";

                await conn.ExecuteAsync(query, monitoramento);
            }
        }
    }
}
