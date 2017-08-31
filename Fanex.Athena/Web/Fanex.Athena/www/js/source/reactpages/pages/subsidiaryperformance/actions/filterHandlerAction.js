import * as types from './types.js'

export const setSubsidiary = (subsidiary) => {
    return {
        type: types.SET_SUBSIDIARY,
        data: subsidiary
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

export const changeConvertAll = (checked) => {
    return {
        type: types.CHANGE_CONVERT_ALL,
        data: checked
    }
}