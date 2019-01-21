using Brasil = Kurier.Analytics.Acoes.Domain.Services.Brasil;
using Chile = Kurier.Analytics.Acoes.Domain.Services.Chile;
using Argentina = Kurier.Analytics.Acoes.Domain.Services.Argentina;
using Kurier.Analytics.CrossCutting.Response;
using Kurier.Analytics.Search.Domain.Entities;
using Kurier.Analytics.Search.Domain.Repositories;
using Kurier.Analytics.Search.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kurier.Analytics.Acoes.Domain.Entities;
using Kurier.Analytics.Acoes.Domain.Services;
using Kurier.Analytics.Core.Domain.Response;

namespace Kurier.Analytics.Search.Services
{
    public class MonitoringService : IMonitoringService
    {
        protected readonly IQueryRepository queryRepository;
        protected readonly IMonitoringRepository monitoringRepository;
        protected readonly Brasil.IAcoesService brAcoesService;
        protected readonly Chile.IAcoesService chAcoesService;
        protected readonly Argentina.IAcoesService agAcoesService;
        protected readonly IUsuarioAcoesService usuarioAcoesService;

        public MonitoringService(IQueryRepository queryRepository, IMonitoringRepository monitoringRepository, Brasil.IAcoesService brAcoesService,
            Chile.IAcoesService chAcoesService, Argentina.IAcoesService agAcoesService, IUsuarioAcoesService usuarioAcoesService)
        {
            this.queryRepository = queryRepository;
            this.monitoringRepository = monitoringRepository;
            this.brAcoesService = brAcoesService;
            this.chAcoesService = chAcoesService;
            this.agAcoesService = agAcoesService;
            this.usuarioAcoesService = usuarioAcoesService;
        }
        
        public async Task<Response> MonitorQueryAsync()
        {
            IResponse<SimpleQueryResult<Acoes.Domain.Entities.Brasil.Processo>> queryResultBrazil = new Response<SimpleQueryResult<Acoes.Domain.Entities.Brasil.Processo>>();
            IResponse<SimpleQueryResult<Acoes.Domain.Entities.Chile.Processo>> queryResultChile = new Response<SimpleQueryResult<Acoes.Domain.Entities.Chile.Processo>>();
            IResponse<SimpleQueryResult<Acoes.Domain.Entities.Argentina.Processo>> queryResultArgentina = new Response<SimpleQueryResult<Acoes.Domain.Entities.Argentina.Processo>>();

            // 1 - Retorna todas as pesquisas monitoradas
            var pesquisasMonitoradas = await queryRepository.SearchMonitoredByUserAsync();

            foreach (UserSimpleQuery pesquisa in pesquisasMonitoradas)
            {
                IResponse<User> userContent = await usuarioAcoesService.SearchByIdAsync(pesquisa.UserId);
                SearchTemplate template = userContent.Content.SearchTemplates.SingleOrDefault(e => e.SearchTemplateId == pesquisa.SearchTemplateId);

                // 2 - Verifica o histórico de monitoramentos da pesquisa atual
                List<Monitoring> historicoMonitoramentos = await monitoringRepository.SearchMonitoringByQueryAsync(pesquisa.QueryId);

                Monitoring monitoramentoAtual = new Monitoring();
                
                // Realiza nova pesquisa com os mesmos parâmetros
                switch (pesquisa.SearchTemplateId)
                {
                    case 1: // Brasil
                        queryResultBrazil = await brAcoesService.ConsultarProcessoAsync(pesquisa, userContent.Content, template);
                        monitoramentoAtual.QuantityNewProcesses = queryResultBrazil.Content.Total - pesquisa.TotalResult;
                        break;
                    case 2: // Chile
                        queryResultChile = await chAcoesService.ConsultarProcessoAsync(pesquisa, userContent.Content, template);
                        monitoramentoAtual.QuantityNewProcesses = queryResultChile.Content.Total - pesquisa.TotalResult;
                        break;
                    case 3: // Argentina
                        queryResultArgentina = await agAcoesService.ConsultarProcessoAsync(pesquisa, userContent.Content, template);
                        monitoramentoAtual.QuantityNewProcesses = queryResultArgentina.Content.Total - pesquisa.TotalResult;
                        break;
                    default:
                        break;
                }
                
                // Cadastra o monitoramentos na base de dados
                monitoramentoAtual.StatusId = 2;
                monitoramentoAtual.QueryId = pesquisa.QueryId;
                
                await monitoringRepository.InsertAsync(monitoramentoAtual);
            }

            return Response.CreateResponse();
        }
    }
}
