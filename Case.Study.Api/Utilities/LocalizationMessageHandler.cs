using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Case.Study.Api.Utilities
{
    // on service request accepting lang from header.
    public class LocalizationMessageHandler : DelegatingHandler
    {
        //AcceptLanguage property  “tr-TR”
        private readonly List<string> _supportedLanguages = new List<string>() { "tr-TR", "en-US" };

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            SetCulture(request);

            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }

        #region Private Methods

        private void SetCulture(HttpRequestMessage request)
        {
            foreach (var loopLanguage in request.Headers.AcceptLanguage)
            {
                // updating culture
                if (_supportedLanguages.Contains(loopLanguage.Value))
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(loopLanguage.Value);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(loopLanguage.Value);

                    break;
                }
            }
        }
        #endregion
    }
}