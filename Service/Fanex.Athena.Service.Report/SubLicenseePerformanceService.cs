namespace Fanex.Athena.Service.Report
{
    using System;
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Configuration.Interfaces;
    using DataAccess.Report;
    using DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report.Interfaces;

    public class SubLicenseePerformanceService : ISubLicenseePerformanceService
    {
        private readonly IBalanceUpService _balanceUpService;

        private readonly ISubLicenseePerformanceDataAccess _subLicenseePerformanceDataAccess;

        public SubLicenseePerformanceService()
        {
            _balanceUpService = new BalanceUpService();
            _subLicenseePerformanceDataAccess = new SubLicenseePerformanceDataAccess(_balanceUpService.BalanceUpInfo);
        }

        public SubLicenseePerformanceService(IBalanceUpService balanceUpService, ISubLicenseePerformanceDataAccess subLicenseePerformanceDataAccess)
        {
            _balanceUpService = balanceUpService;
            _subLicenseePerformanceDataAccess = subLicenseePerformanceDataAccess;
        }

        public IEnumerable<SubLicenseePerformance> GetData(SubLicenseePerformanceMetaModel subLicenseePerformanceMetaModel)
        {
            throw new NotImplementedException();
        }
    }
}