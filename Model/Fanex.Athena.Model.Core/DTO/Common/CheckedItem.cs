// --------------------------------------------------------------------------
// <copyright file="CheckedItem.cs" company="">
// Copyright (coffee) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DTO.Common
{
    using System;

    /// <summary>
    /// Check box item class.
    /// used for multiple selected drop down list.
    /// </summary>
    [Serializable]
    public class CheckedItem
    {
        public string Name { get; set; }

        public int ID { get; set; }

        public bool Checked { get; set; }
    }
}