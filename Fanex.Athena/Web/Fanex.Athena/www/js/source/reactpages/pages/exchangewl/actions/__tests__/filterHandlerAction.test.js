import * as actions from '../filterHandlerAction'
import * as types from '../types'

describe('filterHandlerAction', () => {
    it('setReportBy - should create an correct action', () => {
        const data = { value: 1 };
        const expectedAction = {
            type: types.SET_REPORTBY,
            data: data
        };

        const result = actions.setReportBy(data);

        expect(result).toEqual(expectedAction);
    })

    it('setDateRange - should create an correct action', () => {
        const dateRange = { startDate: '01/01/2017', endDate: '10/10/2017' };

        const range = {
            fromDate: dateRange.startDate,
            toDate: dateRange.endDate
        };

        const expectedAction = {
            type: types.SET_DATE_RANGE,
            data: range
        }

        expect(actions.setDateRange(dateRange)).toEqual(expectedAction);
    })

    it('setParams - should create an correct action', () => {
        const data = { param1: 1 };
        const expectedAction = {
            type: types.SET_PARAM,
            data: data
        };

        const result = actions.setParams(data);

        expect(result).toEqual(expectedAction);
    })
})