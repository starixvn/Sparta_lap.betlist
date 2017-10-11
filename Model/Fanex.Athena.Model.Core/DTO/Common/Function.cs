// --------------------------------------------------------------------------
// <copyright file="Function.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DTO.Common
{
    using System;
    using Resources;

    /// <summary>
    /// Function class.
    /// </summary>
    [Serializable]
    public class Function
    {
        public int FunctionId { get; set; }

        public string Name
        {
            get
            {
                return Menu.ResourceManager.GetString("Menu_" + FunctionId);
            }
        }

        public string Description { get; set; }

        public int ParentId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S3996:UriPropertiesShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        public string Url { get; set; }

        public bool IsMenu { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsNew { get; set; }

        public string FunctionKey { get; set; }

        public bool IsFunction { get; set; }
    }
}