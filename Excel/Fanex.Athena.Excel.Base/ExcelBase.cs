// --------------------------------------------------------------------------
// <copyright file="ExcelBase.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Base
{
    using Entities;
    using Extension;
    using NPOI.SS.UserModel;

    public abstract class ExcelBase
    {
        protected const int ColumnIndex = 0;

        protected const string FormatDate = "MM/dd/yyyy";

        protected ShowingOption ShowingOption
        {
            get;
            set;
        }
    }
}