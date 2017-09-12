import * as types from './types.js'

export const setSelectedSubsidiary = (subsidiaryKeys) => {
    return {
        type: types.SET_SUBSIDIARY,
        data: subsidiaryKeys
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

export const setValidDateRange = (isValid) => {
    return {
        type: types.SET_VALID_DATE_RANGE,
        data: isValid
    }
}

export const changeConvertAll = (checked) => {
    return {
        type: types.CHANGE_CONVERT_ALL,
        data: checked
    }
}