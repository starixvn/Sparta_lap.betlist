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
    public class ApiCompressHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => base.SendAsync(request, cancellationToken).ContinueWith(
                responseToCompleteTask =>
                {
                    var response = responseToCompleteTask.Result;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        response.Content = new StringContent("Unauthorized.");
                    }

                    if (response.RequestMessage != null && response.RequestMessage.Headers.AcceptEncoding != null
                    && response.RequestMessage.Headers.AcceptEncoding.Count > 0)
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