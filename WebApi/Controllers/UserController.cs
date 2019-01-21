using Actions.Domain.Entities;
using Actions.Domain.Services;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi.Controllers
{
    [SecretAuthentication(Ativo = true)]
    public class MeController : BaseController
    {
        IUserActionsService usuarioAcoesService;
        public MeController(IUserActionsService usuarioAcoesService)
        {
            this.usuarioAcoesService = usuarioAcoesService;
        }
        
        [Route("api/me")]
        public async Task<HttpResponseMessage> Get()
        {
            var response = await usuarioAcoesService.SearchByIdAsync(RecuperarUsuarioAutenticado().UserInfo.UserId);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [SecretAuthentication(Ativo = false)]
        [Route("api/signup")]
        public async Task<HttpResponseMessage> Post([FromBody]User model)
        {
            var response = await usuarioAcoesService.InsertAsync(model);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
