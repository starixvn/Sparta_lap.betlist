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

    public class SubsidiaryPerformanceService : ISubsidiaryPerformanceService
    {
        private readonly IBalanceUpService _balanceUpService;

        private readonly ISubsidiaryPerformanceDataAccess _subLicenseePerformanceDataAccess;

        public SubsidiaryPerformanceService()
        {
            _balanceUpService = new BalanceUpService();
            _subLicenseePerformanceDataAccess = new SubsidiaryPerformanceDataAccess(_balanceUpService.BalanceUpInfo);
        }

        public SubsidiaryPerformanceService(IBalanceUpService balanceUpService, ISubsidiaryPerformanceDataAccess subLicenseePerformanceDataAccess)
        {
            _balanceUpService = balanceUpService;
            _subLicenseePerformanceDataAccess = subLicenseePerformanceDataAccess;
        }

        public IEnumerable<SubsidiaryPerformance> GetData(SubsidiaryPerformanceMetaModel subLicenseePerformanceMetaModel)
        {
            throw new NotImplementedException();
        }
    }
}