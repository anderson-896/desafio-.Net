using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebApi
{
    public class ThreadCultureMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // 1. prioritize languages based upon quality
            var langauges = new List<StringWithQualityHeaderValue>();

            if (request.Headers.AcceptLanguage != null)
            {
                // then check the Accept-Language header.
                langauges.AddRange(request.Headers.AcceptLanguage);
            }

            // sort the languages with quality so we can check them in order.
            langauges = langauges.OrderByDescending(l => l.Quality).ToList();

            CultureInfo culture = null;

            // 2. try to find one language that's available
            foreach (StringWithQualityHeaderValue lang in langauges)
            {
                try
                {
                    culture = CultureInfo.GetCultureInfo(lang.Value);
                    break;
                }
                catch (CultureNotFoundException)
                {
                    // ignore the error
                }
            }

            // 3. if a language is available, set the thread culture
            if (culture != null)
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}