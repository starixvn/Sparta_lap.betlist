// -----------------------------------------------------------------------
// <copyright file="CompressHandler.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.HttpCompress
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Compress Handler class.
    /// </summary>
    public class CompressHandler : DelegatingHandler
    {
        /// <summary>
        /// Compress response and send to client.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response task.</returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith(
                responseToCompleteTask =>
                {
                    var response = responseToCompleteTask.Result;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        response.Content = new StringContent("Unauthorized.");
                    }

                    if (response.RequestMessage != null)
                    {
                        var encoding = FindEncodingType(response.RequestMessage.Headers.AcceptEncoding);
                        if (!string.IsNullOrEmpty(encoding))
                        {
                            response.Content = new CompressedContent(response.Content, encoding);
                        }
                    }

                    return response;
                },
             TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private static string FindEncodingType(HttpHeaderValueCollection<StringWithQualityHeaderValue> acceptEncoding)
        {
            const string GZIP = "gzip";
            const string DEFLATE = "deflate";
            const string IDENTITY = "identity";
            var encodings = new QValueList(true);
            var ordinal = -1;
            foreach (var item in acceptEncoding)
            {
                encodings.Add(QValue.Parse(item.Value, ++ordinal, item.Quality.HasValue ? item.Quality.Value : 1));
            }

            var preferred = encodings.FindPreferred(GZIP, DEFLATE, IDENTITY);
            if (preferred.IsEmpty && encodings.AcceptWildcard && encodings.Find(GZIP).IsEmpty)
            {
                preferred = new QValue(GZIP);
            }

            return preferred.Name.Equals(GZIP) || preferred.Name.Equals(DEFLATE) ? preferred.Name : string.Empty;
        }
    }
}