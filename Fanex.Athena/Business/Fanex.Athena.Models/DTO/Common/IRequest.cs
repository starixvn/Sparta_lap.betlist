// -----------------------------------------------------------------------
// <copyright file="IRequest.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Common
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