// --------------------------------------------------------------------------
// <copyright file="IResultInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DTO.Common
{
    /// <summary>
    /// Result Info interface.
    /// </summary>
    public interface IResultInfo
    {
        int Code { get; set; }

        bool IsSuccessful { get; }

        string Message { get; set; }

        string ExtendedData { get; set; }
    }
}