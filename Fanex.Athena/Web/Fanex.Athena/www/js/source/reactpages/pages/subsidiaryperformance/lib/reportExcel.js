import Excel from '../../../lib/excel'
import LangHelper from '../../../lib/langHelper'
import { titleCellStyle, headerCellStyle, bodyCellStyle, totalCellStyle, grandTotalCellStyle } from '../../../constants/excelStyle'
import service from '../lib/service'

const COLUMN_COUNT = 7;
const TITLE_ROW_INDEX = 0;
const HEADER_ROW_INDEX = 1;
const DATA_ROW_INDEX = 2;

const SUBSIDIARY_COLUMN_INDEX = 0;
const CURRENCY_COLUMN_INDEX = 1;
const PRODUCT_COLUMN_INDEX = 2;
const BETCOUNT_COLUMN_INDEX = 3;
const TURNOVER_COLUMN_INDEX = 4;
const CUSTOMERWL_COLUMN_INDEX = 5;
const SUBSIDIARYWL_COLUMN_INDEX = 6;

const MAIN_SECTION_STYLE = "mainSectionStyle";
const TOTAL_SECTION_STYLE = "totalSectionStyle";
const GRAND_TOTAL_SECTION_STYLE = "grandTotalSectionStyle";

const fields = {
    subsidiary: "Subsidiary",
    currencyName: "CurrencyName",
    product: "Product",
    betCount: "BetCount",
    turnover: "Turnover",
    customerWinLoss: "CustomerWinLoss",
    subsidiaryWinLoss: "SubsidiaryWinLoss",
}

const columnConfigs = [{
        field: fields.subsidiary,
        [MAIN_SECTION_STYLE]: bodyCellStyle.rowspan,
        [TOTAL_SECTION_STYLE]: totalCellStyle.default,
        [GRAND_TOTAL_SECTION_STYLE]: grandTotalCellStyle.default
    },
    {
        field: fields.currencyName,
        [MAIN_SECTION_STYLE]: bodyCellStyle.center,
        [TOTAL_SECTION_STYLE]: totalCellStyle.default,
        [GRAND_TOTAL_SECTION_STYLE]: grandTotalCellStyle.default
    },
    {
        field: fields.product,
        [GRAND_TOTAL_SECTION_STYLE]: grandTotalCellStyle.default
    },
    {
        field: fields.betCount,
        [MAIN_SECTION_STYLE]: bodyCellStyle.number,
        [TOTAL_SECTION_STYLE]: totalCellStyle.number,
        [GRAND_TOTAL_SECTION_STYLE]: grandTotalCellStyle.number
    },
    {
        field: fields.turnover,
        [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
        [TOTAL_SECTION_STYLE]: totalCellStyle.decimal,
        [GRAND_TOTAL_SECTION_STYLE]: grandTotalCellStyle.decimal
    },
    {
        field: fields.customerWinLoss,
        [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
        [TOTAL_SECTION_STYLE]: totalCellStyle.decimal,
        [GRAND_TOTAL_SECTION_STYLE]: grandTotalCellStyle.decimal
    },
    {
        field: fields.subsidiaryWinLoss,
        [MAIN_SECTION_STYLE]: { positive: bodyCellStyle.decimal, negative: bodyCellStyle.negativeDecimal },
        [TOTAL_SECTION_STYLE]: { positive: totalCellStyle.decimal, negative: totalCellStyle.negativeDecimal },
        [GRAND_TOTAL_SECTION_STYLE]: { positive: grandTotalCellStyle.decimal, negative: grandTotalCellStyle.negativeDecimal }
    }
]

export default class ReportExcel {
    constructor(gridData = {}, title, fileName = "SubsidiaryPerformance") {
        this.worksheet = {};
        this.title = title;
        this.fileName = fileName;
        this.gridData = gridData;
        this.currentRowIndex = DATA_ROW_INDEX;
        this.mergeConfig = [];
    }

    export () {
        if (!this.gridData.Data || !this.gridData.Data.length) {
            return;
        }

        this._createTitle();
        this._createHeader();
        this._createSheetData();
        this._createExcelConfig();

        Excel.export(this.worksheet, this.fileName);
    }

    _createTitle() {
        this.mergeConfig.push({ s: { r: TITLE_ROW_INDEX, c: 0 }, e: { r: TITLE_ROW_INDEX, c: COLUMN_COUNT - 1 } });
        const address = Excel.getCellAddress({ c: 0, r: TITLE_ROW_INDEX });
        this.worksheet[address] = { t: "s", v: this.title, s: titleCellStyle }
    }

    _createHeader() {
        const headers = service.getHeaderText();
        for (let i = 0; i < COLUMN_COUNT; i++) {
            const address = Excel.getCellAddress({ c: i, r: HEADER_ROW_INDEX });
            this.worksheet[address] = { t: "s", v: headers[i], s: headerCellStyle }
        }
    }

    _createSheetData() {
        this._createMainData();
        this._createFooterData();
        this._createGrandTotalData();
    }

    _createMainData() {
        const mainData = this.gridData.Data;

        for (let i = 0; i < mainData.length; i++) {
            const item = mainData[i];
            const details = item.Details;

            this._createRowByColumnIndex(item, MAIN_SECTION_STYLE, SUBSIDIARY_COLUMN_INDEX, CURRENCY_COLUMN_INDEX);

            this.mergeConfig.push({ s: { r: this.currentRowIndex, c: 0 }, e: { r: this.currentRowIndex + details.length - 1, c: 0 } });
            this.mergeConfig.push({ s: { r: this.currentRowIndex, c: 1 }, e: { r: this.currentRowIndex + details.length - 1, c: 1 } });

            for (let detailIndex = 0; detailIndex < details.length; detailIndex++) {
                const detail = details[detailIndex];
                this._createRowByColumnIndex(detail, MAIN_SECTION_STYLE, PRODUCT_COLUMN_INDEX, SUBSIDIARYWL_COLUMN_INDEX);
                this.currentRowIndex++;
            }
        }
    }

    _createFooterData() {
        const totalDataList = this.gridData.Total;

        for (let i = 0; i < totalDataList.length; i++) {
            const item = totalDataList[i];
            this._createRowByColumnIndex(item, TOTAL_SECTION_STYLE, SUBSIDIARY_COLUMN_INDEX, SUBSIDIARYWL_COLUMN_INDEX);
            this.currentRowIndex++;
        }
    }

    _createGrandTotalData() {
        const grandTotal = this.gridData.GrandTotal;
        this._createRowByColumnIndex(grandTotal, GRAND_TOTAL_SECTION_STYLE, SUBSIDIARY_COLUMN_INDEX, SUBSIDIARYWL_COLUMN_INDEX);
    }

    _createExcelConfig() {
        const cellStart = Excel.getCellAddress({ r: 0, c: 0 });
        const cellEnd = Excel.getCellAddress({ r: this.currentRowIndex, c: COLUMN_COUNT - 1 });

        this.worksheet["!merges"] = this.mergeConfig;
        this.worksheet["!ref"] = `${cellStart}:${cellEnd}`;
        const wscols = [{ wpx: 150 }, { wpx: 120 }, { wpx: 180 }, { wpx: 120 }, { wpx: 250 }, { wpx: 250 }, { wpx: 250 }];

        this.worksheet['!cols'] = wscols;
    }

    _createRowByColumnIndex(item, sectionStyle, startIndex, endIndex) {
        for (let i = startIndex; i <= endIndex; i++) {
            const { cellAddress, type, value, style } = this._getCellObject(item, sectionStyle, i);
            this.worksheet[cellAddress] = { t: type, v: value, s: style };
        }
    }

    _getCellObject(item, sectionStyle, columnIndex) {
        const columnConfig = columnConfigs[columnIndex];
        const field = columnConfig.field;

        const cellAddress = Excel.getCellAddress({ c: columnIndex, r: this.currentRowIndex });
        const type = typeof item[columnConfig.field] === "number" ? "n" : "s";
        const value = item[field] !== null ? item[field] : "";

        let style = columnConfig[sectionStyle];
        if (field === fields.subsidiaryWinLoss) {
            style = value < 0 ? style.negative : style.positive;
        }

        return { cellAddress, type, value, style };
    }
}