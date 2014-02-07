// --------------------------------------------------------------------------
// <copyright file="ReportController.WinLossByLeague.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Web.ActionFilter;

    /// <summary>
    /// WinLossByLeagueController class.
    /// </summary>
    public partial class ReportController
    {
        #region Action
       
        /// <summary>
        /// Win Loss By Product - Level 0.
        /// </summary>
        /// <param name="metaModel">The level 0 MetaModel.</param>
        /// <returns>Process Request.</returns>
        [HttpPost]        
        public dynamic WinLossByLeague([FromBody] WinLossByLeagueMetaModel metaModel)
        {                     
            return ProcessRequest(metaModel);
        }

        #endregion Action  

        #region ProcessRequest
        
        /// <summary>
        /// Win Loss By Product - Export excel.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>        
        /// <returns>Http Response Message.</returns>
        [HttpGet]        
        public HttpResponseMessage WinLossByLeagueExcel([FromUri] WinLossByLeagueMetaModel metaModel)
        {
            if (!IsValidRequest(metaModel))
            {
                RedirectToAccessDenied();
            }

            metaModel.PageIndex = 1;
            metaModel.PageSize = int.MaxValue - 1;
            var totalrows = 0;
            var dataList = GetData(metaModel, ref totalrows);
            const string FileName = "WinLossByLeague.xls";
            var templatePath = HttpContext.Current.Server.MapPath("~/App_Data/Template/Reports/WinLossByLeagues.xls");
            var licenseeSession = SessionService.LicenseeSession;           

            var currencyList = SessionService.UserSession.CurrencyList;
            var currencyName = currencyList.SingleOrDefault(i => i.Value == (metaModel.Level == 0 ? metaModel.CurrencyId : metaModel.CurrencyId));

            var convertingOption = new ConvertingOption
            {
                IsConvert = false,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = currencyName.Key
            };

            var viewModel = new WinLossByLeagueViewModel
            {
                MainResult = dataList                
            };

            var totalData = CalculateTotal(dataList);
            var winlossByLeague = new WinlossByLeagueExcel(templatePath, FileName, convertingOption, metaModel, viewModel, totalData)
            {
                Level = metaModel.Level               
            };
            winlossByLeague.Export();

            return ExcelStream(winlossByLeague.Workbook, FileName);
        }

        /// <summary>
        /// Win Loss By Product - Get report data.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="totalrows">Total rows.</param>
        /// <returns>Report data list.</returns>
        private IList<WinLossByLeague> GetData(WinLossByLeagueMetaModel metaModel, ref int totalrows)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var licenseeId = licenseeSession.LicenseeId;
            metaModel.LicenseeId = licenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;
            metaModel.MoveBetsDay = _balanceUpService.BalanceUpInfo.MoveBetsDay;
            metaModel.BalanceUpDay = _balanceUpService.BalanceUpInfo.BalanceUpDay;
            
            return metaModel.Level == 0 ? 
                    _winlossByLeagueService.GetWinlossByLeagues(metaModel) :
                    _winlossByLeagueService.GetWinlossByLeagueCustomers(metaModel, ref totalrows);            
        }

        /// <summary>
        /// Check if the request is valid or not.
        /// Check if the session is valid.
        /// Check if currency and drill down currency is valid.
        /// </summary>
        /// <param name="metaModel">Meta model object.</param>
        /// <param name="currencyId">Currency Id.</param>
        /// <returns>Request is valid or not.</returns>
        private bool IsValidRequest(WinLossByLeagueMetaModel metaModel, int currencyId = 0)
        {
            if (!IsValidSession)
            {
                return false;
            }

            var currencyList = SessionService.UserSession.CurrencyList;
            var validModel = (metaModel.CurrencyId <= 0 && metaModel.CurrencyId >= -2)
                             || currencyList.Values.Contains(metaModel.CurrencyId);
            var validCurrency = (currencyId <= 0 && currencyId >= -2)
                                 || currencyList.Values.Contains(currencyId);

            return validModel && validCurrency;
        }
        
        /// <summary>
        ///  Process request.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>        
        /// <returns>Dynamic type.</returns>
        private dynamic ProcessRequest(WinLossByLeagueMetaModel metaModel)
        {
            if (!IsValidRequest(metaModel))
            {
                RedirectToAccessDenied();
            }

            var totalrows = 0;
            var dataList = GetData(metaModel, ref totalrows);
            return dataList.Count > 0
                ? (dynamic)new
                {
                    TotalRows = totalrows,
                    Data = dataList,
                    Total = CalculateTotal(dataList)
                }
                : new
                {
                    TotalRows = totalrows,
                    Data = new List<LicenseeWinLossGridViewModel>(),
                    Total = new List<LicenseeWinLossGridViewModel>(),
                };
        }

        /// <summary>
        /// Get total data.
        /// </summary>
        /// <param name="dataList">List data.</param>
        /// <returns>Total data.</returns>
        private WinLossByLeague CalculateTotal(IList<WinLossByLeague> dataList)
        {
            return new WinLossByLeague()
                    {
                        BetCount = dataList.Sum(x => x.BetCount),
                        TurnOver = dataList.Sum(x => x.TurnOver),
                        CustomerWinLoss = dataList.Sum(x => x.CustomerWinLoss),
                        LicenseeWL = dataList.Sum(x => x.LicenseeWL),
                        BaCommission = dataList.Sum(x => x.BaCommission),
                        AmountDue = dataList.Sum(x => x.AmountDue)
                    };
        }
        #endregion ProcessRequest
    }
}