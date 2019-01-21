using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Extensions
{
    public static class HttpContextExtensions
    {
        public static string[] GetIndiceFromGetParams(this HttpContext context)
        {
            string[] anosPesquisa = context.Request.Params.GetValues("anoPesquisa");
            if (anosPesquisa != null && anosPesquisa.Length == 1)
            {
                anosPesquisa = anosPesquisa.ElementAtOrDefault(0)?.Split(',');
            }
            return anosPesquisa;
        }
    }
}