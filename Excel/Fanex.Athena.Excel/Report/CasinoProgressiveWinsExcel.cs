// --------------------------------------------------------------------------
// <copyright file="CasinoProgressiveWinsExcel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using Base.Entities;
    using Fanex.Athena.Excel.Base.Extension;
    using Fanex.Athena.Model.Core.Helper;
    using Models.DTO.Report;
    using Models.MetaModel.Report;
    using Models.ViewModel.Report;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using Resources;

    public class CasinoProgressiveWinsExcel : ReportBase
    {
        private const int ColumnDate = 1;
        private const int ColumnExternalId = 2;
        private const int ColumnSystemId = 3;
        private const int ColumnRefNo = 4;
        private const int ColumnChoice = 5;
        private const int ColumnAmount = 6;

        private const int ColumnTemplateCount = 7;
        private const int RowContentTemplate = 2;
        private const int RowNoInfoTemplate = 3;

        private readonly IEnumerable<CasinoProgressiveWins> _dataList;

        private readonly CasinoProgressiveWinsMetaModel _metaModel;

        private IFont _boldBlackFont;
        private IFont _boldBlueFont;

        public CasinoProgressiveWinsExcel(
                string templatePath,
                string exportingFileName,
                ConvertingOption convertingOption,
                CasinoProgressiveWinsMetaModel metaModel,
                CasinoProgressiveWinsViewModel viewModel)
            : base(templatePath, exportingFileName, null, convertingOption)
        {
            _metaModel = metaModel;
            _dataList = viewModel.DataList;
        }

        public override void AssignData()
        {
            ISheet sheet = Workbook.CloneSheet("Template", "Data");

            AssignHeader(sheet);
            AssignTitle(sheet);

            if (_dataList.Any())
            {
                AssignSheetData(sheet);
            }
            else
            {
                AssignNoInfoRow(sheet);
            }

            ProcessUI(sheet);
        }

        public override void AssignHeader(ISheet sheet)
        {
            IRow row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
            row.Cells[ColumnIndex].SetCellValue("#");
            row.Cells[ColumnDate].SetCellValue(ViewResources.Date);
            row.Cells[ColumnExternalId].SetCellValue(ViewResources.ExternalId);
            row.Cells[ColumnSystemId].SetCellValue(ViewResources.SystemId);
            row.Cells[ColumnRefNo].SetCellValue(ViewResources.ReferenceNo);
            row.Cells[ColumnChoice].SetCellValue(ViewResources.Choice);
            row.Cells[ColumnAmount].SetCellValue(ViewResources.Amount);
        }

        public override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUI(sheet);
        }

        public override void AssignSheetData(ISheet sheet)
        {
            InitializeFonts();

            int index = 0;

            foreach (var item in _dataList)
            {
                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + RowContentTemplate);
                AssignCommonData(row, item);
                index++;
            }
        }

        public override void AssignTitle(ISheet sheet)
        {
            var fromdate = _metaModel.FromDate.ToString(FormatDate);
            var todate = _metaModel.ToDate.ToString(FormatDate);
            var title = GetExactTitleCurrencyName(_metaModel.CurrencyId);

            ReportTitle = string.Format(ViewResources.CasinoProgressiveWins + ", {0} --> {1} - {2}", fromdate, todate, title);
            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 2);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        protected void AssignCommonData(IRow row, CasinoProgressiveWins data)
        {
            row.Cells[ColumnIndex].SetCellValue(data.Index);
            row.Cells[ColumnExternalId].SetCellValue(data.ExternalId);
            row.Cells[ColumnSystemId].SetCellValue(data.SystemId);
            row.Cells[ColumnDate].SetCellValue(data.WinlostDate.ToShortDateString());
            row.Cells[ColumnIndex].SetCellValue(data.Index);
            row.Cells[ColumnDate].SetCellValue(data.WinlostDate);
            row.Cells[ColumnExternalId].SetCellValue(data.ExternalId);
            row.Cells[ColumnSystemId].SetCellValue(data.SystemId);
            row.Cells[ColumnRefNo].SetCellValue(data.RefNo);
            row.Cells[ColumnChoice].SetCellValue(GetChoiceText(data));
            row.Cells[ColumnAmount].SetCellValue(data.WinAmount);
        }

        private HSSFRichTextString GetChoiceText(CasinoProgressiveWins data)
        {
            var gameName = CacheRef.GetMicroGamingGameName(data.TransDesc);
            var betTypeName = CacheRef.GetBettype(data.BetType);
            var sportTypeName = CacheRef.GetSportName(data.SportType);

            string choiceText = string.Format("{0}\n{1} {2}", gameName, sportTypeName, betTypeName);

            var result = new HSSFRichTextString(choiceText);
            result.ApplyFont(0, gameName.Length, _boldBlackFont);
            result.ApplyFont(gameName.Length, gameName.Length + sportTypeName.Length + 1, _boldBlueFont);
            return result;
        }

        private void InitializeFonts()
        {
            _boldBlackFont = this.Workbook.CreateFont();
            _boldBlackFont.Boldweight = (short)FontBoldWeight.Bold;

            _boldBlueFont = this.Workbook.CreateFont();
            _boldBlueFont.Boldweight = (short)FontBoldWeight.Bold;
            _boldBlueFont.Color = HSSFColor.LightBlue.Index;
        }
    }
}