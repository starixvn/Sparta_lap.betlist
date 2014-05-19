// --------------------------------------------------------------------------
// <copyright file="WinlossByLeagueExcel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using Common.Extensions;
    using Entities;
    using Models.DTO.Report;
    using Models.MetaModel.Report;
    using Models.ViewModel;
    using Models.ViewModel.Report;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Win loss by product - Export excel.
    /// </summary>
    public class WinlossByLeagueExcel : ReportBase
    {
        private const int ColumnBetCount = 4;
        private const int ColumnBaComm = 8;
        private const int ColumnAmtDueTo = 9;              
        private const int ColumnCustomerWinLoss = 6;
        private const int ColumnLicenseeWl = 7;
        private const int ColumnMatch = 1;        
        private const int ColumnExternalId = 2;        
        private const int ColumnSystemId = 3;
        private const int ColumnTemplateCount = 13;        
        private const int ColumnTurnover = 5;
        private const int RowContentTemplate = 4;
        private const int RowContentTitleTemplate = 3;
        private const int RowNoInfoTemplate = 9;
        private const int RowTotalSolidBorder = 8;
        private const int NumOfRowLevel0 = 10;
        private const int NumOfRowLevel1 = 8;

        private readonly WinLossByLeagueMetaModel _metaModel;        
        private readonly IList<WinLossByLeague> _winlossByLeagueList;
        private readonly WinLossByLeague _totalData;

        public WinlossByLeagueExcel(
                                     string templatePath,
                                     string exportingFileName,
                                     ConvertingOption convertingOption,                 
                                     WinLossByLeagueMetaModel metaModel,
                                     WinLossByLeagueViewModel viewModel,
                                     WinLossByLeague totalData)
            : base(templatePath, exportingFileName, new ShowingOption(), convertingOption)
        {
            _metaModel = metaModel;
            _winlossByLeagueList = viewModel.MainResult;
            _totalData = totalData;
            HeaderTextList = new List<string>
            {
                "#",
                ViewResources.Match,
                ViewResources.ExternalId,
                ViewResources.SystemId,                
                ViewResources.BetCount,
                ViewResources.Turnover,
                ViewResources.CustomerW_L,                
                ViewResources.LicenseeW_L,
                ViewResources.BACommission,
                ViewResources.AmountDue,
            };
        }

        public int Level
        {
            get;
            set;
        }

        public string TitleReport
        {
            get;
            set;
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        public override void AssignData()
        {
            int numOfDataRow = _winlossByLeagueList.Count;

            ISheet sheet = Workbook.CloneSheet("Template", "Data");

            AssignHeader(sheet);
            AssignTitle(sheet);

            if (numOfDataRow == 0)
            {
                AssignNoInfoRow(sheet);
            }
            else
            {
                AssignSheetData(sheet);
            }

            ProcessUi(sheet);
        }

        /// <summary>
        /// Assign data to header section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignHeader(ISheet sheet)
        {
            IRow row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);

            for (int i = 0; i < HeaderTextList.Count; i++)
            {
                row.Cells[i].SetCellValue(HeaderTextList[i]);
            }

            row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);

            row.Cells[ColumnCustomerWinLoss].SetCellValue(ViewResources.WinLoss);                        
        }

        public override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentTitleTemplate, Level == 0 ? NumOfRowLevel0 : NumOfRowLevel1, ViewResources.NoInfo);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignSheetData(ISheet sheet)
        {
            if (Level == 1)
            {
                HideRow(sheet, 3);
            }

            int currentRow = 4;

            int rowCount = _winlossByLeagueList.Count;

            var row = sheet.CopyRow(TemplateSheet, RowContentTitleTemplate, 3);
            row.Cells[0].SetCellValue(_metaModel.LeagueName);

            for (int index = 0; index < rowCount; index++)
            {                
                WinLossByLeague winlossByLeague = _winlossByLeagueList[index];                

                IRow rowContent = sheet.CopyRow(TemplateSheet, RowContentTemplate, currentRow);

                rowContent.Cells[ColumnIndex].SetCellValue(winlossByLeague.Index);
                if (Level == 0)
                {
                    rowContent.Cells[ColumnMatch].SetCellValue(winlossByLeague.HomeName + Environment.NewLine + winlossByLeague.AwayName);
                    rowContent.Cells[ColumnAmtDueTo].SetCellValue(winlossByLeague.AmountDue);
                    rowContent.Cells[ColumnBaComm].SetCellValue(winlossByLeague.BaCommission);                    
                }
                else
                {
                    rowContent.Cells[ColumnExternalId].SetCellValue(winlossByLeague.ExternalId);
                    rowContent.Cells[ColumnSystemId].SetCellValue(winlossByLeague.SystemId);
                }

                AssignCommonData(rowContent, winlossByLeague);

                currentRow++;
            }
                        
            AssignTotalData(sheet, currentRow);            
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignTitle(ISheet sheet)
        {           
            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, Level == 0 ? NumOfRowLevel0 : NumOfRowLevel1, _metaModel.ReportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 4);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        /// <summary>
        /// Process UI, hide redundant row and column.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void ProcessUi(ISheet sheet)
        {          
            switch (Level)
            {
                case 0:
                    HideColumn(sheet, ColumnExternalId, ColumnSystemId);                    
                    break;

                case 1:
                    HideColumn(sheet, ColumnMatch, ColumnAmtDueTo);                    
                    break;
            }
        }

        protected static void AssignCommonData(IRow row, WinLossByLeague winlossByLeague)
        {
            row.Cells[ColumnTurnover].SetCellValue(winlossByLeague.TurnOver);
            row.Cells[ColumnBetCount].SetCellValue(winlossByLeague.BetCount);
            row.Cells[ColumnCustomerWinLoss].SetCellValue(winlossByLeague.CustomerWinLoss);            
            row.Cells[ColumnLicenseeWl].SetCellValue(winlossByLeague.LicenseeWL);
            row.Cells[ColumnBaComm].SetCellValue(winlossByLeague.BaCommission);     
        }

        /// <summary>
        /// Assign data to Total section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="currentRow">Excel row index.</param>
        /// <returns>The row index after assign data.</returns>
        protected int AssignTotalData(ISheet sheet, int currentRow)
        {
            int rowTemplate = RowTotalSolidBorder;
            sheet.MergeCells(currentRow, 0, currentRow, 1);
            IRow row = sheet.CopyRow(TemplateSheet, rowTemplate, currentRow);                        
            AssignCommonData(row, _totalData);
            row.Cells[ColumnAmtDueTo].SetCellValue(_totalData.AmountDue);
            row.Cells[ColumnBaComm].SetCellValue(_totalData.BaCommission);
            return currentRow;          
        }
    }
}