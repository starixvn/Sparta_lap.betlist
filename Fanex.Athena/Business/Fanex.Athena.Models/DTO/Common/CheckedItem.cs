// --------------------------------------------------------------------------
// <copyright file="CheckedItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (coffee) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Alex.Nguyen" date="">Created</change>
// </history>

namespace Fanex.Athena.Models.DTO.Common
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