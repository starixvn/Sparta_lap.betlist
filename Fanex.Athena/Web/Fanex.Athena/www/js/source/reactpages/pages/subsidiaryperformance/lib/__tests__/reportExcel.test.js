import Excel from '../../../../lib/excel'
import LangHelper from '../../../../lib/langHelper'
import { titleCellStyle, headerCellStyle, bodyCellStyle, totalCellStyle, grandTotalCellStyle } from '../../../../constants/excelStyle'
import service from '../../lib/service'
import ReportExcel from '../reportExcel'

jest.mock('../../../../lib/excel');

import { totalData, bodyData, grandTotalData } from './fakeData';

const gridData = { Data: bodyData, ToTal: totalData, GrandTotal: grandTotalData };
const title = "Report Title";
const fileName = "SubsidiaryPerformance";

const COLUMN_COUNT = 7;
const TITLE_ROW_INDEX = 0;
const HEADER_ROW_INDEX = 1;

describe('reportExcel ', () => {
    beforeAll(() => {
        LangHelper.getSingleResource = jest.fn((resource) => resource);
        Excel.export = jest.fn();
    })
    afterAll(() => {
        jest.resetAllMocks();
    })

    it('export - Excel.export should be called with correct parameter', () => {
        const expectedWorksheet = {};
        const reportExcel = new ReportExcel(gridData, title);

        reportExcel._createTitle = jest.fn();
        reportExcel._createHeader = jest.fn();
        reportExcel._createSheetData = jest.fn();
        reportExcel._createExcelConfig = jest.fn();

        reportExcel.export();

        expect(Excel.export).toHaveBeenCalledWith(expectedWorksheet, fileName);
    })

    describe('_createTitle', () => {
        it('mergeConfig should be correct value', () => {
            const mergeConfig = [{ s: { r: TITLE_ROW_INDEX, c: 0 }, e: { r: TITLE_ROW_INDEX, c: COLUMN_COUNT - 1 } }];

            const reportExcel = new ReportExcel({}, title);

            reportExcel._createTitle();

            expect(reportExcel.mergeConfig).toEqual(mergeConfig);
        })

        it('worksheet should be correct value', () => {
            const expectedWorksheet = {
                A1: { t: "s", v: title, s: titleCellStyle }
            };

            const reportExcel = new ReportExcel({}, title);
            Excel.getCellAddress = jest.fn(() => "A1");

            reportExcel._createTitle();

            expect(reportExcel.worksheet).toEqual(expectedWorksheet);

            Excel.getCellAddress.mockReset();
        })
    })
})