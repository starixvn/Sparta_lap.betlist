import Excel from '../../../../lib/excel'
import LangHelper from '../../../../lib/langHelper'
import service from '../../lib/service'
import ReportExcel from '../reportExcel'
import { titleCellStyle, headerCellStyle, bodyCellStyle, totalCellStyle, grandTotalCellStyle } from '../../../../constants/excelStyle'
import { ReportLevel, ReportBy } from '../../../../constants/report'

const TITLE_ROW_INDEX = 0;
const MAX_COLUMN = 15;

describe('reportExcel ', () => {
    const rowData = {
        Date: '2017-01-01T12:00:00',
        ExternalId: 'ExternalId',
        SystemId: 'SystemId',
        CurrencyName: 'CurrencyName',
        CurrencyId: 1,
        Turnover: 'Turnover',
        NetTurnover: 'NetTurnover',
        TotalTransaction: 'TotalTransaction',
        CustomerWL: 'CustomerWL',
        CustomerComm: 'CustomerComm',
        CustomerWLTotal: 'CustomerWLTotal',
        LicenseeWL: 'LicenseeWL',
        AmountDueTo: 'AmountDueTo',
        LicenseeComm: 'LicenseeComm',
        LicenseeWLTotal: 'LicenseeWLTotal'
    }
    
    const gridData = {
        DataList: [rowData],
        TotalDataList: [rowData],
        SummaryTotal: {
            'Currency': rowData
        }
    };

    const fileName = 'ExchangeWinLoss';
    const title = 'Exchange Win Loss Title';

    beforeAll(() => {
        LangHelper.getSingleResource = jest.fn((resource) => resource);
        Excel.export = jest.fn();
    })

    afterAll(() => {
        jest.resetAllMocks();
    })
    
    describe('export', () => {
        it('data is empty - do nothing', () => {
            const reportExcel = new ReportExcel();

            const result = reportExcel.export();

            expect(Excel.export).not.toHaveBeenCalled();
        })

        it('data is not empty - call functions to create sections for excel file', () => {
            const expectedWorksheet = {};

            const reportExcel = new ReportExcel(gridData);
            reportExcel._createHeader = jest.fn();
            reportExcel._createSheetData = jest.fn();
            reportExcel._createExcelConfig = jest.fn();

            const result = reportExcel.export();

            expect(Excel.export).toHaveBeenCalledWith(expectedWorksheet, fileName);
            expect(reportExcel._createHeader).toHaveBeenCalled();
            expect(reportExcel._createSheetData).toHaveBeenCalled();
            expect(reportExcel._createExcelConfig).toHaveBeenCalled();
        })
    })

    describe('_createHeader', () => {
        const LevelRow = 'B2';
        const NextLevelRow = 'C2';

        it('MAIN - create Date column', () => {
            const level = ReportLevel.MAIN;
            const expectedRow = { t: 's', v: 'Date', s: headerCellStyle };

            const reportExcel = new ReportExcel(gridData, title, level, false, 1);
            reportExcel._createTitle = jest.fn();

            reportExcel._createHeader();
            const dateRow = reportExcel.worksheet[LevelRow];

            expect(dateRow).toEqual(expectedRow);
        });

        it('LICENSEE - create SystemId column', () => {
            const level = ReportLevel.LICENSEE;
            const expectedRow = { t: 's', v: 'SystemId', s: headerCellStyle };

            const reportExcel = new ReportExcel(gridData, title, level, false, 1);
            reportExcel._createTitle = jest.fn();

            reportExcel._createHeader();
            const systemIdRow = reportExcel.worksheet[LevelRow];

            expect(systemIdRow).toEqual(expectedRow);
        });

        it('CUSTOMER - create ExternalId and SystemId columns', () => {
            const level = ReportLevel.CUSTOMER;
            const expectedExternalIdRow = { t: 's', v: 'ExternalId', s: headerCellStyle };
            const expectedSystemIdRow = { t: 's', v: 'SystemId', s: headerCellStyle };

            const reportExcel = new ReportExcel(gridData, title, level, false, 1);
            reportExcel._createTitle = jest.fn();

            reportExcel._createHeader();
            const externalIdRow = reportExcel.worksheet[LevelRow];
            const systemIdRow = reportExcel.worksheet[NextLevelRow];

            expect(externalIdRow).toEqual(expectedExternalIdRow);
            expect(systemIdRow).toEqual(expectedSystemIdRow);
        });

        it('isAllowedCustomerComm is false - do not create CustomerWL, CustomerComm and CustomerWLTotal column', () => {
            const level = ReportLevel.MAIN;
            const CustomerRow = 'G2';
            const CustomerWLRow = 'G3';
            const expectedCustomerRow = { t: 's', v: 'CustomerW_L', s: headerCellStyle };

            const reportExcel = new ReportExcel(gridData, title, level, false, 1);
            reportExcel._createTitle = jest.fn();

            reportExcel._createHeader();
            const customerRowdRow = reportExcel.worksheet[CustomerRow];
            const customerWLRow = reportExcel.worksheet[CustomerWLRow];

            expect(customerRowdRow).toEqual(expectedCustomerRow);
            expect(customerWLRow).toEqual(undefined);
        });

        it('isAllowedCustomerComm is true - create CustomerWL, CustomerComm and CustomerWLTotal column', () => {
            const level = ReportLevel.MAIN;
            const CustomerRow = 'G2';
            const CustomerWLRow = 'G3';
            const expectedCustomerRow = { t: 's', v: 'Customer', s: headerCellStyle };
            const expectedCustomerWLRow = { t: 's', v: 'CustomerW_L', s: headerCellStyle };

            const reportExcel = new ReportExcel(gridData, title, level, true, 1);
            reportExcel._createTitle = jest.fn();

            reportExcel._createHeader();
            const customerRowdRow = reportExcel.worksheet[CustomerRow];
            const customerWLRow = reportExcel.worksheet[CustomerWLRow];

            expect(customerRowdRow).toEqual(expectedCustomerRow);
            expect(customerWLRow).toEqual(expectedCustomerWLRow);
        });

        describe('_createTitle', () => {
            const MAX_COLUMN = 15;
            const TitleRow = 'A1';

            it('MAIN and isAllowedCustomerComm is false - create Title row with colSpan correcly', () => {
                const colSpan = MAX_COLUMN - 2; // - ExternalId - SystemId - CustomerComm - CustomerWLTotal
                const expectedRow = { t: 's', v: title, s: titleCellStyle };
                const expectedMerge = { s: { r: TITLE_ROW_INDEX, c: 0 }, e: { r: TITLE_ROW_INDEX, c: colSpan - 1 } };

                const reportExcel = new ReportExcel(gridData, title, ReportLevel.MAIN, true, 1);
                reportExcel._createHeader();

                const titleRow = reportExcel.worksheet[TitleRow];
                const mergeTitle = reportExcel.mergeConfig[reportExcel.mergeConfig.length - 1];

                expect(titleRow).toEqual(expectedRow);
                expect(mergeTitle).toEqual(expectedMerge);
            });

            it('LICENSEE and isAllowedCustomerComm is false - create Title row with colSpan correcly', () => {
                const colSpan = MAX_COLUMN - 2 - 2; // - ExternalId - Date - CustomerComm - CustomerWLTotal
                const expectedRow = { t: 's', v: title, s: titleCellStyle };
                const expectedMerge = { s: { r: TITLE_ROW_INDEX, c: 0 }, e: { r: TITLE_ROW_INDEX, c: colSpan - 1 } };

                const reportExcel = new ReportExcel(gridData, title, ReportLevel.LICENSEE, false, 1);
                reportExcel._createHeader();

                const titleRow = reportExcel.worksheet[TitleRow];
                const mergeTitle = reportExcel.mergeConfig[reportExcel.mergeConfig.length - 1];

                expect(titleRow).toEqual(expectedRow);
                expect(mergeTitle).toEqual(expectedMerge);
            });

            it('CUSTOMER and isAllowedCustomerComm is false - create Title row with colSpan correcly', () => {
                const colSpan = MAX_COLUMN - 1 - 2; // - Date - CustomerComm - CustomerWLTotal
                const expectedRow = { t: 's', v: title, s: titleCellStyle };
                const expectedMerge = { s: { r: TITLE_ROW_INDEX, c: 0 }, e: { r: TITLE_ROW_INDEX, c: colSpan - 1} };

                const reportExcel = new ReportExcel(gridData, title, ReportLevel.CUSTOMER, false, 1);
                reportExcel._createHeader();

                const titleRow = reportExcel.worksheet[TitleRow];
                const mergeTitle = reportExcel.mergeConfig[reportExcel.mergeConfig.length - 1];

                expect(titleRow).toEqual(expectedRow);
                expect(mergeTitle).toEqual(expectedMerge);
            });

        });
    })
})