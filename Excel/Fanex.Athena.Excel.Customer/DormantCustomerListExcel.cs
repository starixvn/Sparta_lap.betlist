// --------------------------------------------------------------------------
// <copyright file="CustomerListExcel.cs" company="">
// Copyright (coffee) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Customer
{
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using Fanex.Athena.Excel.Base.Extension;
    using Model.Customer.DTO;
    using NPOI.SS.UserModel;
    using Resources;

    public class DormantCustomerListExcel : ReportBase
    {
        private const int RowContentTemplate = 3;
        private const int ExternalIDColumn = 0;
        private const int SystemIDColumn = 1;
        private const int BalanceColumn = 2;
        private const int ArchivedDateColumn = 3;

        private const int ColumnTemplateCount = 4;

        private readonly string _reporTitle;
        private readonly IList<string> _headerTexts;

        private readonly IEnumerable<DormantCustomer> _dataList;

        public DormantCustomerListExcel(string templatePath, string reportTitle, IEnumerable<DormantCustomer> dataList)
            : base(templatePath, dataList.Count())
        {
            _dataList = dataList;
            _reporTitle = reportTitle;
            _headerTexts = new List<string>
            {
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.Balance,
                ViewResources.ArchivedDate
            };
        }

        protected override void AssignHeader(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate);

            for (int i = 0; i < _headerTexts.Count; i++)
            {
                row.Cells[i].SetCellValue(_headerTexts[i]);
            }
        }

        protected override void AssignSheetData(ISheet sheet)
        {
            var rowIndex = RowContentTemplate;
            foreach (var item in _dataList)
            {
                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, rowIndex++);

                row.Cells[ExternalIDColumn].SetCellValue(item.ExternalId);
                row.Cells[SystemIDColumn].SetCellValue(item.SystemId);
                row.Cells[BalanceColumn].SetCellValue(item.Balance.ToString("#,##0.00"));
                row.Cells[ArchivedDateColumn].SetCellValue(item.ArchivedDate.ToString("MM/dd/yyyy"));
            }
        }

        protected override void AssignTitle(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, ColumnTemplateCount, _reporTitle);

            /*Freeze 2 rows title and header*/
            sheet.CreateFreezePane(0, RowContentTemplate);

            /*Remove content below header before insert new data.*/
            ClearTemplate(sheet, RowContentTemplate);
        }
    }
}