import Excel from 'lrflib/excel'
import LangHelper from 'lrflib/langHelper'
import * as excelStyle from 'lrflib/excelStyle'

const COLUMN_COUNT = 7;
const START_OF_DATA_ROW = 1;

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
        this._createExcelConfig();
    }

    _createMainData() {
        const mainData = this.gridData.Data;

        for (let item of mainData) {
            const details = item.Details;

            const subsidiaryCell = Excel.getCellAddress({ c: 0, r: this.currentRowIndex });
            const currencyCell = Excel.getCellAddress({ c: 1, r: this.currentRowIndex });

            this.worksheet[subsidiaryCell] = { t: "s", v: item.Subsidiary, s: excelStyle.ROW_SPAN_CELL_STYLE };
            this.worksheet[currencyCell] = { t: "s", v: item.Currency, s: excelStyle.ROW_SPAN_CENTER_CELL_STYLE };

            this.mergeConfig.push({ s: { r: this.currentRowIndex, c: 0 }, e: { r: this.currentRowIndex + details.length - 1, c: 0 } });
            this.mergeConfig.push({ s: { r: this.currentRowIndex, c: 1 }, e: { r: this.currentRowIndex + details.length - 1, c: 1 } });

            for (let detailIndex = 0; detailIndex < details.length; detailIndex++) {
                const detail = details[detailIndex];

                const productCell = Excel.getCellAddress({ c: 2, r: this.currentRowIndex });
                const betCountCell = Excel.getCellAddress({ c: 3, r: this.currentRowIndex });
                const turnoverCell = Excel.getCellAddress({ c: 4, r: this.currentRowIndex });
                const customerWinLossCell = Excel.getCellAddress({ c: 5, r: this.currentRowIndex });
                const subsidiaryWinLossCell = Excel.getCellAddress({ c: 6, r: this.currentRowIndex });

                this.worksheet[productCell] = { t: "s", v: detail.Product };
                this.worksheet[betCountCell] = { t: "n", v: detail.BetCount, s: excelStyle.NUMBER_CELL_STYLE };
                this.worksheet[turnoverCell] = { t: "n", v: detail.Turnover, s: excelStyle.DECIMAL_CELL_STYLE };
                this.worksheet[customerWinLossCell] = { t: "n", v: detail.CustomerWinLoss, s: excelStyle.DECIMAL_CELL_STYLE };
                this.worksheet[subsidiaryWinLossCell] = { t: "n", v: detail.SubsidiaryWinLoss, s: excelStyle.DECIMAL_CELL_STYLE };

                this.currentRowIndex++;
            }
        }
    }

    _createFooterData() {
        const totalDataList = this.gridData.Total;

        for (let item of totalDataList) {
            const subsidiaryCell = Excel.getCellAddress({ c: 0, r: this.currentRowIndex });
            const currencyCell = Excel.getCellAddress({ c: 1, r: this.currentRowIndex });
            const betCountCell = Excel.getCellAddress({ c: 3, r: this.currentRowIndex });
            const turnoverCell = Excel.getCellAddress({ c: 4, r: this.currentRowIndex });
            const customerWinLossCell = Excel.getCellAddress({ c: 5, r: this.currentRowIndex });
            const subsidiaryWinLossCell = Excel.getCellAddress({ c: 6, r: this.currentRowIndex });

            this.worksheet[subsidiaryCell] = { t: "s", v: `${LangHelper.getSingleResource("Total")} ${item.Subsidiary}`, s: excelStyle.TOTAL_CELL_STYLE };
            this.worksheet[currencyCell] = { t: "s", v: `${LangHelper.getSingleResource("Based")} ${item.CurrencyName}`, s: excelStyle.TOTAL_CELL_STYLE };
            this.worksheet[betCountCell] = { t: "n", v: item.BetCount, s: excelStyle.TOTAL_NUMBER_CELL_STYLE };
            this.worksheet[turnoverCell] = { t: "n", v: item.Turnover, s: excelStyle.TOTAL_DECIMAL_CELL_STYLE };
            this.worksheet[customerWinLossCell] = { t: "n", v: item.CustomerWinLoss, s: excelStyle.TOTAL_DECIMAL_CELL_STYLE };
            this.worksheet[subsidiaryWinLossCell] = { t: "n", v: item.SubsidiaryWinLoss, s: excelStyle.TOTAL_DECIMAL_CELL_STYLE };

            this.currentRowIndex++;
        }
    }

    _createGrandTotalData() {
        const grandTotal = this.gridData.GrandTotal;

        const subsidiaryCell = Excel.getCellAddress({ c: 0, r: this.currentRowIndex });
        const currencyCell = Excel.getCellAddress({ c: 1, r: this.currentRowIndex });
        const betCountCell = Excel.getCellAddress({ c: 3, r: this.currentRowIndex });
        const turnoverCell = Excel.getCellAddress({ c: 4, r: this.currentRowIndex });
        const customerWinLossCell = Excel.getCellAddress({ c: 5, r: this.currentRowIndex });
        const subsidiaryWinLossCell = Excel.getCellAddress({ c: 6, r: this.currentRowIndex });

        this.worksheet[subsidiaryCell] = { t: "s", v: `${LangHelper.getSingleResource("Total")}`, s: excelStyle.TOTAL_CELL_STYLE };
        this.worksheet[currencyCell] = { t: "s", v: `${LangHelper.getSingleResource("Based")} ${grandTotal.CurrencyName}`, s: excelStyle.TOTAL_CELL_STYLE };
        this.worksheet[betCountCell] = { t: "n", v: grandTotal.BetCount, s: excelStyle.TOTAL_NUMBER_CELL_STYLE };
        this.worksheet[turnoverCell] = { t: "n", v: grandTotal.Turnover, s: excelStyle.TOTAL_DECIMAL_CELL_STYLE };
        this.worksheet[customerWinLossCell] = { t: "n", v: grandTotal.CustomerWinLoss, s: excelStyle.TOTAL_DECIMAL_CELL_STYLE };
        this.worksheet[subsidiaryWinLossCell] = { t: "n", v: grandTotal.SubsidiaryWinLoss, s: excelStyle.TOTAL_DECIMAL_CELL_STYLE };
    }

    _createExcelConfig() {
        const cellStart = Excel.getCellAddress({ r: 0, c: 0 });
        const cellEnd = Excel.getCellAddress({ r: this.currentRowIndex, c: COLUMN_COUNT - 1 });

        this.worksheet["!merges"] = this.mergeConfig;
        this.worksheet["!ref"] = `${cellStart}:${cellEnd}`;
        const wscols = [{ wpx: 150 }, { wpx: 120 }, { wpx: 180 }, { wpx: 120 }, { wpx: 250 }, { wpx: 250 }, { wpx: 250 }];

        this.worksheet['!cols'] = wscols;
    }
}