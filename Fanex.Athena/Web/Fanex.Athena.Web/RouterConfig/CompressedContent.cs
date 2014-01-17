// -----------------------------------------------------------------------
// <copyright file="CompressedContent.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Web.RouterConfig
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Compressed content class.
    /// </summary>
    public class CompressedContent : HttpContent
    {
        #region Fields

        /// <summary>
        /// Original content.
        /// </summary>
        private HttpContent _originalContent;

        /// <summary>
        /// Encoding type.
        /// </summary>
        private string _encodingType;

        #endregion Fields

        #region Constructors

        public CompressedContent(HttpContent content, string encodingType)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            if (encodingType == null)
            {
                throw new ArgumentNullException("encodingType");
            }

            this._originalContent = content;
            this._encodingType = encodingType.ToLowerInvariant();

            if (this._encodingType != "gzip" && this._encodingType != "deflate")
            {
                throw new InvalidOperationException(string.Format("Encoding '{0}' is not supported. Only supports gzip or deflate encoding.", this._encodingType));
            }

            // copy the headers from the original content
            foreach (KeyValuePair<string, IEnumerable<string>> header in this._originalContent.Headers)
            {
                this.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            this.Headers.ContentEncoding.Add(encodingType);
        }

        #endregion Constructors

        #region Override

        /// <summary>
        /// Try Compute Length.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns>True or false.</returns>
        protected override bool TryComputeLength(out long length)
        {
            length = -1;

            return false;
        }

        /// <summary>
        /// Serialize stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="context">The transport content.</param>
        /// <returns>The serialization task.</returns>
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            Stream compressedStream = null;

            if (this._encodingType == "gzip")
            {
                compressedStream = new GZipStream(stream, CompressionMode.Compress, leaveOpen: true);
            }
            else if (this._encodingType == "deflate")
            {
                compressedStream = new DeflateStream(stream, CompressionMode.Compress, leaveOpen: true);
            }

            return this._originalContent.CopyToAsync(compressedStream).ContinueWith(tsk =>
            {
                if (compressedStream != null)
                {
                    compressedStream.Dispose();
                }
            });
        }

        #endregion Override
    }
}