// --------------------------------------------------------------------------
// <copyright file="ConvertingOption.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Excel.Base.Entities
{
    /// <summary>
    /// Converting option.
    /// </summary>
    public class ConvertingOption
    {
        public ConvertingOption()
        {
            IsConvert = true;
            BaseCurrency = @"EUR";
        }

        public string BaseCurrency
        {
            get;
            set;
        }

        public string CurrencyName
        {
            get;
            set;
        }

        public bool IsConvert
        {
            get;
            set;
        }
    }
}