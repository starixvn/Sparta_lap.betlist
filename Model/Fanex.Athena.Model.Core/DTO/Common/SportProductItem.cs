// --------------------------------------------------------------------------
// <copyright file="SportProductItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (coffee) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Alex.Nguyen" date="20131009">Created</change>
// </history>

namespace Fanex.Athena.Model.Core.DTO.Common
{
    /// <summary>
    /// Sport product item class.
    /// Used for Multiple selected drop down list.
    /// Product id of sport book is "0".
    /// </summary>
    public class SportProductItem
    {
        public SportProductItem()
        {
            this.ID = -1;
            this.ParentId = -1; /*in case it's a product*/
        }

        public int ID { get; set; }

        public int ParentId { get; set; }

        public string Name { get; set; }

        public bool Checked { get; set; }
    }
}