import * as types from './types.js'

export const setReportBy = (value) => {
    return {
        type: types.SET_REPORTBY,
        data: value
    }
}

export const setExcludeTestData = (value) => {
    return {
        type: types.SET_EXCLUDE_TESTDATA,
        data: value
    }
}

export const setDateRange = (dateRange) => {
    const range = {
        fromDate: dateRange.startDate,
        toDate: dateRange.endDate
    }

    return {
        type: types.SET_DATE_RANGE,
        data: range
    }
}