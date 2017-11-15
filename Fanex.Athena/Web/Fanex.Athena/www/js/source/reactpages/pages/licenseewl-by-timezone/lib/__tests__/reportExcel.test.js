import Excel from '../../../../lib/excel'
import LangHelper from '../../../../lib/langHelper'
import { titleCellStyle, headerCellStyle, bodyCellStyle, totalCellStyle, grandTotalCellStyle } from '../../../../constants/excelStyle'
import service from '../../lib/service'
import ReportExcel from '../reportExcel'

import { totalData, bodyData, grandTotalData } from './fakeData';

const gridData = { Data: bodyData, Total: totalData, GrandTotal: grandTotalData };
const title = "Report Title";
const fileName = "SubsidiaryPerformance";

const COLUMN_COUNT = 7;
const TITLE_ROW_INDEX = 0;
const HEADER_ROW_INDEX = 1;
const DATA_ROW_INDEX = 2;

describe('reportExcel ', () => {
    beforeAll(() => {
        LangHelper.getSingleResource = jest.fn((resource) => resource);
        Excel.export = jest.fn();
    })
    afterAll(() => {
        jest.resetAllMocks();
    })

    describe('export', () => {
        it('no data - Do nothing', () => {
            const reportExcel = new ReportExcel(undefined, title);
            reportExcel._createTitle = jest.fn();

            reportExcel.export();

            expect(reportExcel._createTitle).not.toHaveBeenCalled();
        })

        it('get data - _Create* functions and Excel.export should be called with correct parameter', () => {
            const expectedWorksheet = {};
            const reportExcel = new ReportExcel(gridData, title);

            reportExcel._createTitle = jest.fn();
            reportExcel._createHeader = jest.fn();
            reportExcel._createSheetData = jest.fn();
            reportExcel._createExcelConfig = jest.fn();

            reportExcel.export();

            expect(reportExcel._createTitle).toHaveBeenCalled();
            expect(reportExcel._createHeader).toHaveBeenCalled();
            expect(reportExcel._createSheetData).toHaveBeenCalled();
            expect(reportExcel._createExcelConfig).toHaveBeenCalled();
            expect(Excel.export).toHaveBeenCalledWith(expectedWorksheet, fileName);
        })
    })

    describe('_createTitle', () => {
        it('mergeConfig should has correct value', () => {
            const mergeConfig = [{ s: { r: TITLE_ROW_INDEX, c: 0 }, e: { r: TITLE_ROW_INDEX, c: COLUMN_COUNT - 1 } }];

            const reportExcel = new ReportExcel({}, title);

            reportExcel._createTitle();

            expect(reportExcel.mergeConfig).toEqual(mergeConfig);
        })

        it('worksheet should has correct value', () => {
            const expectedWorksheet = {
                A1: { t: "s", v: title, s: titleCellStyle }
            };

            const reportExcel = new ReportExcel({}, title);

            reportExcel._createTitle();
            const result = reportExcel.worksheet;

            expect(result).toEqual(expectedWorksheet);
        })
    })

    describe('_createHeader', () => {
        it('worksheet should has correct value', () => {
            const expectedWorksheet = {
                A2: { t: "s", v: "Subsidiary", s: headerCellStyle },
                B2: { t: "s", v: "Currency", s: headerCellStyle },
                C2: { t: "s", v: "Product", s: headerCellStyle },
                D2: { t: "s", v: "BetCount", s: headerCellStyle },
                E2: { t: "s", v: "Turnover", s: headerCellStyle },
                F2: { t: "s", v: "CustomerW_L", s: headerCellStyle },
                G2: { t: "s", v: "SubsidiaryWL/ Comm", s: headerCellStyle }
            };

            const reportExcel = new ReportExcel({}, title);

            reportExcel._createHeader();
            const result = reportExcel.worksheet;

            expect(result).toEqual(expectedWorksheet);
        })
    })

    describe('_createMainData', () => {
        it('mergeConfig should has correct value', () => {
            const currentRowIndex = DATA_ROW_INDEX;

            const mergeConfig = [
                { s: { r: currentRowIndex, c: 0 }, e: { r: currentRowIndex + 1, c: 0 } },
                { s: { r: currentRowIndex, c: 1 }, e: { r: currentRowIndex + 1, c: 1 } }
            ];

            const reportExcel = new ReportExcel(gridData, title);

            reportExcel._createMainData();

            expect(reportExcel.mergeConfig).toEqual(mergeConfig);
        })

        it('worksheet should has correct value', () => {
            const expectedWorksheet = {
                A3: { t: "s", v: "916515", s: bodyCellStyle.rowspan },
                B3: { t: "s", v: "UUS", s: bodyCellStyle.center },
                C3: { t: "s", v: "All" },
                D3: { t: "n", v: 52, s: bodyCellStyle.number },
                E3: { t: "n", v: 322.0000, s: bodyCellStyle.decimal },
                F3: { t: "n", v: 142.3210, s: bodyCellStyle.decimal },
                G3: { t: "n", v: -141.7636, s: bodyCellStyle.negativeDecimal },
                C4: { t: "s", v: "Cash Out" },
                D4: { t: "n", v: 52, s: bodyCellStyle.number },
                E4: { t: "n", v: 322.0000, s: bodyCellStyle.decimal },
                F4: { t: "n", v: 142.3210, s: bodyCellStyle.decimal },
                G4: { t: "n", v: 141.7636, s: bodyCellStyle.decimal }
            };

            const reportExcel = new ReportExcel(gridData, title);

            reportExcel._createMainData();
            const result = reportExcel.worksheet;

            expect(result).toEqual(expectedWorksheet);
        })
    })

    describe('_createFooterData', () => {
        it('worksheet should has correct value', () => {
            const expectedWorksheet = {
                A3: { t: "s", v: "Total 858249", s: totalCellStyle.default },
                B3: { t: "s", v: "Based EUR", s: totalCellStyle.default },
                C3: { t: "s", v: "", s: undefined },
                D3: { t: "n", v: 894, s: totalCellStyle.number },
                E3: { t: "n", v: 0.0, s: totalCellStyle.decimal },
                F3: { t: "n", v: 0.0, s: totalCellStyle.decimal },
                G3: { t: "n", v: 0.0, s: totalCellStyle.decimal },
                A4: { t: "s", v: "Total 916515", s: totalCellStyle.default },
                B4: { t: "s", v: "Based EUR", s: totalCellStyle.default },
                C4: { t: "s", v: "", s: undefined },
                D4: { t: "n", v: 99, s: totalCellStyle.number },
                E4: { t: "n", v: 0.0, s: totalCellStyle.decimal },
                F4: { t: "n", v: 0.0, s: totalCellStyle.decimal },
                G4: { t: "n", v: -141.7636, s: totalCellStyle.negativeDecimal }
            };

            const reportExcel = new ReportExcel(gridData, title);

            reportExcel._createFooterData();
            const result = reportExcel.worksheet;

            expect(result).toEqual(expectedWorksheet);
        })
    })

    describe('_createGrandTotalData', () => {
        it('worksheet should has correct value', () => {
            const expectedWorksheet = {
                A3: { t: "s", v: "Total", s: grandTotalCellStyle.default },
                B3: { t: "s", v: "Based EUR", s: grandTotalCellStyle.default },
                C3: { t: "s", v: "", s: grandTotalCellStyle.default },
                D3: { t: "n", v: 13286376, s: grandTotalCellStyle.number },
                E3: { t: "n", v: 405367913.09429135, s: grandTotalCellStyle.decimal },
                F3: { t: "n", v: -14046804.028517062, s: grandTotalCellStyle.decimal },
                G3: { t: "n", v: 14046469.750504408, s: grandTotalCellStyle.decimal }
            };

            const reportExcel = new ReportExcel(gridData, title);

            reportExcel._createGrandTotalData();
            const result = reportExcel.worksheet;

            expect(result).toEqual(expectedWorksheet);
        })
    })

    describe('_createSheetData', () => {
        it('worksheet should has correct value', () => {
            const expectedWorksheet = {
                A3: { t: "s", v: "916515", s: bodyCellStyle.rowspan },
                B3: { t: "s", v: "UUS", s: bodyCellStyle.center },
                C3: { t: "s", v: "All" },
                D3: { t: "n", v: 52, s: bodyCellStyle.number },
                E3: { t: "n", v: 322.0000, s: bodyCellStyle.decimal },
                F3: { t: "n", v: 142.3210, s: bodyCellStyle.decimal },
                G3: { t: "n", v: -141.7636, s: bodyCellStyle.negativeDecimal },
                C4: { t: "s", v: "Cash Out" },
                D4: { t: "n", v: 52, s: bodyCellStyle.number },
                E4: { t: "n", v: 322.0000, s: bodyCellStyle.decimal },
                F4: { t: "n", v: 142.3210, s: bodyCellStyle.decimal },
                G4: { t: "n", v: 141.7636, s: bodyCellStyle.decimal },

                A5: { t: "s", v: "Total 858249", s: totalCellStyle.default },
                B5: { t: "s", v: "Based EUR", s: totalCellStyle.default },
                C5: { t: "s", v: "", s: undefined },
                D5: { t: "n", v: 894, s: totalCellStyle.number },
                E5: { t: "n", v: 0.0, s: totalCellStyle.decimal },
                F5: { t: "n", v: 0.0, s: totalCellStyle.decimal },
                G5: { t: "n", v: 0.0, s: totalCellStyle.decimal },
                A6: { t: "s", v: "Total 916515", s: totalCellStyle.default },
                B6: { t: "s", v: "Based EUR", s: totalCellStyle.default },
                C6: { t: "s", v: "", s: undefined },
                D6: { t: "n", v: 99, s: totalCellStyle.number },
                E6: { t: "n", v: 0.0, s: totalCellStyle.decimal },
                F6: { t: "n", v: 0.0, s: totalCellStyle.decimal },
                G6: { t: "n", v: -141.7636, s: totalCellStyle.negativeDecimal },

                A7: { t: "s", v: "Total", s: grandTotalCellStyle.default },
                B7: { t: "s", v: "Based EUR", s: grandTotalCellStyle.default },
                C7: { t: "s", v: "", s: grandTotalCellStyle.default },
                D7: { t: "n", v: 13286376, s: grandTotalCellStyle.number },
                E7: { t: "n", v: 405367913.09429135, s: grandTotalCellStyle.decimal },
                F7: { t: "n", v: -14046804.028517062, s: grandTotalCellStyle.decimal },
                G7: { t: "n", v: 14046469.750504408, s: grandTotalCellStyle.decimal }
            };

            const reportExcel = new ReportExcel(gridData, title);

            reportExcel._createSheetData();
            const result = reportExcel.worksheet;

            expect(result).toEqual(expectedWorksheet);
        })

        it('currentRowIndex should has correct value', () => {
            const expectedCurrentRowIndex = 6;
            const reportExcel = new ReportExcel(gridData, title);

            reportExcel._createSheetData();
            const result = reportExcel.currentRowIndex;

            expect(result).toEqual(expectedCurrentRowIndex);
        })
    })

    describe('_createExcelConfig', () => {
        it('worksheet should has correct value', () => {
            const mergeConfig = ["merge config"];
            const wscols = [{ wpx: 150 }, { wpx: 120 }, { wpx: 180 }, { wpx: 120 }, { wpx: 250 }, { wpx: 250 }, { wpx: 250 }];

            const expectedWorksheet = {
                "!merges": mergeConfig,
                "!ref": "A1:G6",
                "!cols": wscols
            };

            const reportExcel = new ReportExcel(undefined, title);
            reportExcel.currentRowIndex = 5;
            reportExcel.mergeConfig = mergeConfig;
            reportExcel._createExcelConfig();

            const result = reportExcel.worksheet;

            expect(result).toEqual(expectedWorksheet);
        })
    })
})