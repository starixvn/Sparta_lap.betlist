import * as types from './types.js'

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

export const setSelectedNotificationType = (notificationTypesKeys) => {
    return {
        type: types.SET_NOTIFICATION_TYPE,
        data: notificationTypesKeys
    }
}