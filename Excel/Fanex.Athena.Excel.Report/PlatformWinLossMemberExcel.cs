// --------------------------------------------------------------------------
// <copyright file="PlatformWinLossMemberExcel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using Base.Entities;
    using Fanex.Athena.Excel.Base.Extension;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using Resources;

    public class PlatformWinLossMemberExcel : ReportBase
    {
        private const int ColumnExternalId = 1;
        private const int ColumnSystemId = 2;
        private const int ColumnCurrency = 3;
        private const int ColumnTurnover = 4;
        private const int ColumnTotalTrans = 5;
        private const int ColumnCustomerWl = 6;
        private const int ColumnCustomerCommission = 7;
        private const int ColumnCustomerTotal = 8;
        private const int ColumnLicenseeWl = 9;
        private const int ColumnBaCommission = 10;
        private const int ColumnAmtDueToBa = 11;
        private const int ColumnTemplateCount = 12;

        private const int RowContentTemplate = 3;
        private const int RowNoInfoTemplate = 5;
        private const int RowTotalTemplate = 4;

        private readonly IEnumerable<PlatformWinLossMember> _dataList;
        private readonly PlatformWinLossViewModel _model;
        private readonly PlatformWinLossMember _totalData;

        private readonly IEnumerable<ProductOption> _productOptions;

        public PlatformWinLossMemberExcel(
                        string templatePath,
                                  PlatformWinLossViewModel model,
                                  ShowingOption showingOption)
            : base(templatePath, model.MainResult.Count(), showingOption)
        {
            _productOptions = model.Param.ProductOptions;
            _model = model;
            _dataList = model.MainResult;
            _totalData = model.TotalResult;
        }

        protected override void AssignHeader(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
            var headerTextList = new[]
            {
                "#",
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.Currency,
                ViewResources.Turnover,
                ViewResources.TotalTransaction,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.LicenseeW_L,
                ViewResources.BACommission,
                ViewResources.AmtDueToBA
            };

            SetTextRowHeader(row, headerTextList);

            row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);

            row.Cells[ColumnCustomerWl].SetCellValue(ViewResources.WinLoss);
            row.Cells[ColumnCustomerCommission].SetCellValue(ViewResources.Comm);
            row.Cells[ColumnCustomerTotal].SetCellValue(ViewResources.Total);
        }

        /// <summary>
        /// Assign no info row.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignNoInfoRow(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignSheetData(ISheet sheet)
        {
            var rowIndex = 0;
            int index = 1;
            foreach (PlatformWinLossMember item in _dataList)
            {
                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, rowIndex + RowContentTemplate);
                row.Cells[ColumnIndex].SetCellValue(index);
                row.Cells[ColumnCurrency].SetCellValue(item.CurrencyName);
                AssignCellData(row, item);
                rowIndex++;
                index++;
            }

            AssignTotal(sheet);
        }

        protected override void AssignTitle(ISheet sheet)
        {
            var metaModel = _model.Param;
            var platformText = ViewResources.Platform + ": " + PlatformType.GetPlatformName(metaModel.PlatformType);
            var currencyText = metaModel.CurrencyId == 0 ?
                ViewResources.AllCurrencies : $"{ViewResources.Currency} {metaModel.CurrencyName}";

            var reportTitle = string.Format(
                                         ViewResources.PlatformWinLossTitle,
                                         metaModel.FromDate.ToString(FormatDate),
                                         metaModel.ToDate.ToString(FormatDate),
                                         currencyText,
                                         GetBaseCurrencyTitle(),
                                         platformText) + ", " + GetProductTitle();

            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, reportTitle);

            sheet.CreateFreezePane(0, 3);

            ClearTemplate(sheet, RowContentTemplate);
        }

        /// <summary>
        /// Process UI, hide redundant row and column.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void ProcessUI(ISheet sheet)
        {
            if (!ShowingOption.CustomerCommVisible)
            {
                sheet.HideColumn(ColumnCustomerTotal, ColumnCustomerCommission);
                sheet.HideRow(RowHeaderTemplate + 1);
            }

            if (!ShowingOption.BAVisible)
            {
                sheet.HideColumn(ColumnBaCommission, ColumnAmtDueToBa);
            }
        }

        private static void AssignCellData(IRow row, PlatformWinLossMember item)
        {
            row.Cells[ColumnExternalId].SetCellValue(item.ExternalId);
            row.Cells[ColumnSystemId].SetCellValue(item.SystemId);
            row.Cells[ColumnTurnover].SetCellValue(item.Turnover);
            row.Cells[ColumnTotalTrans].SetCellValue(item.TotalTransactions);
            row.Cells[ColumnCustomerWl].SetCellValue(item.CustomerWinLoss);
            row.Cells[ColumnCustomerCommission].SetCellValue(item.CustomerComm);
            row.Cells[ColumnCustomerTotal].SetCellValue(item.CustomerTotal);
            row.Cells[ColumnLicenseeWl].SetCellValue(item.LicenseeWL);
            row.Cells[ColumnBaCommission].SetCellValue(item.BACommission);
            row.Cells[ColumnAmtDueToBa].SetCellValue(item.AmtDueToBA);
        }

        protected internal virtual void AssignTotal(ISheet sheet)
        {
            var totalRow = _dataList.Count() + RowContentTemplate;

            var row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, totalRow);
            sheet.MergeCells(totalRow, ColumnIndex, totalRow, ColumnCurrency);
            row.Cells[ColumnIndex].SetCellValue(ViewResources.Total);
            AssignCellData(row, _totalData);
        }

        private string GetProductTitle()
        {
            if (_productOptions.Count() == 1)
            {
                return $"{ViewResources.Product}: {_productOptions.First().Code}";
            }

            var selectedProducts = _productOptions.Where(x => x.Status);
            if (selectedProducts.Count() == _productOptions.Count())
            {
                return $"{ViewResources.Product}: {ViewResources.All}";
            }

            var text = selectedProducts
                .Aggregate(string.Empty, (current, x) => current + (string.IsNullOrEmpty(current) ? x.Code : ", " + x.Code));

            return $"{ViewResources.Product}: {text}";
        }

        private string GetBaseCurrencyTitle()
        {
            string title;
            var baseCurrencyId = _model.Param.BaseCurrencyId;
            switch (baseCurrencyId)
            {
                case BaseCurrencyType.Eur:
                    title = string.Format(ViewResources.Viewin, FormatKey.Eur);
                    break;

                case BaseCurrencyType.Usd:
                    title = string.Format(ViewResources.Viewin, FormatKey.Usd);
                    break;

                case BaseCurrencyType.Gbp:
                    title = string.Format(ViewResources.Viewin, FormatKey.Gbp);
                    break;

                default:
                    title = string.Format(ViewResources.Viewin, _model.Param.CurrencyName);
                    break;
            }

            return title;
        }
    }
}