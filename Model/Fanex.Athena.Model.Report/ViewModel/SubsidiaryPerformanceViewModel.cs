namespace Fanex.Athena.Model.Report.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.DataDomain;
    using Core.DTO.Common;
    using Core.MetaModel.Product;
    using Service.Core.Users;
    using Service.Core.Users.Interfaces;

    public class SubsidiaryPerformanceViewModel
    {
        private readonly ISessionService _sessionService;

        public SubsidiaryPerformanceViewModel()
        {
            _sessionService = new SessionService();
            FromDate = DateTime.Today;
            ToDate = DateTime.Today;
        }

        public IEnumerable<CheckedItem> SubsidiaryList
        {
            get
            {
                var agentList = _sessionService.UserSession.CurrencyAgentList.SelectMany(x => x.Value).Distinct();

                if (agentList.Any())
                {
                    var subsidiaryList = agentList.Select(x => new CheckedItem
                    {
                        ID = x,
                        Name = x.ToString(),
                        Checked = false
                    }).ToList();

                    subsidiaryList[0].Checked = true;

                    return subsidiaryList;
                }

                return new List<CheckedItem>();
            }
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public IEnumerable<ProductOption> ProductOptionList { get; set; }

        public IEnumerable<string> BaseCurrencyList => SystemConstants.BaseCurrencyList;

        public bool ConvertAll { get; set; }
    }
}