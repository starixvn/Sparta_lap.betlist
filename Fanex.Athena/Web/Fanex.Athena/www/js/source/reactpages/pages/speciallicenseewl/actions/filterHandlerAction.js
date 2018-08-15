import * as types from './types.js'

export const setSelectedDrawFilter = (value) => {
    return {
        type: types.SET_DRAWFILTER,
        data: value
    }
}

export const setSelectedReportBy = (value) => {
    return {
        type: types.SET_REPORTBY,
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

export const setParams = (params) => {
    return {
        type: types.SET_PARAMS,
        data: params
    }
}

export const setReportTitle = (title) => {
    return {
        type: types.SET_REPORT_TITLE,
        data: title
    }
}