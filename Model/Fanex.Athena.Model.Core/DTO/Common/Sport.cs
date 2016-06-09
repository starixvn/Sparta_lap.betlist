// --------------------------------------------------------------------------
// <copyright file="Sport.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DTO.Common
{
    using Fanex.Athena.Model.Core.Helper;

    /// <summary>
    /// Sport class.
    /// </summary>
    public class Sport
    {
        private string _sportName;

        public Sport(string id)
        {
            SportId = id;
        }

        public Sport(string id, string name)
        {
            SportId = id;
            SportName = name;
        }

        public string SportId { get; set; }

        public string SportName
        {
            get
            {
                if (string.IsNullOrEmpty(_sportName))
                {
                    _sportName = CacheRef.GetSportName(SportId);
                }

                return _sportName;
            }

            set
            {
                _sportName = value;
            }
        }
    }
}