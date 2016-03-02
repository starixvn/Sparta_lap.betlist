// -----------------------------------------------------------------------
// <copyright file="ReportController.MicroGamingWinLoss.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Api.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;

    /// <summary>
    /// ReportController class.
    /// </summary>
    public partial class ReportController
    {
        #region Action

        [HttpPost]
        public dynamic MicroGamingWinLoss(
            MicroGamingWinLossMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;
            IDictionary<string, MicroGamingWinLoss> summaryTotal;
            SortableList<MicroGamingWinLoss> dataList;

            dataList = MicroGamingWinLossService.GetSummaryWinloss(metaModel, out summaryTotal);

            if (metaModel.IsExportExcel)
            {
                return ExportExcel(metaModel, dataList, summaryTotal);
            }

            if (dataList != null && dataList.Count > 0)
            {
                return new
                {
                    Data = DoRestructureDataMicroGaming(dataList.GroupBy(i => new { i.TypeID, i.Date })),
                    Total = MicroGamingWinLossService.TotalCalc(dataList),
                    SummaryTotal = summaryTotal
                };
            }

            return GetEmptyMicroGamingWinLossResult();
        }

        #endregion Action

        #region Private Utility

        private static IList<MicroGamingWinLoss> DoRestructureDataMicroGaming(dynamic groupData)
        {
            var newDataList = new List<MicroGamingWinLoss>();

            var index = 0;
            foreach (var item in groupData)
            {
                var rowSpanFlag = true;
                var temps = (IList<MicroGamingWinLoss>)item;
                /*processing index value*/
                var numOfItem = temps.Count();

                foreach (var col in temps)
                {
                    if (rowSpanFlag && numOfItem > 1)
                    {
                        index++;
                        col.RowSpan = numOfItem;
                        rowSpanFlag = false;
                    }
                    else if (numOfItem > 1)
                    {
                        col.RowSpan = -1;
                    }
                    else
                    {
                        index++;
                        col.RowSpan = 0;
                    }

                    col.Index = index;
                    newDataList.Add(col);
                }
            }

            return newDataList;
        }

        private static dynamic GetEmptyMicroGamingWinLossResult()
        {
            return new
            {
                Data = new List<ThirdPartyWinLoss>(),
                Total = new List<ThirdPartyWinLoss>(),
                SummaryTotal = new Dictionary<string, ThirdPartyWinLoss>()
            };
        }

        private HttpResponseMessage ExportExcel(
            MicroGamingWinLossMetaModel metaModel,
            IList<MicroGamingWinLoss> dataList,
            IDictionary<string, MicroGamingWinLoss> summaryTotal)
        {
            var userSession = SessionService.UserSession;
            var templatePath = HttpContext.Current.Server.MapPath(@"~/App_Data/Template/Reports/MicroGamingWinLoss.xls");
            var currency = userSession.CurrencyList.SingleOrDefault(i => i.Value == metaModel.CurrencyId);

            var exportedFileName = string.Format(
                "MGCWinLoss_{0:ddMMMyyyy}_{1}.xls", 
                metaModel.FromDate, 
                currency.Value == 0 ? "All" : currency.Key);
            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = currency.Key
            };
        
            var excelModel = new MicroGamingWinLossViewModel
            {
                MainResult = dataList,
                SummaryTotal = summaryTotal,
            };

            var excel = new MicroGamingWinLossExcel(templatePath, exportedFileName, convertingOption, metaModel, excelModel);
            
            excel.Export();

            return ExcelStream(excel.Workbook, exportedFileName);
        }

        #endregion Private Utility
    }
}
