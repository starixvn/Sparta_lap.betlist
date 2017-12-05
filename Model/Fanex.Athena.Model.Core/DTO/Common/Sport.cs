// --------------------------------------------------------------------------
// <copyright file="Sport.cs" company="">
// Copyright (c) . All rights reserved.
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

        public Sport(int id)
        {
            SportId = id;
        }

        public Sport(int id, string name)
        {
            SportId = id;
            SportName = name;
        }

        public int SportId { get; set; }

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