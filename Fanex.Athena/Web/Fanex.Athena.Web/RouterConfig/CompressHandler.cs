// -----------------------------------------------------------------------
// <copyright file="CompressHandler.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Web.RouterConfig
{
    using System.Linq;
    using System.Net.Http;
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
                    HttpResponseMessage response = responseToCompleteTask.Result;
                    response.EnsureSuccessStatusCode();
                    if (response.RequestMessage.Headers.AcceptEncoding != null)
                    {
                        var encodingType = response.RequestMessage.Headers.AcceptEncoding.FirstOrDefault();
                        response.Content = new CompressedContent(response.Content, encodingType != null ? encodingType.Value : "gzip");
                    }

                    return response;
                },
             TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}