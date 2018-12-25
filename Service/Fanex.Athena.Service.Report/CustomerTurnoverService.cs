// -----------------------------------------------------------------------
// <copyright file="CustomerTurnoverService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Core.ResultModel;

    public class CustomerTurnoverService : ICustomerTurnoverService
    {
        private readonly ICustomerTurnoverDataAccess _customerTurnoverDataAccess;

        public CustomerTurnoverService(ICustomerTurnoverDataAccess customerTurnoverDataAccess)
        {
            _customerTurnoverDataAccess = customerTurnoverDataAccess;
        }

        public ReportResultModel<CustTurnover> GetDataList(CustomerTurnoverMetaModel metaModel,
            IDictionary<int, string> currencyList, string languageCode)
        {
            if (!metaModel.IsValid)
            {
                return new ReportResultModel<CustTurnover>
                {
                    DataList = new List<CustTurnover>(),
                    TotalData = new CustTurnover()
                };
            }

            var result = _customerTurnoverDataAccess.GetDataList(metaModel, currencyList, languageCode);

            if (result.DataList.Any())
            {
                result.DataList = RestructureData(result.DataList);
            }

            return result;
        }

        private static IEnumerable<CustTurnover> RestructureData(IEnumerable<CustTurnover> datalist)
        {
            var sortedData = datalist.OrderByDescending(i => i.ReportBy).ThenBy(i => i.UserName)
                .ThenBy(i => i.SportType).ThenBy(i => i.Bettype).ToArray();

            var newDataList = new List<CustTurnover>();

            var groupData = sortedData.GroupBy(i => i.Index);

            foreach (var item in groupData)
            {
                var isFirstRowOfGroup = true;
                var numOfItem = item.Count();
                var groupSportdata = item.GroupBy(i => i.SportType);
                var totalTurnover = item.Sum(i => i.Turnover);
                foreach (var sports in groupSportdata)
                {
                    var groupedList = ProcessSportGroup(sports, isFirstRowOfGroup, numOfItem, totalTurnover);
                    isFirstRowOfGroup = false;
                    newDataList.AddRange(groupedList);
                }
            }

            return newDataList;
        }

        private static IEnumerable<CustTurnover> ProcessSportGroup(
            IGrouping<string, CustTurnover> sportGroup,
            bool isFirstRowOfGroup,
            int numOfItem,
            decimal totalTurnover)
        {
            var isFirstRowOfSportGroup = true;
            var numOfSportItem = sportGroup.Count();
            var list = new List<CustTurnover>();
            var hasRowSpan = isFirstRowOfGroup;

            foreach (var item in sportGroup)
            {
                var temp = item;
                temp.TotalTurnover = totalTurnover;

                temp.RowSpan = GetRowSpan(hasRowSpan, numOfItem);
                hasRowSpan = false;

                temp.RowSpan2 = GetRowSpan(isFirstRowOfSportGroup, numOfSportItem);
                isFirstRowOfSportGroup = false;

                list.Add(temp);
            }

            return list;
        }

        private static int GetRowSpan(bool hasRowSpan, int numOfItems)
        {
            if (hasRowSpan && numOfItems > 1)
            {
                return numOfItems;
            }
            else if (numOfItems > 1)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}