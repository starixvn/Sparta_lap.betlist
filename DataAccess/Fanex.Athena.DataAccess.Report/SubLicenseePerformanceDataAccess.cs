namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using Core;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;
    using Model.Report.MetaModel;

    public class SubLicenseePerformanceDataAccess : BaseDataAccess, ISubLicenseePerformanceDataAccess
    {
        public SubLicenseePerformanceDataAccess(BalanceUp balanceUp) : base(balanceUp)
        {
        }

        IEnumerable<SubLicenseePerformance> ISubLicenseePerformanceDataAccess.GetData(SubLicenseePerformanceMetaModel subLicenseePerformanceMetaModel)
        {
            throw new NotImplementedException();
        }
    }
}