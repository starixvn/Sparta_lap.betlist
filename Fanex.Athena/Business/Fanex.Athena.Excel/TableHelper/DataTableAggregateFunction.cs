// -----------------------------------------------------------------------
// <copyright file="DataTableAggregateFunction.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.Athena.Excel.TableHelper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public enum AggregateFunction
    {
        Sum,
        Avg,
        Count,
        Max,
        Min
    }

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DataTableAggregateFunction
    {
        /// <summary>
        /// Performed for which column
        /// </summary>
        public string ColumnName
        {
            get;
            set;
        }

        /// <summary>
        /// The function to be performed
        /// </summary>
        public AggregateFunction enmFunction
        {
            get;
            set;
        }

        /// <summary>
        /// What should be the name after output
        /// </summary>
        public string OutPutColumnName
        {
            get;
            set;
        }
    }
}