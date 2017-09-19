// --------------------------------------------------------------------------
// <copyright file="CustomerBalanceExcel.cs" company="">
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
    using Helpers;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NPOI.SS.UserModel;
    using Resources;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class CustomerBalanceExcel : ReportBase
    {
        private const int ColumnAdjustment = 7;
        private const int ColumnAffiliateTransfer = 16;
        private const int ColumnAmountDueToBa = 19;
        private const int ColumnBonus = 6;
        private const int ColumnClosingBalance = 15;
        private const int ColumnCommBa = 18;
        private const int ColumnCustomerComm = 13;
        private const int ColumnCustomerTotal = 14;
        private const int ColumnCustomerWinLoss = 12;
        private const int ColumnDeposit = 4;
        private const int ColumnDrawGame = 9;
        private const int ColumnExternalId = 1;
        private const int ColumnNetTurnOver = 10;
        private const int ColumnOpeningBalance = 3;
        private const int ColumnPendingFunds = 11;
        private const int ColumnSystemId = 2;
        private const int ColumnTemplateCount = 20;
        private const int ColumnTurnOver = 8;
        private const int ColumnTurnoverBa = 17;
        private const int ColumnWithdrawal = 5;
        private const int RowContentTemplate = 3;
        private const int RowNoInfoTemplate = 5;
        private const int RowTableHeaderTemplate = 1;
        private const int RowTotalTemplate = 4;

        private readonly IEnumerable<CustBalance> _exportData;
        private readonly CustomerBalanceMetaModel _model;
        private readonly CustBalance _totalList;

        private readonly ConvertingOption _convertingOption;

        public CustomerBalanceExcel(
                                    string templatePath,
                                    CustomerBalanceMetaModel model,
                                    IEnumerable<CustBalance> exportData,
                                    CustBalance totalList,
                                    ShowingOption showingOption,
                                    ConvertingOption convertingOption)
            : base(templatePath, showingOption)
        {
            _convertingOption = convertingOption;
            _model = model;
            _exportData = exportData;
            _totalList = totalList;
            HeaderTextList = new List<string>
            {
                "#",
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.OpeningBalance,
                ViewResources.Deposit,
                ViewResources.Withdrawal,
                ViewResources.Bonus,
                ViewResources.Adjustment,
                ViewResources.Turnover,
                ViewResources.DrawGame,
                ViewResources.NetTurnover,
                ViewResources.PendingFunds,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.ClosingBalance,
                ViewResources.AffiliateTransfer,
                ViewResources.ProductBA
            };
        }

        protected override void AssignData()
        {
            int numOfDataRow = _exportData.Count();

            if (numOfDataRow == 0)
            {
                ISheet sheet = Workbook.CloneSheet("Template", "Data");
                AssignTitle(sheet);
                AssignHeader(sheet);
                AssignNoInfoRow(sheet);
                ProcessUI(sheet);
            }
            else
            {
                int sheetCount = numOfDataRow % MaxRowCount == 0
                                 ? numOfDataRow / MaxRowCount
                                 : (numOfDataRow / MaxRowCount) + 1;
                var rowIndex = 0;
                for (int i = 0; i < sheetCount; i++)
                {
                    var sheet = Workbook.CloneSheet("Template", "Customer Balance" + (i + 1));
                    AssignTitle(sheet);
                    AssignHeader(sheet);
                    rowIndex = AssignSheetData(sheet, i);

                    if (i == sheetCount - 1)
                    {
                        AssignTotalData(sheet, rowIndex + RowContentTemplate);
                    }

                    ProcessUI(sheet);
                }
            }
        }

        protected override void AssignHeader(ISheet sheet)
        {
            IRow row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
            IRow row2 = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);

            for (int i = 0; i < HeaderTextList.Count; i++)
            {
                row.Cells[i].SetCellValue(HeaderTextList[i]);
            }

            row2.Cells[ColumnCustomerWinLoss].SetCellValue(ViewResources.WinLoss);
            row2.Cells[ColumnCustomerComm].SetCellValue(ViewResources.Comm);
            row2.Cells[ColumnCustomerTotal].SetCellValue(ViewResources.Total);

            row2.Cells[ColumnTurnoverBa].SetCellValue(ViewResources.Turnover);
            row2.Cells[ColumnCommBa].SetCellValue(ViewResources.Comm);
            row2.Cells[ColumnAmountDueToBa].SetCellValue(ViewResources.AmtDueTo);
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        protected override int AssignSheetData(ISheet sheet, int pageIndex)
        {
            int rowCount = _exportData.Count();
            int startIndex = checked(pageIndex * MaxRowCount);
            int endIndex = startIndex + MaxRowCount > rowCount ? rowCount : startIndex + MaxRowCount;
            int rowIndex = 0;

            for (int index = startIndex; index < endIndex; index++)
            {
                IRow row = sheet.CopyRow(TemplateSheet, RowContentTemplate, rowIndex + RowContentTemplate);
                var item = _exportData.ElementAt(index);

                row.Cells[ColumnIndex].SetCellValue(index + 1);
                row.Cells[ColumnExternalId].SetCellValue(item.ExternalId);
                row.Cells[ColumnSystemId].SetCellValue(item.SystemId);
                row.Cells[ColumnOpeningBalance].SetCellValue(item.OpeningBalance);
                row.Cells[ColumnDeposit].SetCellValue(item.Deposit);
                row.Cells[ColumnWithdrawal].SetCellValue(item.Withdrawal);
                row.Cells[ColumnBonus].SetCellValue(item.Bonus);
                row.Cells[ColumnAdjustment].SetCellValue(item.Adjustment);
                row.Cells[ColumnTurnOver].SetCellValue(item.Turnover);
                row.Cells[ColumnDrawGame].SetCellValue(item.DrawGame);
                row.Cells[ColumnNetTurnOver].SetCellValue(item.NetTurnover);
                row.Cells[ColumnPendingFunds].SetCellValue(item.PendingFunds);
                row.Cells[ColumnCustomerWinLoss].SetCellValue(item.WinLoss);
                row.Cells[ColumnCustomerComm].SetCellValue(item.PlayerComm);
                row.Cells[ColumnCustomerTotal].SetCellValue(item.WinLoss + item.PlayerComm);
                row.Cells[ColumnClosingBalance].SetCellValue(item.ClosingBalance);
                row.Cells[ColumnAffiliateTransfer].SetCellValue(item.AffiliateTransfer);
                row.Cells[ColumnTurnoverBa].SetCellValue(item.TurnoverBA);
                row.Cells[ColumnCommBa].SetCellValue(item.CommBA);
                row.Cells[ColumnAmountDueToBa].SetCellValue(item.AmountDueToBA);

                rowIndex++;
            }

            return rowIndex;
        }

        protected override void AssignTitle(ISheet sheet)
        {
            string pattern = ViewResources.CustomerBalance + ", {0} - {1}, {2}";
            ReportTitle = string.Format(
                                        pattern,
                                        _model.FromDate.ToString("MM/dd/yyyy"),
                                        _model.ToDate.ToString("MM/dd/yyyy"),
                                        ReportExcelHelper.GetTitleCurrencyName(_convertingOption));

            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        protected override void ProcessUI(ISheet sheet)
        {
            if (!ShowingOption.CustomerCommVisible)
            {
                sheet.HideColumn(ColumnCustomerTotal, ColumnCustomerComm);

                // If row isn't hide (in case BA visible), then merge cell and set value for header
                sheet.MergeCells(RowTableHeaderTemplate, ColumnCustomerWinLoss, RowTableHeaderTemplate + 1, ColumnCustomerWinLoss);
                sheet.GetCell(RowTableHeaderTemplate, ColumnCustomerWinLoss).SetCellValue(ViewResources.WinLoss);
            }

            if (!ShowingOption.BAVisible)
            {
                sheet.HideColumn(ColumnTurnoverBa, ColumnCommBa, ColumnAmountDueToBa);
            }

            if (!ShowingOption.CustomerCommVisible && !ShowingOption.BAVisible)
            {
                sheet.HideRow(RowTableHeaderTemplate + 1);
            }
        }

        private void AssignTotalData(ISheet sheet, int rowTotal)
        {
            IRow row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, rowTotal);

            sheet.MergeCells(rowTotal, ColumnIndex, rowTotal, ColumnSystemId);
            row.Cells[ColumnOpeningBalance].SetCellValue(_totalList.OpeningBalance);
            row.Cells[ColumnDeposit].SetCellValue(_totalList.Deposit);
            row.Cells[ColumnWithdrawal].SetCellValue(_totalList.Withdrawal);
            row.Cells[ColumnBonus].SetCellValue(_totalList.Bonus);
            row.Cells[ColumnAdjustment].SetCellValue(_totalList.Adjustment);
            row.Cells[ColumnTurnOver].SetCellValue(_totalList.Turnover);
            row.Cells[ColumnDrawGame].SetCellValue(_totalList.DrawGame);
            row.Cells[ColumnNetTurnOver].SetCellValue(_totalList.NetTurnover);
            row.Cells[ColumnPendingFunds].SetCellValue(_totalList.PendingFunds);
            row.Cells[ColumnCustomerWinLoss].SetCellValue(_totalList.WinLoss);
            row.Cells[ColumnCustomerComm].SetCellValue(_totalList.PlayerComm);
            row.Cells[ColumnCustomerTotal].SetCellValue(_totalList.WinLoss + _totalList.PlayerComm);
            row.Cells[ColumnClosingBalance].SetCellValue(_totalList.ClosingBalance);
            row.Cells[ColumnAffiliateTransfer].SetCellValue(_totalList.AffiliateTransfer);
            row.Cells[ColumnTurnoverBa].SetCellValue(_totalList.TurnoverBA);
            row.Cells[ColumnCommBa].SetCellValue(_totalList.CommBA);
            row.Cells[ColumnAmountDueToBa].SetCellValue(_totalList.AmountDueToBA);
        }
    }
}