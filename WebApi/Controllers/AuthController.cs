using Auth.Domain.Services;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class AuthController : ApiController
    {
        IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [Route("api/signin")]
        public async Task<HttpResponseMessage> Post([FromBody]LoginModel model)
        {
            if(model == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Empty Payload");
            }
            var token = await authService.AuthenticateAsync(model.Email, model.Password);
            return Request.CreateResponse(HttpStatusCode.OK, token);
        }

        
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    
}
