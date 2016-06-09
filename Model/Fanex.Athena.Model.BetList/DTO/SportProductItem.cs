// --------------------------------------------------------------------------
// <copyright file="SportProductItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (coffee) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Alex.Nguyen" date="20131009">Created</change>
// </history>

namespace Fanex.Athena.Models.DTO
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Sport product item class.
    /// Used for Multiple selected drop down list.
    /// </summary>
    public class SportProductItem
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int ID { get; set; }

        [Required]
        public bool Checked { get; set; }

        public IList<CheckBoxItem> Sports { get; set; }
    }
}