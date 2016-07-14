// -----------------------------------------------------------------------
// <copyright file="IRequest.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DTO.Common
{
    /// <summary>
    /// IRequest interface.
    /// </summary>
    public interface IRequest
    {
        string Ip { get; set; }

        string Domain { get; set; }

        string Browser { get; set; }

        string Platform { get; set; }

        string CountryCode { get; set; }

        string Language { get; set; }
    }
}