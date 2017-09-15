import Excel from 'lrflib/excel'
import LangHelper from 'lrflib/langHelper'
import * as excelStyle from 'lrflib/excelStyle'

const COLUMN_COUNT = 7;
const START_OF_DATA_ROW = 1;

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

const columnConfigs = [{
        field: "Subsidiary",
        [MAIN_SECTION_STYLE]: "ROW_SPAN_CELL_STYLE",
        [TOTAL_SECTION_STYLE]: "TOTAL_CELL_STYLE",
        [GRAND_TOTAL_SECTION_STYLE]: "GRAND_TOTAL_CELL_STYLE"
    },
    {
        field: "CurrencyName",
        [MAIN_SECTION_STYLE]: "ROW_SPAN_CENTER_CELL_STYLE",
        [TOTAL_SECTION_STYLE]: "TOTAL_CELL_STYLE",
        [GRAND_TOTAL_SECTION_STYLE]: "GRAND_TOTAL_CELL_STYLE"
    },
    {
        field: "Product",
        [GRAND_TOTAL_SECTION_STYLE]: "GRAND_TOTAL_CELL_STYLE"
    },
    {
        field: "BetCount",
        [MAIN_SECTION_STYLE]: "NUMBER_CELL_STYLE",
        [TOTAL_SECTION_STYLE]: "TOTAL_NUMBER_CELL_STYLE",
        [GRAND_TOTAL_SECTION_STYLE]: "GRAND_TOTAL_NUMBER_CELL_STYLE"
    },
    {
        field: "Turnover",
        [MAIN_SECTION_STYLE]: "DECIMAL_CELL_STYLE",
        [TOTAL_SECTION_STYLE]: "TOTAL_DECIMAL_CELL_STYLE",
        [GRAND_TOTAL_SECTION_STYLE]: "GRAND_TOTAL_DECIMAL_CELL_STYLE"
    },
    {
        field: "CustomerWinLoss",
        [MAIN_SECTION_STYLE]: "DECIMAL_CELL_STYLE",
        [TOTAL_SECTION_STYLE]: "TOTAL_DECIMAL_CELL_STYLE",
        [GRAND_TOTAL_SECTION_STYLE]: "GRAND_TOTAL_DECIMAL_CELL_STYLE"
    },
    {
        field: "SubsidiaryWinLoss",
        [MAIN_SECTION_STYLE]: "DECIMAL_CELL_STYLE",
        [TOTAL_SECTION_STYLE]: "TOTAL_DECIMAL_CELL_STYLE",
        [GRAND_TOTAL_SECTION_STYLE]: "GRAND_TOTAL_DECIMAL_CELL_STYLE"
    }
]

const HEADERS = [
    LangHelper.getSingleResource("Subsidiary"), LangHelper.getSingleResource("Currency"),
    LangHelper.getSingleResource("Product"), LangHelper.getSingleResource("BetCount"),
    LangHelper.getSingleResource("Turnover"), LangHelper.getSingleResource("CustomerWinLoss"),
    `${LangHelper.getSingleResource("SubsidiaryWL")}/ ${LangHelper.getSingleResource("Comm")}`
];

export default class ReportExcel {
    constructor(gridData = [], fileName = "SubsidiaryPerformance") {
        this.worksheet = {};
        this.fileName = fileName;
        this.gridData = gridData;
        this.currentRowIndex = START_OF_DATA_ROW;
        this.mergeConfig = [];
    }

    export () {
        this._createHeader();
        this._createSheetData();
        this._createExcelConfig();

        Excel.export(this.worksheet, this.fileName);
    }

    _createHeader() {
        for (let i = 0; i < COLUMN_COUNT; i++) {
            const address = Excel.getCellAddress({ c: i, r: 0 });
            this.worksheet[address] = { t: "s", v: HEADERS[i], s: excelStyle.HEADER_STYLE }
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

        const styleName = columnConfig[sectionStyle];
        const style = styleName ? excelStyle[styleName] : undefined;

        const cellAddress = Excel.getCellAddress({ c: columnIndex, r: this.currentRowIndex });
        const type = typeof item[columnConfig.field] === "number" ? "n" : "s";
        const value = item[field] !== null ? item[field] : "";

        return { cellAddress, type, value, style };
    }
}