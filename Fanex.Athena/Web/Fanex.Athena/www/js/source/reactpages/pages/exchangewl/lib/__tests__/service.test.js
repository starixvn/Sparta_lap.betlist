import LangHelper from '../../../../lib/langHelper'
import Site from '../../../../lib/site'
import Report from '../../../../lib/report'

import service from '../service'

describe('service ', () => {
    beforeAll(() => {
        LangHelper.getSingleResource = jest.fn(resource => {
            if (resource === "Viewin") {
                return "Viewin {0}";
            }

            return resource;
        });

        Site.convertDateToString = jest.fn((date) => date);
    })
    afterAll(() => {
        jest.resetAllMocks();
    })

    describe('getReportTitle', () => {
        it('all currencies - return correct content', () => {
            const fromDate = "01/01/2017";
            const toDate = "10/10/2017";
            const selectedCurrency = "USD";

            const expectedResult = `ExchangeWinLoss, ${fromDate} - ${toDate} - Viewin ${selectedCurrency}`
            const dateRange = { fromDate, toDate };

            const result = service.getReportTitle(dateRange, selectedCurrency);

            expect(result).toBe(expectedResult);
        })

        it('all currencies - return correct content', () => {
            const fromDate = "01/01/2017";
            const toDate = "10/10/2017";
            const selectedCurrency = "EUR";

            const expectedResult = `ExchangeWinLoss, ${fromDate} - ${toDate} - AllCurrencies`
            const dateRange = { fromDate, toDate };

            const result = service.getReportTitle(dateRange, selectedCurrency, true);

            expect(result).toBe(expectedResult);
        })
    })

    describe('getHeaderText', () => {
        it('header text should has correct content', () => {
            const expectedResult = {
                index: '#',
                date: 'Date',
                month: 'Month',
                externalId: 'ExternalId',
                systemId: 'SystemId',
                currency: 'Currency',
                turnover: 'Turnover',
                netTurnover: 'NetTurnover',
                totalTransaction: 'TotalTransaction',
                customer: 'Customer',
                customerWL: 'CustomerW_L',
                customerComm: 'CustomerComm',
                total: 'Total',
                licensee: 'Licensee',
                licenseeWL: 'LicenseeW_L',
                amountDueTo: 'AmountDueTo',
                licenseeComm: 'LicenseeComm',
            };

            const result = service.getHeaderText();

            expect(result).toEqual(expectedResult);
        })
    })

    describe('getTotalText', () => {
        it('text should has correct content', () => {
            const expectedResult = {
                total: 'Total',
                totalIn: 'Totalin'
            };

            const result = service.getTotalText();

            expect(result).toEqual(expectedResult);
        })
    })

    describe('getBreadcrumbText', () => {
        it('text should has correct content', () => {
            const expectedResult = {
                main: 'Main',
                licensee: 'Licensee',
                customer: 'Customer'
            };

            const result = service.getBreadcrumbText();

            expect(result).toEqual(expectedResult);
        })
    })

    describe('showErrorMsg', () => {
        it('alert correct message', () => {
            window.alert = jest.fn();

            service.showErrorMsg();

            expect(alert).toHaveBeenCalledWith('ErrorProcessingMsg');
        })
    })

    describe('getDrilldownDateRange', () => {
        it('call getDrilldownDateRange from Report', () => {
            Report.getDrilldownDateRange = jest.fn();

            const reportBy = 1;
            const fromDate = '01/01/2017';
            const toDate = '01/10/2017';
            const drillDownDate = '01/05/2017';

            service.getDrilldownDateRange(reportBy, fromDate, toDate, drillDownDate);

            expect(Report.getDrilldownDateRange).toHaveBeenCalledWith(reportBy, fromDate, toDate, drillDownDate);
        })
    })
})