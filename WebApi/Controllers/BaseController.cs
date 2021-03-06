﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public abstract class BaseController : ApiController
    {
        protected SecretAuthenticationIdentity RecuperarUsuarioAutenticado()
        {
            return System.Web.HttpContext.Current.User.Identity as SecretAuthenticationIdentity;
        }
    }
}
