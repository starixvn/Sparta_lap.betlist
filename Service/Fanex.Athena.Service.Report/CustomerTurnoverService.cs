// -----------------------------------------------------------------------
// <copyright file="CustomerTurnoverService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Core.ResultModel;

    public class CustomerTurnoverService : ICustomerTurnoverService
    {
        private readonly ICustTurnoverDataAccess _custTurnoverDataAccess;

        public CustomerTurnoverService()
        {
            _custTurnoverDataAccess = new CustomerTurnoverDataAccess();
        }

        public ReportResultModel<CustTurnover> GetDataList(CustomerTurnoverMetaModel metaModel, string languageCode)
        {
            if (!metaModel.IsValid)
            {
                return new ReportResultModel<CustTurnover>
                {
                    DataList = new List<CustTurnover>(),
                    TotalData = new CustTurnover()
                };
            }

            var result = _custTurnoverDataAccess.GetDataList(metaModel, languageCode);

            if (result.DataList.Any())
            {
                var rowIndex = (metaModel.PageIndex - 1) * metaModel.PageSize;
                result.DataList = RestructureData(result.DataList, rowIndex);
            }

            return result;
        }

        [SuppressMessage(
            "StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static IEnumerable<CustTurnover> RestructureData(IEnumerable<CustTurnover> datalist, int startRowIndex)
        {
            var sortedData = datalist.OrderByDescending(i => i.ReportBy).ThenBy(i => i.UserName)
                .ThenBy(i => i.SportType).ThenBy(i => i.Bettype).ToArray();

            var newDataList = new List<CustTurnover>();

            var rowIndex = startRowIndex;
            var groupData = sortedData.GroupBy(i => new { i.ReportBy, i.UserName, i.Currency });

            foreach (var item in groupData)
            {
                var rowSpanFlag = true;
                var numOfItem = item.Count();
                var groupSportdata = item.GroupBy(i => i.SportType);
                var totalTurnover = item.Sum(i => i.Turnover);
                foreach (var sports in groupSportdata)
                {
                    var rowSpan2Flag = true;
                    var numOfSportItem = sports.Count();

                    /*processing index value*/
                    foreach (var col in sports)
                    {
                        /* processing rowspan*/
                        var temp = col;
                        if (rowSpanFlag && numOfItem > 1)
                        {
                            rowIndex++;
                            temp.RowSpan = numOfItem;
                            rowSpanFlag = false;
                        }
                        else if (numOfItem > 1)
                        {
                            temp.RowSpan = -1;
                        }
                        else
                        {
                            rowIndex++;
                            temp.RowSpan = 0;
                        }

                        temp.Index = rowIndex;
                        /*processing rowspan2*/
                        if (rowSpan2Flag && numOfSportItem > 1)
                        {
                            temp.RowSpan2 = numOfSportItem;
                            rowSpan2Flag = false;
                        }
                        else if (numOfSportItem > 1)
                        {
                            temp.RowSpan2 = -1;
                        }
                        else
                        {
                            temp.RowSpan2 = 0;
                        }

                        /*update totalturnover column*/
                        temp.TotalTurnover = totalTurnover;
                        newDataList.Add(temp);
                    }
                }
            }

            return newDataList;
        }
    }
}