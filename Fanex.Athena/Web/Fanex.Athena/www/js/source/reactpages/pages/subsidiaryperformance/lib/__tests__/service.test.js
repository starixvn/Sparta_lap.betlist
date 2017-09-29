import LangHelper from '../../../../lib/langHelper'
import Site from '../../../../lib/site'

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
        it('return correct content', () => {
            const fromDate = "09/01/2017";
            const toDate = "09/30/2017";
            const selectedBaseCurrency = "EUR";

            const expectedResult = `SubsidiaryPerformance, ${fromDate} - ${toDate} - Viewin ${selectedBaseCurrency}`
            const dateRange = { fromDate, toDate };

            const result = service.getReportTitle(dateRange, selectedBaseCurrency);

            expect(result).toBe(expectedResult);
        })
    })

    describe('getHeaderText', () => {
        it('header text should has correct content', () => {
            const expectedResult = {
                subsidiary: "Subsidiary",
                currency: "Currency",
                product: "Product",
                betCount: "BetCount",
                turnover: "Turnover",
                customerWL: "CustomerW_L",
                subsidiaryWL_comm: "SubsidiaryWL/ Comm"
            }

            const result = service.getHeaderText();

            expect(result).toEqual(expectedResult);
        })
    })
})