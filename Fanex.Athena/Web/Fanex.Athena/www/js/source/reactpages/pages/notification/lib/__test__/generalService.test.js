import LangHelper from '../../../../lib/langHelper'
import Site from '../../../../lib/site'

import generalService from '../generalService'

describe('generalService', () => {
    beforeAll(() => {
        LangHelper.getSingleResource = jest.fn((resource) => resource);

        Site.convertDateToString = jest.fn((date) => date);
    })

    afterAll(() => {
        jest.resetAllMocks();
    })

    describe('getReportTitle', () => {
        it('returns correct title', () => {
            const dateRange = {
                fromDate: '01/01/2017',
                toDate: '01/10/2017'
            };
            const expectedTitle = 'General, 01/01/2017 - 01/10/2017';

            var result = generalService.getReportTitle(dateRange);

            expect(result).toEqual(expectedTitle);
        });
    })

    describe('getHeaderText', () => {
        it('returns correct header names', () => {
            const expectedHeader = {
                index: '#',
                type: 'Type',
                dateTime: 'Date/Time',
                content: 'Content'
            }

            var result = generalService.getHeaderText();

            expect(result).toEqual(expectedHeader);
        });
    })
})