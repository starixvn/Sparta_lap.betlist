// --------------------------------------------------------------------------
// <copyright file="ResultInfo.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DTO.Common
{
    using System;

    [Serializable]
    public class ResultInfo<T> : ResultInfo, IResultInfo<T>
    {
        public new T ExtendedData { get; set; }
    }
}