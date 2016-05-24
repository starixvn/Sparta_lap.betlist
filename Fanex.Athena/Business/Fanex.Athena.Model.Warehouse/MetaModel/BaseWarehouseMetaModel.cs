// -----------------------------------------------------------------------
// <copyright file="BaseWarehouseMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Warehouse.MetaModel
{
    using System;
    using Model.Core.MetaModel;

    /// <summary>
    /// BaseWarehouseMetaModel class.
    /// </summary>
    public abstract class BaseWarehouseMetaModel : BaseMetaModel
    {
        protected BaseWarehouseMetaModel()
        {
            FromDate = DateTime.Today;
            ToDate = DateTime.Today;
        }

        public bool IsExportExcel { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public virtual bool IsValid
        {
            get
            {
                return FromDate <= ToDate;
            }
        }
    }
}